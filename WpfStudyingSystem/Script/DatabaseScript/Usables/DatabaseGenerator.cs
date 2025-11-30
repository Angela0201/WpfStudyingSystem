using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;

namespace WpfStudyingSystem.Script.DatabaseScript.Usables
{
    public class DatabaseGenerator : IDatabaseGenerator
    {
        public void GenerateDatabase(string ConnStr)
        {

            //Should make a tables, in case there is wome missing, but consider the way tha database works now, idk do we really need this
            //Ill leave it for confidence

            var conn = new SqlConnection(ConnStr);
            conn.Open();

            string table = @"IF OBJECT_ID('Students') IS NULL
                CREATE TABLE Students(
                Id INT NOT NULL PRIMARY KEY IDENTITY,
                FirstName VARCHAR(50) NOT NULL,
                LastName VARCHAR(50) NOT NULL,
                Age INT NOT NULL
                )";
            new SqlCommand(table, conn).ExecuteNonQuery();

            table = @"IF OBJECT_ID('Assignments') IS NULL
                CREATE TABLE Assignments(
                Id INT NOT NULL PRIMARY KEY IDENTITY,
                Date DATETIME,
                Description VARCHAR(300),
                Type INT NOT NULL
                )";
            new SqlCommand(table, conn).ExecuteNonQuery();

            table = @"IF OBJECT_ID('Teachers') IS NULL
                CREATE TABLE Teachers(
                Id INT NOT NULL PRIMARY KEY IDENTITY,
                FirstName VARCHAR(50) NOT NULL,
                LastName VARCHAR(50) NOT NULL
                )";
            new SqlCommand(table, conn).ExecuteNonQuery();



            table = @"IF OBJECT_ID('AssignmentsStatistics') IS NULL
                CREATE TABLE AssignmentsStatistics(
                StudentId INT NOT NULL,
                AssignmentId INT NOT NULL,
                Points INT
                )";
            new SqlCommand(table, conn).ExecuteNonQuery();

            table = @"IF OBJECT_ID('Courses') IS NULL
                CREATE TABLE Courses(
                Id INT NOT NULL PRIMARY KEY IDENTITY,
                Name VARCHAR(50) NOT NULL,
                TeacherId INT NOT NULL
                )";
            new SqlCommand(table, conn).ExecuteNonQuery();

            table = @"IF OBJECT_ID('Drafts') IS NULL
                CREATE TABLE Drafts(
                StudentId INT NOT NULL,
                CourseId INT NOT NULL
                )";
            new SqlCommand(table, conn).ExecuteNonQuery();

            conn.Close();
        }
    }
}
