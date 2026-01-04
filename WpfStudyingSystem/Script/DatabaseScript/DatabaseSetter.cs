using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.Interfaces;

namespace WpfStudyingSystem.Script.DatabaseScript.Usables
{

    //Big and not optimized, may be works

    public class DatabaseSetter : IDatabaseSetter
    {
        private IDatabaseConnectionString databaseConnectionString = new DatabaseConnectionString();
        private string ConnStr => databaseConnectionString.ConnectionString;


        private int GetIdByHumanId(int humanId, string from)
        {
            var app = (App)Application.Current;
            IDatabaseController controller = app.Services.GetService<IDatabaseController>();
            DataTable table = controller.ExecuteReturnCommand($@"SELECT * FROM {from} where HumanId = {humanId}");
            if (table.Rows.Count > 0) { return (int)table.Rows[0]["Id"]; }
            return -1;
        }


        public void RemoveAssignment(int assignmentId)
        {///removes assignment, and removes it from dependencies and statistics
            string command =
$@"DELETE FROM {TableNameSet.ASSIGNMENTS} WHERE Id = {assignmentId}";
            AssignData(command);

            command =
$@"DELETE FROM {TableNameSet.ASSIGNMENTS_DEPENDENCIES} WHERE AssignmentId = {assignmentId}";
            AssignData(command);

            command =
$@"DELETE FROM {TableNameSet.ASSIGNMENTS_STATISTICS} WHERE AssignmentId = {assignmentId}";
            AssignData(command);
        }

        public void RemoveCourse(int courceId)
        {///removes course, and removes it from drafts and dependencies
            string command =
$@"DELETE FROM {TableNameSet.COURSES} WHERE Id = {courceId}";
            AssignData(command);

            command =
$@"DELETE FROM {TableNameSet.DRAFTS} WHERE CourseId = {courceId}";
            AssignData(command);

            command =
$@"DELETE FROM {TableNameSet.ASSIGNMENTS_DEPENDENCIES} WHERE CourseId = {courceId}";
            AssignData(command);
        }

        public void RemoveHunman(int humanId)
        {///remove human and everything related to him
            string command =
$@"DELETE FROM {TableNameSet.HUMANS} WHERE Id = {humanId}";
            AssignData(command);

            int id = GetIdByHumanId(humanId, TableNameSet.STUDENTS);
            if (id >= 0) { RemoveStudent(id); }

            id = GetIdByHumanId(humanId, TableNameSet.TEACHERS);
            if (id >= 0) { RemoveTeacher(id); }
        }

        public void RemoveStudent(int studentId)
        {///remove human and everything related to him, but dont remove related human
            string command =
$@"DELETE FROM {TableNameSet.STUDENTS} WHERE Id = {studentId}";
            AssignData(command);

            command =
$@"DELETE FROM {TableNameSet.DRAFTS} WHERE StudentId = {studentId}";
            AssignData(command);

            command =
$@"DELETE FROM {TableNameSet.ASSIGNMENTS_STATISTICS} WHERE StudentId = {studentId}";
            AssignData(command);
        }

        public void RemoveTeacher(int teacherId)
        {///remove teacher and everything related to him, but dont remove related human
            string command =
$@"DELETE FROM {TableNameSet.TEACHERS} WHERE Id = {teacherId}";
            AssignData(command);

            command =
$@"UPDATE {TableNameSet.COURSES}
 SET TeacherId = -1
 WHERE TeacherId = {teacherId};";
            AssignData(command);
        }




