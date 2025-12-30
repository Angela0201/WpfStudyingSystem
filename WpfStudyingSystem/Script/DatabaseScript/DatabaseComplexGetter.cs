using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Classes.Constructor.Builders;
using WpfStudyingSystem.Script.Classes.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Usables;

namespace WpfStudyingSystem.Script.DatabaseScript
{
    public class DatabaseComplexGetter : IDatabaseComplexGetter
    {

        private IDatabaseConnectionString databaseConnectionString = new DatabaseConnectionString();
        private string ConnStr => databaseConnectionString.ConnectionString;

        private DataTable GetTable(string tableName)
        {
            var app = (App)Application.Current;
            IDatabaseGetter getter = app.Services.GetService<IDatabaseGetter>();

            var table = getter.GetTable(tableName);

            return table;
        }

        private DataRow GetRow(string tableName, int id)
        {
            var table = GetTable(tableName);

            if (table.Rows.Count > 0) { return table.Rows[0]; }
            return null;
        }

        public Assignment GetAssignment(int id)
        {
            var app = (App)Application.Current;
            IBuildDirector director = app.Services.GetService<IBuildDirector>();


            DataRow row = GetRow(TableNameSet.ASSIGNMENTS, id);

            switch ((int)row["Type"])
            {
                case 0:
                    return director.BuildAssignment(new GradeAssignmentBuilder(), (string)row["Name"], (string)row["Description"], (DateTime)row["Date"], (int)row["Id"]);

                case 1:
                    return director.BuildAssignment(new CreditAssignmentBuilder(), (string)row["Name"], (string)row["Description"], (DateTime)row["Date"], (int)row["Id"]);

                case 2:
                    return director.BuildAssignment(new EAPAssignmentBuilder(), (string)row["Name"], (string)row["Description"], (DateTime)row["Date"], (int)row["Id"]);

                default: return null;
            }
        }

        public Course GetCourse(int id)
        {
            var app = (App)Application.Current;
            IBuildDirector director = app.Services.GetService<IBuildDirector>();


            DataRow row = GetRow(TableNameSet.COURSES, id);

            return director.BuildCourse(new CourseBuilder(), (string)row["Name"], (int)row["TeacherId"], (int)row["Id"]);
        }

        public Human GetHuman(int id, string from)
        {
            ///If you select from teachers, then id will be teacher id, not human id
            ///same for students and humans (humans will have human id, logically)
            var app = (App)Application.Current;
            IBuildDirector director = app.Services.GetService<IBuildDirector>();


            DataRow row = GetRow(from,id);
            
            switch (from)
            {
                case TableNameSet.HUMANS:
                    return director.BuildHuman(new HumanBuilder(), (string)row["FirstName"], (string)row["LastName"], (int)row["Age"], (int)row["Id"]);

                case TableNameSet.TEACHERS:
                    return director.BuildHuman(new TeacherBuilder(), (string)row["FirstName"], (string)row["LastName"], (int)row["Age"], (int)row["Id"]);

                case TableNameSet.STUDENTS:
                    return director.BuildHuman(new StudentBuilder(), (string)row["FirstName"], (string)row["LastName"], (int)row["Age"], (int)row["Id"]);
                default: return null;

            }
        }
    }
}
