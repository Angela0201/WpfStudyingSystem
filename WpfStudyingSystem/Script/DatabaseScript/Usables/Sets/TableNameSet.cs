using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfStudyingSystem.Script.DatabaseScript.Usables
{
    public static class TableNameSet
    {
        //To avoid hardstrings or misspeling, use this values, as if it was enumeration

        public const string HUMANS = "Humans";

        public const string STUDENTS = "Students";
        public const string ASSIGNMENTS = "Assignments";
        public const string TEACHERS = "Teachers";

        public const string ASSIGNMENTS_STATISTICS = "AssignmentsStatistics";
        public const string COURSES = "Courses";
        public const string DRAFTS = "Drafts";

        public const string ASSIGNMENTS_DEPENDENCIES = "AssignmentsDependencies";

    }
}