        public void SetAssignment(Assignment assignment, int courseId)
        {///Crates assignment and sets it in assignment dependencies table
            //MessageBox.Show("AAAAAAAAAAAAAAAAAAA");
            string command =
$@"INSERT INTO {TableNameSet.ASSIGNMENTS} (Name, Date, Description, Type)
 VALUES ('{assignment.Name}', {assignment.DateString}, '{assignment.Description}', {(int)assignment.Type});";
            AssignData(command);

            //
            //var app = (App)Application.Current;
            //IDatabaseController ctl = app.Services.GetService<IDatabaseController>();
            //DataTable dt = ctl.ExecuteReturnCommand($@"SELECT * FROM {TableNameSet.ASSIGNMENTS};");
            //int asId = 
            //
            MessageBox.Show(command);

            command =
$@"INSERT INTO {TableNameSet.ASSIGNMENTS_DEPENDENCIES} (CourseId, AssignmentId)
 VALUES ({courseId}, {assignment.Id});";
            AssignData(command);
            MessageBox.Show(command);

            var app = (App)Application.Current;
            IDatabaseController ctl = app.Services.GetService<IDatabaseController>();
            DataTable dt = ctl.ExecuteReturnCommand($@"SELECT * FROM {TableNameSet.DRAFTS} WHERE CourseId = {courseId};");
            if (dt.Rows.Count < 1) { return; }

            command =
$@"INSERT INTO {TableNameSet.ASSIGNMENTS_STATISTICS} (StudentId, AssignmentId, Points)
 VALUES ";
            if (dt.Rows.Count < 1) { return; }
            foreach (DataRow row in dt.Rows)
            {
                command += $" ({(int)row["StudentId"]}, {assignment.Id}, 0),";
            }
            MessageBox.Show(command);
            command = command.Remove(command.Length - 1) + ";";
            AssignData(command);

        }

        public void SetCourse(Course course)
        {///Creates course
            string command =
$@"INSERT INTO {TableNameSet.COURSES} (Name, TeacherId)
 VALUES ('{course.Name}', {course.TeacherId});";
            AssignData(command);
        }

