using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.DatabaseScript.Usables;
using WpfStudyingSystem.Script.Interfaces;

namespace WpfStudyingSystem.Script.Exporting
{
    public class ExportService : IExportService
    {
        private readonly IDatabaseController controller;

        public ExportService(IDatabaseController controller)
        {
            this.controller = controller;
        }

        public string BuildExport()
        {
            var data = BuildExportData();
            return BuildCsv(data);
        }

        private StudySystemExportData BuildExportData()
        {
            var data = new StudySystemExportData();

            var courses = controller.ExecuteReturnCommand($"SELECT Id, Name, TeacherId FROM {TableNameSet.COURSES};");
            foreach (DataRow row in courses.Rows)
            {
                data.Courses.Add(new CourseRow
                {
                    Id = row["Id"] == DBNull.Value ? -1 : Convert.ToInt32(row["Id"]),
                    Name = row["Name"] == DBNull.Value ? "" : row["Name"].ToString(),
                    TeacherId = row["TeacherId"] == DBNull.Value ? -1 : Convert.ToInt32(row["TeacherId"])
                });
            }

            var teachers = controller.ExecuteReturnCommand(
                $"SELECT t.Id AS Id, t.HumanId AS HumanId, h.FirstName AS FirstName, h.LastName AS LastName, h.Age AS Age " +
                $"FROM {TableNameSet.TEACHERS} t " +
                $"LEFT JOIN {TableNameSet.HUMANS} h ON t.HumanId = h.Id;"
            );
            foreach (DataRow row in teachers.Rows)
            {
                data.Teachers.Add(new TeacherRow
                {
                    Id = row["Id"] == DBNull.Value ? -1 : Convert.ToInt32(row["Id"]),
                    HumanId = row["HumanId"] == DBNull.Value ? -1 : Convert.ToInt32(row["HumanId"]),
                    FirstName = row["FirstName"] == DBNull.Value ? "" : row["FirstName"].ToString(),
                    LastName = row["LastName"] == DBNull.Value ? "" : row["LastName"].ToString(),
                    Age = row["Age"] == DBNull.Value ? 0 : Convert.ToInt32(row["Age"])
                });
            }

            var students = controller.ExecuteReturnCommand(
                $"SELECT s.Id AS Id, s.HumanId AS HumanId, h.FirstName AS FirstName, h.LastName AS LastName, h.Age AS Age " +
                $"FROM {TableNameSet.STUDENTS} s " +
                $"LEFT JOIN {TableNameSet.HUMANS} h ON s.HumanId = h.Id;"
            );
            foreach (DataRow row in students.Rows)
            {
                data.Students.Add(new StudentRow
                {
                    Id = row["Id"] == DBNull.Value ? -1 : Convert.ToInt32(row["Id"]),
                    HumanId = row["HumanId"] == DBNull.Value ? -1 : Convert.ToInt32(row["HumanId"]),
                    FirstName = row["FirstName"] == DBNull.Value ? "" : row["FirstName"].ToString(),
                    LastName = row["LastName"] == DBNull.Value ? "" : row["LastName"].ToString(),
                    Age = row["Age"] == DBNull.Value ? 0 : Convert.ToInt32(row["Age"])
                });
            }

            var drafts = controller.ExecuteReturnCommand($"SELECT StudentId, CourseId FROM {TableNameSet.DRAFTS};");
            foreach (DataRow row in drafts.Rows)
            {
                data.Drafts.Add(new DraftRow
                {
                    StudentId = row["StudentId"] == DBNull.Value ? -1 : Convert.ToInt32(row["StudentId"]),
                    CourseId = row["CourseId"] == DBNull.Value ? -1 : Convert.ToInt32(row["CourseId"])
                });
            }

            var assignments = controller.ExecuteReturnCommand($"SELECT Id, Name, Date, Description, Type FROM {TableNameSet.ASSIGNMENTS};");
            foreach (DataRow row in assignments.Rows)
            {
                data.Assignments.Add(new AssignmentRow
                {
                    Id = row["Id"] == DBNull.Value ? -1 : Convert.ToInt32(row["Id"]),
                    Name = row["Name"] == DBNull.Value ? "" : row["Name"].ToString(),
                    Date = row["Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["Date"]),
                    Description = row["Description"] == DBNull.Value ? "" : row["Description"].ToString(),
                    Type = row["Type"] == DBNull.Value ? 0 : Convert.ToInt32(row["Type"])
                });
            }

            var deps = controller.ExecuteReturnCommand($"SELECT CourseId, AssignmentId FROM {TableNameSet.ASSIGNMENTS_DEPENDENCIES};");
            foreach (DataRow row in deps.Rows)
            {
                data.AssignmentDependencies.Add(new AssignmentDependencyRow
                {
                    CourseId = row["CourseId"] == DBNull.Value ? -1 : Convert.ToInt32(row["CourseId"]),
                    AssignmentId = row["AssignmentId"] == DBNull.Value ? -1 : Convert.ToInt32(row["AssignmentId"])
                });
            }

            var stats = controller.ExecuteReturnCommand($"SELECT StudentId, AssignmentId, Points FROM {TableNameSet.ASSIGNMENTS_STATISTICS};");
            foreach (DataRow row in stats.Rows)
            {
                data.AssignmentStatistics.Add(new AssignmentStatisticsRow
                {
                    StudentId = row["StudentId"] == DBNull.Value ? -1 : Convert.ToInt32(row["StudentId"]),
                    AssignmentId = row["AssignmentId"] == DBNull.Value ? -1 : Convert.ToInt32(row["AssignmentId"]),
                    Points = row["Points"] == DBNull.Value ? 0 : Convert.ToInt32(row["Points"])
                });
            }

            return data;
        }

        private string BuildCsv(StudySystemExportData data)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"CreatedAt;{Escape(data.CreatedAt.ToString("dd.MM.yyyy"))}");
            sb.AppendLine();

