using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;

namespace WpfStudyingSystem.Script.DatabaseScript.Usables
{
    public class DatabaseConnectionString : IDatabaseConnectionString
    {
        private string databaseName = "StudySystemDB";
        public string DatabaseName { get => databaseName; set => databaseName = value; }
        public string ConnectionString => @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\" +databaseName+@".mdf;Integrated Security=True";
    }
}
