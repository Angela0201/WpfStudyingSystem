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



            conn.Close();
            return new DataTable();
        }

        public DataRow GetNewDataRow(string tableName)
        {
            throw new NotImplementedException();
        }

        public void RecordToTable(string tableName, DataRow data)
        {
            throw new NotImplementedException();
        }
    }
}