        private void SetToAssignmentsStatistics(int studentId, int courseId)
        {
            string command =
$@"INSERT INTO {TableNameSet.ASSIGNMENTS_STATISTICS} (StudentId, AssignmentId, Points)
 VALUES ";
            var app = (App)Application.Current;
            IDatabaseController ctl = app.Services.GetService<IDatabaseController>();
            DataTable dt = ctl.ExecuteReturnCommand(
$@"SELECT *
 FROM {TableNameSet.ASSIGNMENTS_DEPENDENCIES}
 WHERE CourseId = {courseId}");

            if (dt.Rows.Count < 1) { return; }

            foreach (DataRow row in dt.Rows)
            {
                command += $" ({studentId}, {(int)row["AssignmentId"]}, {0}),";
            }
            command = command.Remove(command.Length-1) + ";";
            AssignData(command);
        }

        public void SetHunman(Human human)
        {///Sets human in human table
            string command = 
$@"INSERT INTO {TableNameSet.HUMANS} (FirstName, LastName, Age)
 VALUES ('{human.FirstName}', '{human.LastName}', {human.Age});";
            AssignData(command);
        }

        public void SetStudent(Human student, int humanId = -1)
        {
            ///Sets Student in students table
            ///If you want to append existing human to students, then specify human id
            ///else, it will generate human automatically
            int hId;
            if (humanId < 0) { hId = SetHumanReturnId(student); }
            else { hId = humanId; }
            string command =
$@"INSERT INTO {TableNameSet.STUDENTS} (HumanId)
 VALUES ({hId});";
            AssignData(command);
        }

        public void SetTeacher(Human teacher, int humanId = -1)
        {
            ///Sets Studen in teachers table
            ///If you want to append existing human to teachers, then specify human id
            ///else, it will generate human automatically
            int hId;
            if (humanId < 0) { hId = SetHumanReturnId(teacher); }
            else { hId = humanId; }
            string command =
$@"INSERT INTO {TableNameSet.TEACHERS} (HumanId)
 VALUES ({hId});";
            AssignData(command);
        }



        private void AssignData(string command)
        {
            var conn = new SqlConnection(ConnStr);
            conn.Open();

            var cmd = new SqlCommand(command, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        private int SetHumanReturnId(Human human)
        {
            var app = (App)Application.Current;
            IDatabaseGetter getter = app.Services.GetService<IDatabaseGetter>();
            int id = getter.GetUniqueId(TableNameSet.HUMANS);

            string command =
$@"INSERT INTO {TableNameSet.HUMANS} ( FirstName, LastName, Age)
 VALUES ('{human.FirstName}', '{human.LastName}', {human.Age});";
            AssignData(command);
            return id;
        }



        public void ChangeStudentAssignmentPoints(int studentId, int assignmentId, int points)
        {///changes the student grade
            string command =
$@"UPDATE {TableNameSet.ASSIGNMENTS_STATISTICS}
 SET Points = {points}
 WHERE StudentId = {studentId} AND AssignmentId = {assignmentId};";
            AssignData(command);
        }



        public void AssignStudentToCourse(int studentId, int courseId)
        {
            ///Adds student to a course in draft datatable and connects it to the assignments
            string command =
$@"INSERT INTO {TableNameSet.DRAFTS} (StudentId, CourseId)
 VALUES ({studentId}, {courseId});";
            AssignData(command);

            SetToAssignmentsStatistics(studentId, courseId);
        }

        public void AssignTeacherToCourse(int teacherId, int courseId)
        {///sets new teacher id to course
            string command =
$@"UPDATE {TableNameSet.COURSES}
 SET TeacherId = {teacherId}
 WHERE Id = {courseId};";
            AssignData(command);
        }

        public void UpdateCourseName(int courseId, string newName)
        {
            string safeName = (newName ?? "").Replace("'", "''");

            string command =
$@"UPDATE {TableNameSet.COURSES}
 SET Name = '{safeName}'
 WHERE Id = {courseId};";

            AssignData(command);
        }

        public void RemoveStudentFromCourse(int studentId, int courseId)
        {///Removes student from draft and unassign assignments in assgnments statistics
            string command =
$@"DELETE FROM {TableNameSet.DRAFTS} WHERE StudentId = {studentId} AND CourseId = {courseId}";
            AssignData(command);

            command =
$@"DELETE FROM {TableNameSet.ASSIGNMENTS_STATISTICS} WHERE StudentId = {studentId} AND (";

            var app = (App)Application.Current;
            IDatabaseController ctl = app.Services.GetService<IDatabaseController>();
            DataTable dt = ctl.ExecuteReturnCommand(
$@"SELECT *
 FROM {TableNameSet.ASSIGNMENTS_DEPENDENCIES}
 WHERE CourseId = {courseId}");
            if (dt.Rows.Count < 1) { return; }

            foreach (DataRow row in dt.Rows)
            {
                command += $" AssignmentId = {(int)row["AssignmentId"]} OR";
            }
            command = command.Remove(command.Length - 2) + ")";
            //MessageBox.Show(command);
            AssignData(command);
        }

        public void RemoveTeacherFromCourse(int teacherId, int courseId)
        {///replace teacher id in course with -1 in databse
            string command =
$@"UPDATE {TableNameSet.COURSES}
 SET TeacherId = -1
 WHERE Id = {courseId} AND TeacherId = {teacherId};";
            AssignData(command);
        }
        public void UpdateAssignment(Assignment assignment)
        {
            string safeName = (assignment.Name ?? "").Replace("'", "''");
            string safeDesc = (assignment.Description ?? "").Replace("'", "''");
            string safeDate = assignment.Date.ToString("yyyy-MM-dd HH:mm:ss");
            int typeInt = (int)assignment.Type;

            string command =
        $@"UPDATE {TableNameSet.ASSIGNMENTS}
SET Name = '{safeName}', Date = '{safeDate}', Description = '{safeDesc}', Type = {typeInt}
WHERE Id = {assignment.Id};";

            AssignData(command);
        }
    }
}
