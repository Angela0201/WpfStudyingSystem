using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.DatabaseScript.Usables;

namespace WpfStudyingSystem.Script.DatabaseScript.Interfaces
{
    public interface IDatabaseGetter
    {
        DataTable GetTable(string tableName);
        DataRow GetRow(int id, string tableName);

        //
        DataTable GetAssignmentTableViaCourseId(int CourseId);

        DataTable GetStudentTableViaCourseId(int CourseId);
        //


        int GetUniqueId(string tableName);
    }
}
