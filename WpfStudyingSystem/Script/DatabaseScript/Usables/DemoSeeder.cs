using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.Interfaces;

namespace WpfStudyingSystem.Script.DatabaseScript.Usables
{
    public class DemoSeeder : IDemoSeeder
    {
        public void SeedIfEmpty(IDatabaseController controller)
        {
            var existing = controller.ExecuteReturnCommand($"SELECT TOP 1 Id FROM {TableNameSet.COURSES};");
            if (existing.Rows.Count > 0) return;

            int teacher1 = CreateTeacher(controller, "John", "Smith", 35);
            int teacher2 = CreateTeacher(controller, "Mike", "Tyson", 41);

            int course1 = CreateCourse(controller, "History", teacher1);
            int course2 = CreateCourse(controller, "Calculus 2", teacher2);

            int student1 = CreateStudent(controller, "Maria", "Ivanova", 20);
            int student2 = CreateStudent(controller, "Ivan", "Petrov", 21);
            int student3 = CreateStudent(controller, "Petr", "Smirnov", 19);

            AssignStudentToCourse(controller, student1, course1);
            AssignStudentToCourse(controller, student2, course1);
            AssignStudentToCourse(controller, student3, course2);

            int a1 = CreateAssignment(controller, "Final test", DateTime.Now.AddDays(7), "Grade example", 0);
            int a2 = CreateAssignment(controller, "Group project", DateTime.Now.AddDays(10), "Credit example", 1);
            int a3 = CreateAssignment(controller, "Essay", DateTime.Now.AddDays(14), "EAP example", 2);

            AddAssignmentToCourse(controller, course1, a1);
            AddAssignmentToCourse(controller, course1, a2);
            AddAssignmentToCourse(controller, course2, a3);

            EnsureStatistics(controller, course1, a1);
            EnsureStatistics(controller, course1, a2);
            EnsureStatistics(controller, course2, a3);

            EnsureStatistics(controller, course1, a1);
            EnsureStatistics(controller, course1, a2);
            EnsureStatistics(controller, course2, a3);

            SetDemoPoints(controller, student1, a1, 78);
            SetDemoPoints(controller, student2, a1, 95);

            SetDemoPoints(controller, student1, a2, 1);
            SetDemoPoints(controller, student2, a2, 0);

            SetDemoPoints(controller, student3, a3, 88);
        }

        private int CreateHuman(IDatabaseController controller, string first, string last, int age)
        {
            first = (first ?? "").Replace("'", "''");
            last = (last ?? "").Replace("'", "''");

            DataTable dt = controller.ExecuteReturnCommand(
$@"INSERT INTO {TableNameSet.HUMANS} (FirstName, LastName, Age)
VALUES ('{first}', '{last}', {age});
SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;");

            return Convert.ToInt32(dt.Rows[0]["NewId"]);
        }

        private int CreateTeacher(IDatabaseController controller, string first, string last, int age)
        {
            int humanId = CreateHuman(controller, first, last, age);

            DataTable dt = controller.ExecuteReturnCommand(
$@"INSERT INTO {TableNameSet.TEACHERS} (HumanId)
VALUES ({humanId});
SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;");

            return Convert.ToInt32(dt.Rows[0]["NewId"]);
        }

        private int CreateStudent(IDatabaseController controller, string first, string last, int age)
        {
            int humanId = CreateHuman(controller, first, last, age);

            DataTable dt = controller.ExecuteReturnCommand(
$@"INSERT INTO {TableNameSet.STUDENTS} (HumanId)
VALUES ({humanId});
SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;");

            return Convert.ToInt32(dt.Rows[0]["NewId"]);
        }

        private int CreateCourse(IDatabaseController controller, string name, int teacherId)
        {
            name = (name ?? "").Replace("'", "''");

            DataTable dt = controller.ExecuteReturnCommand(
$@"INSERT INTO {TableNameSet.COURSES} (Name, TeacherId)
VALUES ('{name}', {teacherId});
SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;");

            return Convert.ToInt32(dt.Rows[0]["NewId"]);
        }

        private int CreateAssignment(IDatabaseController controller, string name, DateTime date, string desc, int type)
        {
            name = (name ?? "").Replace("'", "''");
            desc = (desc ?? "").Replace("'", "''");
            string safeDate = date.ToString("yyyy-MM-dd HH:mm:ss");

            DataTable dt = controller.ExecuteReturnCommand(
$@"INSERT INTO {TableNameSet.ASSIGNMENTS} (Name, Date, Description, Type)
VALUES ('{name}', '{safeDate}', '{desc}', {type});
SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;");

            return Convert.ToInt32(dt.Rows[0]["NewId"]);
        }

        private void AssignStudentToCourse(IDatabaseController controller, int studentId, int courseId)
        {
            controller.ExecuteCommand(
$@"INSERT INTO {TableNameSet.DRAFTS} (StudentId, CourseId)
VALUES ({studentId}, {courseId});");
        }

        private void AddAssignmentToCourse(IDatabaseController controller, int courseId, int assignmentId)
        {
            controller.ExecuteCommand(
$@"INSERT INTO {TableNameSet.ASSIGNMENTS_DEPENDENCIES} (CourseId, AssignmentId)
VALUES ({courseId}, {assignmentId});");
        }

        private void EnsureStatistics(IDatabaseController controller, int courseId, int assignmentId)
        {
            var students = controller.ExecuteReturnCommand(
$@"SELECT StudentId FROM {TableNameSet.DRAFTS} WHERE CourseId = {courseId};");

            foreach (DataRow row in students.Rows)
            {
                int studentId = Convert.ToInt32(row["StudentId"]);
                controller.ExecuteCommand(
                    $@"IF NOT EXISTS (
    SELECT 1 FROM {TableNameSet.ASSIGNMENTS_STATISTICS}
    WHERE StudentId = {studentId} AND AssignmentId = {assignmentId}
)
INSERT INTO {TableNameSet.ASSIGNMENTS_STATISTICS} (StudentId, AssignmentId, Points)
VALUES ({studentId}, {assignmentId}, 0);");
            }
        }
        private void SetDemoPoints(IDatabaseController controller, int studentId, int assignmentId, int points)
        {
            controller.ExecuteCommand(
        $@"UPDATE {TableNameSet.ASSIGNMENTS_STATISTICS}
SET Points = {points}
WHERE StudentId = {studentId} AND AssignmentId = {assignmentId};");
        }
    }
}