            sb.AppendLine("[Courses]");
            sb.AppendLine("Id;Name;TeacherId");
            foreach (var c in data.Courses) sb.AppendLine($"{c.Id};{Escape(c.Name)};{c.TeacherId}");
            sb.AppendLine();

            sb.AppendLine("[Teachers]");
            sb.AppendLine("Id;HumanId;FirstName;LastName;Age");
            foreach (var t in data.Teachers) sb.AppendLine($"{t.Id};{t.HumanId};{Escape(t.FirstName)};{Escape(t.LastName)};{t.Age}");
            sb.AppendLine();

            sb.AppendLine("[Students]");
            sb.AppendLine("Id;HumanId;FirstName;LastName;Age");
            foreach (var s in data.Students) sb.AppendLine($"{s.Id};{s.HumanId};{Escape(s.FirstName)};{Escape(s.LastName)};{s.Age}");
            sb.AppendLine();

            sb.AppendLine("[Drafts]");
            sb.AppendLine("StudentId;CourseId");
            foreach (var d in data.Drafts) sb.AppendLine($"{d.StudentId};{d.CourseId}");
            sb.AppendLine();

            sb.AppendLine("[Assignments]");
            sb.AppendLine("Id;Name;Date;Description;Type");
            foreach (var a in data.Assignments) sb.AppendLine($"{a.Id};{Escape(a.Name)};{Escape(a.Date.ToString("dd.MM.yyyy"))};{Escape(a.Description)};{a.Type}");
            sb.AppendLine();

            sb.AppendLine("[AssignmentsDependencies]");
            sb.AppendLine("CourseId;AssignmentId");
            foreach (var d in data.AssignmentDependencies) sb.AppendLine($"{d.CourseId};{d.AssignmentId}");
            sb.AppendLine();

            sb.AppendLine("[AssignmentsStatistics]");
            sb.AppendLine("StudentId;AssignmentId;Points");
            foreach (var st in data.AssignmentStatistics) sb.AppendLine($"{st.StudentId};{st.AssignmentId};{st.Points}");

            return sb.ToString();
        }

        private string Escape(string value)
        {
            if (value == null) return "";
            return value.Replace("\r", " ").Replace("\n", " ").Replace(";", ",");
        }
    }
}
