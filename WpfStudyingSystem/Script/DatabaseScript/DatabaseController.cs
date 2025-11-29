using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Usables;
using WpfStudyingSystem.Script.Interfaces;

namespace WpfStudyingSystem.Script.DatabaseScript
{
    public class DatabaseController : IDatabaseController
    {
        private IDatabaseGenerator databaseGenerator = new DatabaseGenerator();
        private IDatabaseConnectionString databaseConnectionString = new DatabaseConnectionString();

        string ConnStr => databaseConnectionString.ConnectionString;
        public void GenerateDatabase()
        {
            databaseGenerator.GenerateDatabase(ConnStr);
        }

        public DataTable GetFullTable(string tableName)
        {
            var conn = new SqlConnection(ConnStr);
            conn.Open();


            var adapter = new SqlDataAdapter("SELECT * FROM " + tableName, conn);
            var table = new DataTable();
            adapter.Fill(table);

            conn.Close();

            return table;
        }

        public void ExecuteCommand(string command)
        {
            var conn = new SqlConnection(ConnStr);
            conn.Open();

            var cmd = new SqlCommand(command, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
