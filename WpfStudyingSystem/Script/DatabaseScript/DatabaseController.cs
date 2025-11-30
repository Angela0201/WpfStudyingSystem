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

        //Current working controller
        //To avoid misspelling, use TableNameSet.cs

        private IDatabaseGenerator databaseGenerator = new DatabaseGenerator();
        private IDatabaseConnectionString databaseConnectionString = new DatabaseConnectionString();

        //Use at start to generate needed tables, in case there is no exist
        string ConnStr => databaseConnectionString.ConnectionString;
        public void GenerateDatabase()
        {
            databaseGenerator.GenerateDatabase(ConnStr);
        }

        //Use to set tables
        public void ExecuteCommand(string command)
        {
            var conn = new SqlConnection(ConnStr);
            conn.Open();

            var cmd = new SqlCommand(command, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        //Use to get tables
        public DataTable ExecuteReturnCommand(string command)
        {
            var conn = new SqlConnection(ConnStr);
            conn.Open();


            var adapter = new SqlDataAdapter(command, conn);
            var table = new DataTable();
            adapter.Fill(table);

            conn.Close();

            return table;
        }
    }
}
