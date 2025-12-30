using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Usables;
using WpfStudyingSystem.Script.Classes.Interfaces;
using WpfStudyingSystem.Script.Classes.Constructor.Builders;
using WpfStudyingSystem.Script.Classes.BaseEntities.Sets;

namespace WpfStudyingSystem.Script.ViewModels
{
    public class MainViewModel
    {
        private readonly IDatabaseGetter getter;
        private readonly IDatabaseSetter setter;
        private readonly IDatabaseComplexGetter complexGetter;
        private readonly IBuildDirector director;

        private ObservableCollection<Course> courses = new ObservableCollection<Course>();
        public ObservableCollection<Course> Courses { get { return courses; } }


        private ObservableCollection<Human> students = new ObservableCollection<Human>();
        public ObservableCollection<Human> Students { get { return students; } }


        private ObservableCollection<Assignment> assignments = new ObservableCollection<Assignment>();
        public ObservableCollection<Assignment> Assignments { get { return assignments; } }

        public MainViewModel()
        {
            var app = (App)Application.Current;
            getter = app.Services.GetService<IDatabaseGetter>();
            complexGetter = app.Services.GetService<IDatabaseComplexGetter>();
            director = app.Services.GetService<IBuildDirector>();


            UpdateCourses();
        }

        public void UpdateCourses()
        {
            DataTable table = getter.GetTable(TableNameSet.COURSES);
            ObservableCollection<Course> nCourses = new ObservableCollection<Course>();

            foreach (DataRow row in table.Rows)
            {
                nCourses.Add(director.BuildCourse(new CourseBuilder(),
                    (string)row["Name"],
                    (int)row["TeacherId"],
                    (int)row["Id"]));
            }

            courses = nCourses;
        }

        public void UpdateStudents(int courseId)
        {
            DataTable table = getter.GetStudentTableViaCourseId(courseId);
            ObservableCollection<Human> nStudents = new ObservableCollection<Human>();

            foreach (DataRow row in table.Rows)
            {
                nStudents.Add(director.BuildHuman(new StudentBuilder(),
                    (string)row["FirstName"],
                    (string)row["LastName"],
                    (int)row["Age"],
                    (int)row["Id"]));
            }

            students = nStudents;
        }

        public void UpdateAssignments(int courseId)
        {
            DataTable table = getter.GetAssignmentTableViaCourseId(courseId);
            ObservableCollection<Assignment> nAssignments = new ObservableCollection<Assignment>();
            IAssignmentBuilder builder;

            foreach (DataRow row in table.Rows)
            {
                switch ((int)row["Type"])
                {
                    case 0:
                        builder = new GradeAssignmentBuilder();
                        break;
                    case 1:
                        builder = new CreditAssignmentBuilder();
                        break;
                    case 2:
                        builder = new EAPAssignmentBuilder();
                        break;
                    default:
                        builder = new GradeAssignmentBuilder();
                        break;
                }

                nAssignments.Add(director.BuildAssignment(builder,
                    (string)row["Name"],
                    (string)row["Description"],
                    (DateTime)row["Date"],
                    (int)row["Id"]));
            }

            assignments = nAssignments;
        }

        public Human GetTeacherById( int teacherId)
        {
             return complexGetter.GetHuman(teacherId,TableNameSet.TEACHERS);
        }
    }
}
