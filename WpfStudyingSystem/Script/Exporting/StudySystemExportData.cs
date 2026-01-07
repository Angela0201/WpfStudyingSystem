using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfStudyingSystem.Script.Exporting
{
    public class StudySystemExportData
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<CourseRow> Courses { get; set; } = new List<CourseRow>();
        public List<TeacherRow> Teachers { get; set; } = new List<TeacherRow>();
        public List<StudentRow> Students { get; set; } = new List<StudentRow>();
        public List<DraftRow> Drafts { get; set; } = new List<DraftRow>();
        public List<AssignmentRow> Assignments { get; set; } = new List<AssignmentRow>();
        public List<AssignmentDependencyRow> AssignmentDependencies { get; set; } = new List<AssignmentDependencyRow>();
        public List<AssignmentStatisticsRow> AssignmentStatistics { get; set; } = new List<AssignmentStatisticsRow>();
    }

    public class CourseRow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeacherId { get; set; }
    }

    public class TeacherRow
    {
        public int Id { get; set; }
        public int HumanId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    public class StudentRow
    {
        public int Id { get; set; }
        public int HumanId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    public class DraftRow
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }

    public class AssignmentRow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
    }

    public class AssignmentDependencyRow
    {
        public int CourseId { get; set; }
        public int AssignmentId { get; set; }
    }

    public class AssignmentStatisticsRow
    {
        public int StudentId { get; set; }
        public int AssignmentId { get; set; }
        public int Points { get; set; }
    }
}
