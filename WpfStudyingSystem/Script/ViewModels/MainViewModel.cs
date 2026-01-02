using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfStudyingSystem.Resources;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Classes.Constructor.Builders;
using WpfStudyingSystem.Script.Classes.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Usables;
using WpfStudyingSystem.Script.Interfaces;

namespace WpfStudyingSystem.Script.ViewModels
{
    public class MainViewModel
    {
        private readonly IDatabaseGetter getter;
        private readonly IDatabaseSetter setter;
        private readonly IDatabaseComplexGetter complexGetter;
        private readonly IBuildDirector director;
        private readonly IDatabaseController controller;

        public ObservableCollection<Course> Courses { get; }
            = new ObservableCollection<Course>();

        public ObservableCollection<Assignment> Assignments { get; }
            = new ObservableCollection<Assignment>();

        public ObservableCollection<Human> CourseStudents { get; }
            = new ObservableCollection<Human>();

        public MainViewModel()
        {
            var app = (App)Application.Current;
            getter = app.Services.GetService<IDatabaseGetter>();
            setter = app.Services.GetService<IDatabaseSetter>();
            complexGetter = app.Services.GetService<IDatabaseComplexGetter>();
            director = app.Services.GetService<IBuildDirector>();
            controller = app.Services.GetService<IDatabaseController>();

            UpdateCourses();

            if (Courses.Count == 0)
            {
                Courses.Add(new Course(1, "Demo course", -1));
            }
        }

        private void UpdateCourses()
        {
            DataTable table = getter.GetTable(TableNameSet.COURSES);

            foreach (DataRow row in table.Rows)
            {
                Courses.Add(director.BuildCourse(new CourseBuilder(),
                    (string)row["Name"],
                    (int)row["TeacherId"],
                    (int)row["Id"]));
            }
        }
        public void LoadAssignments(int courseId)
        {
            Assignments.Clear();

            var app = (App)Application.Current;
            var controller = app.Services.GetService<IDatabaseController>();
            if (controller == null) return;

            DataTable dt = controller.ExecuteReturnCommand(
        $@"SELECT a.Id, a.Name, a.Date, a.Description, a.Type
        FROM {TableNameSet.ASSIGNMENTS} a
        INNER JOIN {TableNameSet.ASSIGNMENTS_DEPENDENCIES} d ON d.AssignmentId = a.Id
        WHERE d.CourseId = {courseId};");

            foreach (DataRow row in dt.Rows)
            {
                int typeInt = (int)row["Type"];

                IAssignmentBuilder builder;
                if (typeInt == 0) builder = new GradeAssignmentBuilder();
                else if (typeInt == 1) builder = new CreditAssignmentBuilder();
                else builder = new EAPAssignmentBuilder();

                var assignment = director.BuildAssignment(
                    builder,
                    (string)row["Name"],
                    (string)row["Description"],
                    (DateTime)row["Date"],
                    (int)row["Id"]);

                Assignments.Add(assignment);
            }
        }
        public void LoadCourseStudents(int courseId)
        {
            CourseStudents.Clear();

            var app = (App)Application.Current;
            var controller = app.Services.GetService<IDatabaseController>();
            var director = app.Services.GetService<IBuildDirector>();

            if (controller == null || director == null) return;

            DataTable dt = controller.ExecuteReturnCommand(
        $@"SELECT s.Id AS StudentId, h.FirstName, h.LastName, h.Age
        FROM {TableNameSet.DRAFTS} d
        INNER JOIN {TableNameSet.STUDENTS} s ON d.StudentId = s.Id
        INNER JOIN {TableNameSet.HUMANS} h ON s.HumanId = h.Id
        WHERE d.CourseId = {courseId};");

            foreach (DataRow row in dt.Rows)
            {
                var human = director.BuildHuman(
                    new StudentBuilder(),
                    (string)row["FirstName"],
                    (string)row["LastName"],
                    (int)row["Age"],
                    (int)row["StudentId"]);

                CourseStudents.Add(human);
            }
        }
    }
}
