using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfStudyingSystem.Resources;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Classes.BaseEntities.Sets;
using WpfStudyingSystem.Script.Classes.Constructor.Builders;
using WpfStudyingSystem.Script.Classes.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Usables;
using WpfStudyingSystem.Script.Interfaces;
using WpfStudyingSystem.Script.ViewModels;
using System.IO;
using WpfStudyingSystem.Script.Exporting;
using Microsoft.Win32;

namespace WpfStudyingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentCourseId;
        private int currentStudentsCourseId;
        private bool redactMode;
        private Assignment currentAssignment;
        private bool courseRedactMode;
        private Course currentCourse;
        private int gradesFilterMode = 0;
        private Script.Other.Sets.StudentGradeInfo? currentGradeInfo;

        public MainWindow()
        {
            InitializeComponent();
            GradeDialogOverlay.Visibility = Visibility.Collapsed;
            DataContext = new MainViewModel();

            AssignmentTypeBox.ItemsSource = Enum.GetValues(typeof(AssignmentTypesEnum));
            AssignmentDialogOverlay.Visibility = Visibility.Collapsed;
        }

        private void AssignmentsCreate_Click(object sender, RoutedEventArgs e)
        {
            var course = CoursesList.SelectedItem as Course;
            if (course == null)
            {
                MessageBox.Show(Strings.Msg_SelectCourseFirst);
                return;
            }

            currentCourseId = course.Id;
            redactMode = false;
            currentAssignment = null;

            AssignmentDialogTitle.Text = Strings.Ui_Assignment_CreateTitle;
            AssignmentNameBox.Text = "";
            AssignmentDatePicker.SelectedDate = DateTime.Now;
            AssignmentTypeBox.SelectedItem = AssignmentTypesEnum.Grade;
            AssignmentDescriptionBox.Text = "";

            AssignmentDialogOverlay.Visibility = Visibility.Visible;
        }

        private void AssignmentsRedact_Click(object sender, RoutedEventArgs e)
        {
            var course = CoursesList.SelectedItem as Course;
            if (course == null)
            {
                MessageBox.Show(Strings.Msg_SelectCourseFirst);
                return;
            }

            var assignment = AssignmentsList.SelectedItem as Assignment;
            if (assignment == null)
            {
                MessageBox.Show(Strings.Msg_SelectAssignmentFirst);
                return;
            }

            currentCourseId = course.Id;
            redactMode = true;
            currentAssignment = assignment;

            AssignmentDialogTitle.Text = Strings.Ui_Assignment_RedactTitle;
            AssignmentNameBox.Text = assignment.Name;
            AssignmentDatePicker.SelectedDate = assignment.Date;
            AssignmentTypeBox.SelectedItem = assignment.Type;
            AssignmentDescriptionBox.Text = assignment.Description;

            AssignmentDialogOverlay.Visibility = Visibility.Visible;
        }

        private void CoursesDelete_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            var course = CoursesList.SelectedItem as Course;
            if (course == null)
            {
                MessageBox.Show(Strings.Msg_SelectCourseFirst);
                return;
            }

            var app = (App)Application.Current;
            var setter = app.Services.GetService<IDatabaseSetter>();
            if (setter == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            setter.RemoveCourse(course.Id);

            vm.Courses.Remove(course);
            CoursesList.SelectedItem = null;

            TeacherNameText.Text = "";
            StudentInfoText.Text = "";
            AssignmentInfoText.Text = "";

            vm.CourseStudents.Clear();
            vm.Assignments.Clear();

            GradesList.ItemsSource = null;
            GradesList.Items.Clear();

            gradesFilterMode = 0;
            FilterButton.Content = Strings.UI_Filter_All;

            currentCourseId = 0;
            currentStudentsCourseId = 0;
            currentGradeInfo = null;
            GradeDialogOverlay.Visibility = Visibility.Collapsed;
        }

        private void AssignmentsDelete_Click(object sender, RoutedEventArgs e)
        {
            var course = CoursesList.SelectedItem as Course;
            if (course == null)
            {
                MessageBox.Show(Strings.Msg_SelectCourseFirst);
                return;
            }

            var assignment = AssignmentsList.SelectedItem as Assignment;
            if (assignment == null)
            {
                MessageBox.Show(Strings.Msg_SelectAssignmentFirst);
                return;
            }

            var app = (App)Application.Current;
            var setter = app.Services.GetService<IDatabaseSetter>();
            if (setter == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            setter.RemoveAssignment(assignment.Id);

            var vm = DataContext as MainViewModel;
            if (vm != null) vm.LoadAssignments(course.Id);

            AssignmentInfoText.Text = "";
        }

        private void AssignmentDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            AssignmentDialogOverlay.Visibility = Visibility.Collapsed;
        }

        private void AssignmentDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (currentCourseId <= 0)
            {
                MessageBox.Show(Strings.Msg_SelectCourseFirst);
                return;
            }

            var name = (AssignmentNameBox.Text ?? "").Trim();
            if (name.Length == 0)
            {
                MessageBox.Show(Strings.Msg_NameRequired);
                return;
            }

            var date = AssignmentDatePicker.SelectedDate ?? DateTime.Now;
            var description = AssignmentDescriptionBox.Text ?? "";
            var type = (AssignmentTypesEnum)(AssignmentTypeBox.SelectedItem ?? AssignmentTypesEnum.Grade);

            var app = (App)Application.Current;
            var director = app.Services.GetService<IBuildDirector>();
            var setter = app.Services.GetService<IDatabaseSetter>();

            if (director == null || setter == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            IAssignmentBuilder builder;
            if (type == AssignmentTypesEnum.Grade) builder = new GradeAssignmentBuilder();
            else if (type == AssignmentTypesEnum.Credit) builder = new CreditAssignmentBuilder();
            else builder = new EAPAssignmentBuilder();

            var id = redactMode && currentAssignment != null ? currentAssignment.Id : -1;
            var assignment = director.BuildAssignment(builder, name, description, date, id);

            var vm = DataContext as MainViewModel;
            if (vm == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            string safeName = (name ?? "").Replace("'", "''");
            string safeDesc = (description ?? "").Replace("'", "''");
            string safeDate = date.ToString("yyyy-MM-dd HH:mm:ss");
            int typeInt = (int)type;

            if (redactMode && currentAssignment != null)
            {
                setter.UpdateAssignment(assignment);
            }
            else
            {
                setter.SetAssignment(assignment, currentCourseId);
            }

            var vmReload = DataContext as MainViewModel;
            if (vmReload != null) vmReload.LoadAssignments(currentCourseId);

            AssignmentDialogOverlay.Visibility = Visibility.Collapsed;
            return;
        }
        private void CoursesCreate_Click(object sender, RoutedEventArgs e)
        {
            courseRedactMode = false;
            currentCourse = null;

            CourseDialogTitle.Text = Strings.Ui_Course_CreateTitle;
            CourseNameBox.Text = "";
            CourseTeacherNameBox.Text = "";

            CourseDialogOverlay.Visibility = Visibility.Visible;
        }

        private void CoursesRedact_Click(object sender, RoutedEventArgs e)
        {
            var course = CoursesList.SelectedItem as Course;
            if (course == null)
            {
                MessageBox.Show(Strings.Msg_SelectCourseFirst);
                return;
            }

            courseRedactMode = true;
            currentCourse = course;

            CourseDialogTitle.Text = Strings.Ui_Course_RedactTitle;
            CourseNameBox.Text = course.Name;
            CourseTeacherNameBox.Text = GetTeacherFullName(course.TeacherId);

            CourseDialogOverlay.Visibility = Visibility.Visible;
        }

        private void CourseDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            CourseDialogOverlay.Visibility = Visibility.Collapsed;
        }

        private void CourseDialogOk_Click(object sender, RoutedEventArgs e)
        {
            var name = (CourseNameBox.Text ?? "").Trim();
            if (name.Length == 0)
            {
                MessageBox.Show(Strings.Msg_CourseNameRequired);
                return;
            }

            var teacherName = (CourseTeacherNameBox.Text ?? "").Trim();
            if (teacherName.Length == 0)
            {
                MessageBox.Show(Strings.Msg_TeacherNameRequired);
                return;
            }

            var app = (App)Application.Current;
            var controller = app.Services.GetService<IDatabaseController>();
            if (controller == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            int teacherId = CreateTeacherReturnId(teacherName);
            if (teacherId <= 0)
            {
                MessageBox.Show(Strings.Msg_TeacherNameFormat);
                return;
            }

            string safeName = (name ?? "").Replace("'", "''");

            var vm = DataContext as MainViewModel;
            if (vm == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            if (courseRedactMode && currentCourse != null)
            {
                var setter = app.Services.GetService<IDatabaseSetter>();
                if (setter == null)
                {
                    MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                    return;
                }

                setter.UpdateCourseName(currentCourse.Id, name);
                setter.AssignTeacherToCourse(teacherId, currentCourse.Id);

                vm.Courses.Clear();

                DataTable table = controller.ExecuteReturnCommand($"SELECT * FROM {TableNameSet.COURSES}");
                var director = app.Services.GetService<IBuildDirector>();

                if (director != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        vm.Courses.Add(director.BuildCourse(new CourseBuilder(),
                            (string)row["Name"],
                            (int)row["TeacherId"],
                            (int)row["Id"]));
                    }
                }

                TeacherNameText.Text = GetTeacherFullName(teacherId);

                CourseDialogOverlay.Visibility = Visibility.Collapsed;
                return;
            }
            else
            {
                DataTable cid = controller.ExecuteReturnCommand(
        $@"INSERT INTO {TableNameSet.COURSES} (Name, TeacherId)
        VALUES ('{safeName}', {teacherId});
        SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;");

                int newCourseId = Convert.ToInt32(cid.Rows[0]["NewId"]);

                vm.Courses.Add(new Course(newCourseId, name, teacherId));
                CoursesList.SelectedItem = vm.Courses.Last();

                TeacherNameText.Text = GetTeacherFullName(teacherId);

                CourseDialogOverlay.Visibility = Visibility.Collapsed;
                return;
            }
        }

        private void CoursesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            var course = CoursesList.SelectedItem as Course;

            GradesList.ItemsSource = null;
            GradesList.Items.Clear();
            currentGradeInfo = null;
            GradeDialogOverlay.Visibility = Visibility.Collapsed;

            gradesFilterMode = 0;
            FilterButton.Content = Strings.UI_Filter_All;

            if (course == null)
            {
                TeacherNameText.Text = "";
                StudentInfoText.Text = "";
                AssignmentInfoText.Text = "";
                if (vm != null)
                {
                    vm.CourseStudents.Clear();
                    vm.Assignments.Clear();
                }
                return;
            }

            TeacherNameText.Text = GetTeacherFullName(course.TeacherId);

            //vm.LoadAssignments(course.Id);
            vm.LoadCourseStudents(course.Id);
            vm.LoadAssignments(course.Id);
            StudentInfoText.Text = "";
            AssignmentInfoText.Text = "";

            //
            FilterGradeNonChange(FilterButton, null);
            //
        }

        //
        private void FilterGradeNonChange(object sender, EventArgs e)
        {
            var course = CoursesList.SelectedItem as Course;
            GradesList.ItemsSource = null;
            GradesList.Items.Clear();

            if (course == null)
            {
                return;
            }

            var vm = DataContext as MainViewModel;
            if (vm == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            var app = (App)Application.Current;
            var informator = app.Services.GetService<WpfStudyingSystem.Script.Other.Interfaces.IInformator>();
            if (informator == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            var students = vm.CourseStudents.ToList();

            informator.LoadStudentList(students, course.Id);

            var list = informator.GetStudentGradeInfoList();

            GradesList.ItemsSource = null;
            GradesList.Items.Clear();

            if (list == null || list.Count == 0)
            {
                GradesList.Items.Add(Strings.Msg_NoGrades);
                return;
            }

            //
            GradesList.ItemsSource = list;
        }
        //

        private void CoursesArea_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is System.Windows.Controls.ListBoxItem))
            {
                CoursesList.SelectedItem = null;
                TeacherNameText.Text = "";
            }
        }

        private void AssignmentsArea_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is System.Windows.Controls.ListBoxItem))
            {
                AssignmentsList.SelectedItem = null;
                AssignmentInfoText.Text = "";
            }
        }
        private void StudentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var student = StudentsList.SelectedItem as Human;
            if (student == null)
            {
                StudentInfoText.Text = "";
                return;
            }

            StudentInfoText.Text = student.FullName + Environment.NewLine + Strings.Ui_Age + ": " + student.Age;
        }

        private void StudentsAdd_Click(object sender, RoutedEventArgs e)
        {
            var course = CoursesList.SelectedItem as Course;
            if (course == null)
            {
                MessageBox.Show(Strings.Msg_SelectCourseFirst);
                return;
            }

            currentStudentsCourseId = course.Id;

            StudentDialogTitle.Text = Strings.Ui_Student_CreateTitle;
            StudentFirstNameBox.Text = "";
            StudentLastNameBox.Text = "";
            StudentAgeBox.Text = "20";

            StudentDialogOverlay.Visibility = Visibility.Visible;
        }

        private void StudentDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            StudentDialogOverlay.Visibility = Visibility.Collapsed;
        }

        private void StudentDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (currentStudentsCourseId <= 0)
            {
                MessageBox.Show(Strings.Msg_SelectCourseFirst);
                return;
            }

            string first = (StudentFirstNameBox.Text ?? "").Trim();
            if (first.Length == 0)
            {
                MessageBox.Show(Strings.Msg_FirstNameRequired);
                return;
            }

            string last = (StudentLastNameBox.Text ?? "").Trim();
            if (last.Length == 0)
            {
                MessageBox.Show(Strings.Msg_LastNameRequired);
                return;
            }

            if (first.Any(char.IsDigit) || last.Any(char.IsDigit))
            {
                MessageBox.Show(Strings.Msg_NameLettersOnly);
                return;
            }

            string ageText = (StudentAgeBox.Text ?? "").Trim();
            int age;
            if (!int.TryParse(ageText, out age) || age <= 0)
            {
                MessageBox.Show(Strings.Msg_AgeInvalid);
                return;
            }

            var app = (App)Application.Current;
            var setter = app.Services.GetService<IDatabaseSetter>();
            var director = app.Services.GetService<IBuildDirector>();

            if (setter == null || director == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            Human nStudent = director.BuildHuman(new StudentBuilder(), first, last, age);
            setter.SetStudent(nStudent);
            setter.AssignStudentToCourse(nStudent.Id, currentStudentsCourseId);

            //string safeFirst = first.Replace("'", "''");
            //string safeLast = last.Replace("'", "''");

            //DataTable hid = controller.ExecuteReturnCommand(
            // $@"INSERT INTO {TableNameSet.HUMANS} (FirstName, LastName, Age)
            // VALUES ('{safeFirst}', '{safeLast}', {age});
            // SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;");

            //   int humanId = Convert.ToInt32(hid.Rows[0]["NewId"]);

            //    DataTable sid = controller.ExecuteReturnCommand(
            // $@"INSERT INTO {TableNameSet.STUDENTS} (HumanId)
            //  VALUES ({humanId});
            // SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;");

            //    int studentId = Convert.ToInt32(sid.Rows[0]["NewId"]);

            //    controller.ExecuteCommand(
            // $@"INSERT INTO {TableNameSet.DRAFTS} (StudentId, CourseId)
            //VALUES ({studentId}, {currentStudentsCourseId});");

            StudentDialogOverlay.Visibility = Visibility.Collapsed;

            var vm = DataContext as MainViewModel;
            if (vm != null) vm.LoadCourseStudents(currentStudentsCourseId);
        }

        private void StudentsDelete_Click(object sender, RoutedEventArgs e)
        {
            var course = CoursesList.SelectedItem as Course;
            if (course == null)
            {
                MessageBox.Show(Strings.Msg_SelectCourseFirst);
                return;
            }

            var student = StudentsList.SelectedItem as Human;
            if (student == null)
            {
                MessageBox.Show(Strings.Msg_SelectStudentFirst);
                return;
            }

            var app = (App)Application.Current;
            var setter = app.Services.GetService<IDatabaseSetter>();
            if (setter == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            setter.RemoveStudentFromCourse(student.Id, course.Id);

            var vm = DataContext as MainViewModel;
            if (vm != null) vm.LoadCourseStudents(course.Id);

            StudentInfoText.Text = "";
        }
        private void AssignmentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = AssignmentsList.SelectedItem as Assignment;
            if (a == null)
            {
                AssignmentInfoText.Text = "";
                return;
            }

            string due = a.Date.ToString("dd.MM.yyyy");
            string type = a.Type.ToString();
            string desc = (a.Description ?? "").Trim();

            if (desc.Length == 0)
            {
                desc = "-";
            }

            AssignmentInfoText.Text =
                Strings.Ui_DueDateLabel + " " + due + Environment.NewLine +
                Strings.Ui_TypeLabel + " " + type + Environment.NewLine +
                Strings.Ui_DescriptionLabel + " " + desc;
        }
        private string GetTeacherFullName(int teacherId)
        {
            if (teacherId <= 0) return "";

            var app = (App)Application.Current;
            var controller = app.Services.GetService<IDatabaseController>();
            if (controller == null) return "";

            var dt = controller.ExecuteReturnCommand(
        $@"SELECT h.FirstName, h.LastName
        FROM {TableNameSet.TEACHERS} t
        INNER JOIN {TableNameSet.HUMANS} h ON t.HumanId = h.Id
        WHERE t.Id = {teacherId};");

            if (dt.Rows.Count == 0) return "";

            string first = (dt.Rows[0]["FirstName"]?.ToString() ?? "").Trim();
            string last = (dt.Rows[0]["LastName"]?.ToString() ?? "").Trim();

            return (first + " " + last).Trim();
        }

        private int CreateTeacherReturnId(string fullName)
        {
            string text = (fullName ?? "").Trim();
            var parts = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) return -1;

            string firstName = parts[0];
            string lastName = string.Join(" ", parts.Skip(1));

            if (firstName.Any(char.IsDigit) || lastName.Any(char.IsDigit))
            {
                return -1;
            }

            string safeFirst = firstName.Replace("'", "''");
            string safeLast = lastName.Replace("'", "''");

            var app = (App)Application.Current;
            var controller = app.Services.GetService<IDatabaseController>();
            if (controller == null) return -1;

            var hid = controller.ExecuteReturnCommand(
        $@"INSERT INTO {TableNameSet.HUMANS} (FirstName, LastName, Age)
        VALUES ('{safeFirst}', '{safeLast}', 20);
        SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;");

            int humanId = Convert.ToInt32(hid.Rows[0]["NewId"]);

            var tid = controller.ExecuteReturnCommand(
        $@"INSERT INTO {TableNameSet.TEACHERS} (HumanId)
        VALUES ({humanId});
        SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;");

            int teacherId = Convert.ToInt32(tid.Rows[0]["NewId"]);

            return teacherId;
        }
        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            var course = CoursesList.SelectedItem as Course;
            if (course == null)
            {
                MessageBox.Show(Strings.Msg_SelectCourseFirst);
                return;
            }

            gradesFilterMode++;
            if (gradesFilterMode > 3) gradesFilterMode = 0;

            if (gradesFilterMode == 0) ((Button)sender).Content = Strings.UI_Filter_All;
            else if (gradesFilterMode == 1) ((Button)sender).Content = Strings.UI_Filter_Grade;
            else if (gradesFilterMode == 2) ((Button)sender).Content = Strings.UI_Filter_Credit;
            else ((Button)sender).Content = Strings.UI_Filter_Eap;

            var app = (App)Application.Current;
            var informator = app.Services.GetService<WpfStudyingSystem.Script.Other.Interfaces.IInformator>();
            if (informator == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            var students = new List<Human>();
            foreach (var item in StudentsList.Items)
            {
                var h = item as Human;
                if (h != null) students.Add(h);
            }

            informator.LoadStudentList(students, course.Id);

            var list = informator.GetStudentGradeInfoList();

            if (gradesFilterMode == 1) list = list.Where(x => x.AssignmentType == AssignmentTypesEnum.Grade).ToList();
            else if (gradesFilterMode == 2) list = list.Where(x => x.AssignmentType == AssignmentTypesEnum.Credit).ToList();
            else if (gradesFilterMode == 3) list = list.Where(x => x.AssignmentType == AssignmentTypesEnum.EAP).ToList();

            GradesList.ItemsSource = null;
            GradesList.Items.Clear();

            if (list == null || list.Count == 0)
            {
                GradesList.Items.Add(Strings.Msg_NoGrades);
                return;
            }

            //
            GradesList.ItemsSource = list;
        }
        private void GradesSetPoints_Click(object sender, RoutedEventArgs e)
        {
            var course = CoursesList.SelectedItem as Course;
            if (course == null)
            {
                MessageBox.Show(Strings.Msg_SelectCourseFirst);
                return;
            }

            if (GradesList.SelectedItem == null)
            {
                MessageBox.Show(Strings.Msg_SelectGradeFirst);
                return;
            }

            if (!(GradesList.SelectedItem is Script.Other.Sets.StudentGradeInfo info))
            {
                MessageBox.Show(Strings.Msg_NoGradeData);
                return;
            }
            currentGradeInfo = info;

            if (info.AssignmentType == AssignmentTypesEnum.Credit)
            {
                MessageBox.Show(
                    Strings.Msg_CreditPointsHint,
                    Strings.Msg_InfoTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            GradeDialogTitle.Text = Strings.Ui_Grade_SetTitle;
            GradePointsBox.Text = info.Points.ToString();

            GradeDialogOverlay.Visibility = Visibility.Visible;
        }
        private void GradeDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            GradeDialogOverlay.Visibility = Visibility.Collapsed;
            currentGradeInfo = null;
        }
        private void GradeDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if (currentGradeInfo == null)
            {
                GradeDialogOverlay.Visibility = Visibility.Collapsed;
                return;
            }

            var info = currentGradeInfo.Value;

            string text = (GradePointsBox.Text ?? "").Trim();
            int points;
            if (!int.TryParse(text, out points))
            {
                MessageBox.Show(Strings.Msg_PointsInvalid);
                return;
            }

            if (info.AssignmentType == AssignmentTypesEnum.Credit)
            {
                if (!(points == 0 || points == 1))
                {
                    MessageBox.Show(Strings.Msg_CreditPoints01);
                    return;
                }
            }
            else
            {
                if (points < 0 || points > 100)
                {
                    MessageBox.Show(Strings.Msg_PointsRange0_100);
                    return;
                }
            }

            var app = (App)Application.Current;
            var setter = app.Services.GetService<IDatabaseSetter>();
            if (setter == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            setter.ChangeStudentAssignmentPoints(info.StudentId, info.AssignmentId, points);

            GradeDialogOverlay.Visibility = Visibility.Collapsed;
            currentGradeInfo = null;

            var course = CoursesList.SelectedItem as Course;
            if (course == null) return;

            var informator = app.Services.GetService<WpfStudyingSystem.Script.Other.Interfaces.IInformator>();
            if (informator == null) return;

            var students = new List<Human>();
            foreach (var item in StudentsList.Items)
            {
                var h = item as Human;
                if (h != null) students.Add(h);
            }

            informator.LoadStudentList(students, course.Id);

            var list = informator.GetStudentGradeInfoList();

            if (gradesFilterMode == 1) list = list.Where(x => x.AssignmentType == AssignmentTypesEnum.Grade).ToList();
            else if (gradesFilterMode == 2) list = list.Where(x => x.AssignmentType == AssignmentTypesEnum.Credit).ToList();
            else if (gradesFilterMode == 3) list = list.Where(x => x.AssignmentType == AssignmentTypesEnum.EAP).ToList();

            GradesList.ItemsSource = null;
            GradesList.Items.Clear();

            if (list == null || list.Count == 0)
            {
                GradesList.Items.Add(Strings.Msg_NoGrades);
                return;
            }

            GradesList.ItemsSource = list;
        }
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            var exporter = app.Services.GetService<IExportService>();
            if (exporter == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            var csv = exporter.BuildExport();

            var dialog = new SaveFileDialog();
            dialog.Filter = "CSV files (*.csv)|*.csv";
            dialog.FileName = "study_system_export.csv";

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                File.WriteAllText(dialog.FileName, csv, Encoding.UTF8);
                MessageBox.Show(Strings.Ui_Ok);
            }
        }

        private void StudentsSort_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            var app = (App)Application.Current;
            var sorter = app.Services.GetService<Script.Other.Interfaces.ISpecificListFilter>();
            if (sorter == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            var list = vm.CourseStudents.ToList();

            var mode = StudentsSortBox.SelectedIndex;

            if (mode == 0) list = sorter.SortListByFirstName(list);
            else if (mode == 1) list = sorter.SortListByLastName(list);
            else if (mode == 2) list = sorter.SortListByAge(list);
            else list = sorter.SortListBySimpleNameName(list);

            vm.CourseStudents.Clear();
            foreach (var s in list) vm.CourseStudents.Add(s);
        }
    }
}
