using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Usables;
using WpfStudyingSystem.Script.Interfaces;

namespace WpfStudyingSystem.Script.DatabaseScript
{
    public class DatabaseGetter: IDatabaseGetter
    {
        private IDatabaseConnectionString databaseConnectionString = new DatabaseConnectionString();
        private string ConnStr => databaseConnectionString.ConnectionString;

        public DataRow GetRow(int id, string tableName)
        {
            return GetTable(tableName).Select($"Id = {id}")[0];
        }

        public DataTable GetTable(string tableName)
        {
            var conn = new SqlConnection(ConnStr);
            conn.Open();

            var adapter = new SqlDataAdapter($"SELECT * FROM {tableName}", conn);
            var table = new DataTable();
            adapter.Fill(table);

            conn.Close();

            return table;
        }

        public int GetUniqueId(string tableName)
        {
            return GetTable(tableName).Rows.Count + 1;
        }
    }
}
