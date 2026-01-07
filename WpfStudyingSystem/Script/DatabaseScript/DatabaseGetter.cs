using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using WpfStudyingSystem.Script.Classes.BaseEntities;
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
            switch (tableName)
            {
                case TableNameSet.STUDENTS:
                    return GetStudentsTable();
                case TableNameSet.TEACHERS:
                    return GetTeacherTable();
                default:
                    break;
            }

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
            //not the best solution, but considering how the whole project is written and the time left,
            //its better than errors

            string cmd;
            switch (tableName)
            {
                case TableNameSet.STUDENTS:
                    cmd = $@"INSERT INTO {TableNameSet.STUDENTS} (HumanId)
 VALUES (0);";
                    break;
                case TableNameSet.TEACHERS:
                    cmd = $@"INSERT INTO {TableNameSet.TEACHERS} (HumanId)
 VALUES (0);";
                    break;
                case TableNameSet.HUMANS:
                    cmd = $@"INSERT INTO {TableNameSet.HUMANS} ( FirstName, LastName, Age)
 VALUES ('test', 'test', 20);";
                    break;
                case TableNameSet.COURSES:
                    cmd = $@"INSERT INTO {TableNameSet.COURSES} (Name, TeacherId)
 VALUES ('test', 0)";
                    break;
                case TableNameSet.ASSIGNMENTS:
                    cmd = $@"INSERT INTO {TableNameSet.ASSIGNMENTS} ( Name, Date, Description, Type)
 VALUES ('test', 00-00-0000, 'test', 0);";
                    break;
                default:
                    return 1;
            }

            var conn = new SqlConnection(ConnStr);
            conn.Open();

            var command = new SqlCommand(cmd, conn);
            command.ExecuteNonQuery();
            command = new SqlCommand($"SELECT Id FROM {tableName} ORDER BY Id Desc", conn); 
            int nid = (int)command.ExecuteScalar();
            command = new SqlCommand($"DELETE FROM {tableName} WHERE Id = {nid}", conn);
            command.ExecuteNonQuery();

            conn.Close();

            return nid + 1;
            //
        }

        private DataTable GetStudentsTable()
        {
            var conn = new SqlConnection(ConnStr);
            conn.Open();

            var adapter = new SqlDataAdapter($"SELECT * FROM {TableNameSet.STUDENTS}"+
                                             $" LEFT JOIN {TableNameSet.HUMANS} ON {TableNameSet.STUDENTS}.HumanId = {TableNameSet.HUMANS}.Id", conn);
            var table = new DataTable();
            adapter.Fill(table);

            conn.Close();

            return table;
        }

        private DataTable GetTeacherTable()
        {
            var conn = new SqlConnection(ConnStr);
            conn.Open();

            var adapter = new SqlDataAdapter($"SELECT * FROM {TableNameSet.TEACHERS}" +
                                             $" LEFT JOIN {TableNameSet.HUMANS} ON {TableNameSet.TEACHERS}.HumanId = {TableNameSet.HUMANS}.Id", conn);
            var table = new DataTable();
            adapter.Fill(table);

            conn.Close();

            return table;
        }
    }
}
