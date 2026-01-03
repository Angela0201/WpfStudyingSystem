using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Classes.BaseEntities.Sets;
using WpfStudyingSystem.Script.DatabaseScript.Usables;
using WpfStudyingSystem.Script.Interfaces;
using WpfStudyingSystem.Script.Other.Sets;

namespace WpfStudyingSystem.Script.Other.Interfaces
{
    public class Informator : IInformator
    {
        private List<Human> studentList = new List<Human>();
        private List<StudentGradeInfo> gradeList = new List<StudentGradeInfo>(); 
        private int courseId = 0;

        public int GetAverangeAge()
        {
            return Convert.ToInt32(studentList.Average(a => a.Age));
        }

        public int GetAverangeGrade()
        {
            return Convert.ToInt32(gradeList.Average(g => g.Points));
        }

        public int GetHighestAge()
        {
            return studentList.Max(a => a.Age);
        }

        public int GetHighestGrade()
        {
            return gradeList.Max(g => g.Points);
        }

        public int GetLowestAge()
        {
            return studentList.Min(a => a.Age);
        }

        public int GetLowestGrade()
        {
            return gradeList.Max(g => g.Points);
        }

        public List<StudentGradeInfo> GetStudentGradeInfoList()
        {
            return gradeList;
        }

        public void LoadStudentList(List<Human> sLst, int cId)
        {
            studentList = sLst;
            courseId = cId;

            var app = (App)Application.Current;
            IDatabaseController dtc = app.Services.GetService<IDatabaseController>();

            string cmd = 
                $"SELECT {TableNameSet.ASSIGNMENTS_DEPENDENCIES}.AssignmentId AS AssignmentId, {TableNameSet.ASSIGNMENTS_DEPENDENCIES}.CourseId AS CourseId, {TableNameSet.ASSIGNMENTS}.Name AS AssignmentName, {TableNameSet.ASSIGNMENTS}.Type AS Type, {TableNameSet.ASSIGNMENTS_STATISTICS}.Points AS Points, {TableNameSet.ASSIGNMENTS_STATISTICS}.StudentId AS StudentId, {TableNameSet.HUMANS}.FirstName AS FirstName, {TableNameSet.HUMANS}.LastName AS LastName" +
                $" FROM {TableNameSet.ASSIGNMENTS_DEPENDENCIES}" +
                $" LEFT JOIN {TableNameSet.ASSIGNMENTS_STATISTICS} ON {TableNameSet.ASSIGNMENTS_DEPENDENCIES}.AssignmentId = {TableNameSet.ASSIGNMENTS_STATISTICS}.AssignmentId" +
                $" LEFT JOIN {TableNameSet.ASSIGNMENTS} ON {TableNameSet.ASSIGNMENTS_DEPENDENCIES}.AssignmentId = {TableNameSet.ASSIGNMENTS}.Id" +
                $" LEFT JOIN {TableNameSet.STUDENTS} ON {TableNameSet.ASSIGNMENTS_STATISTICS}.StudentId = {TableNameSet.STUDENTS}.Id" +
                $" LEFT JOIN {TableNameSet.HUMANS} ON {TableNameSet.STUDENTS}.HumanId = {TableNameSet.HUMANS}.Id" +
                $" WHERE {TableNameSet.ASSIGNMENTS_DEPENDENCIES}.CourseId = {courseId};";

            DataTable table = dtc.ExecuteReturnCommand(cmd);
            gradeList = new List<StudentGradeInfo>();

            foreach (DataRow row in table.Rows)
            {
                //MessageBox.Show($"StudentId - {row["StudentId"]}\nAssignmentId - {row["AssignmentId"]}\nFirstName - {row["FirstName"]}\nLastName - {row["LastName"]}\nAssignmentName - {row["AssignmentName"]}\nPoints - {row["Points"]}\nType - {row["Type"]}");
                gradeList.Add(new StudentGradeInfo(
                (int)row["StudentId"],
                (int)row["AssignmentId"],
                (string)row["FirstName"],
                (string)row["LastName"],
                (string)row["AssignmentName"],
                (int)row["Points"],
                (AssignmentTypesEnum)row["Type"]));
            }

            gradeList = gradeList.OrderByDescending(n => n.AssignmentName).ToList();
        }
    }

}
