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
        private readonly MainViewModel DataContext = new MainViewModel();
        private int curCourseId = 0;

        public MainWindow()
        {
            InitializeComponent();

            UpdateCourseList();

            CoursesList.SelectionChanged += OnCourseListItemSelect;
        }


        public void UpdateCourseList()
        {
            DataContext.UpdateCourses();
            CoursesList.ItemsSource = DataContext.Courses;
            CoursesList.Items.Refresh();
        }

        public void UpdateStudentList(int courseId)
        {
            DataContext.UpdateStudents(courseId);
            StudentsList.ItemsSource = DataContext.Students;
            StudentsList.Items.Refresh();
        }

        public void UpdateAssignmentList(int courseId)
        {
            DataContext.UpdateAssignments(courseId);
            AssignmentsList.ItemsSource = DataContext.Assignments;
            AssignmentsList.Items.Refresh();
        }

        public void ShowTeacherName()
        {
            Course selectedCourse = (Course)CoursesList.SelectedItem;
            if (selectedCourse.TeacherId < 1) { TeacherNameText.Text = ""; return; }
            TeacherNameText.Text = DataContext.GetTeacherById(selectedCourse.TeacherId).FullName;
        }

        private void OnCourseListItemSelect(object sender, SelectionChangedEventArgs e)
        {
            Course selectedCourse = (Course)CoursesList.SelectedItem;
            curCourseId = selectedCourse.Id;

            UpdateStudentList(curCourseId);
            UpdateAssignmentList(curCourseId);
            ShowTeacherName();
        }
    }
}
