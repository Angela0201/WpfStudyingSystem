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
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Classes.Constructor.Builders;
using WpfStudyingSystem.Script.Classes.Interfaces;
using WpfStudyingSystem.Script.Classes.BaseEntities.Sets;
using WpfStudyingSystem.Resources;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Usables;
using WpfStudyingSystem.Script.ViewModels;

namespace WpfStudyingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentCourseId;
        private bool redactMode;
        private Assignment currentAssignment;
        private bool courseRedactMode;
        private Course currentCourse;
        private readonly Dictionary<int, string> courseTeacherNames = new Dictionary<int, string>();

        public MainWindow()
        {
            InitializeComponent();
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

            var removedId = course.Id;

            vm.Courses.Remove(course);
            courseTeacherNames.Remove(removedId);

            TeacherNameText.Text = "";
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

            AssignmentsList.Items.Remove(assignment);
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

            setter.SetAssignment(assignment, currentCourseId);

            AssignmentDialogOverlay.Visibility = Visibility.Collapsed;
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
            CourseTeacherNameBox.Text = courseTeacherNames.ContainsKey(course.Id) ? courseTeacherNames[course.Id] : "";

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

            var vm = DataContext as MainViewModel;
            if (vm == null)
            {
                MessageBox.Show(Strings.Msg_ServicesNotAvailable);
                return;
            }

            if (courseRedactMode && currentCourse != null)
            {
                var index = vm.Courses.IndexOf(currentCourse);
                if (index >= 0)
                {
                    vm.Courses[index] = new Course(currentCourse.Id, name, currentCourse.TeacherId);

                    courseTeacherNames[currentCourse.Id] = teacherName;
                    TeacherNameText.Text = teacherName;

                    CoursesList.SelectedIndex = index;
                }
            }
            else
            {
                var newId = vm.Courses.Count + 1;
                vm.Courses.Add(new Course(newId, name, -1));

                courseTeacherNames[newId] = teacherName;
                TeacherNameText.Text = teacherName;

                CoursesList.SelectedIndex = vm.Courses.Count - 1;
            }

            CourseDialogOverlay.Visibility = Visibility.Collapsed;
        }

        private void CoursesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var course = CoursesList.SelectedItem as Course;
            if (course == null)
            {
                TeacherNameText.Text = "";
                return;
            }

            TeacherNameText.Text = courseTeacherNames.ContainsKey(course.Id) ? courseTeacherNames[course.Id] : "";
        }

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
    }
}
