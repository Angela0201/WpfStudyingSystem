using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Resources;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Classes.BaseEntities.Sets;

namespace WpfStudyingSystem.Script.Other.Sets
{
    public struct StudentGradeInfo
    {
        public string StudentFirstName;
        public string StudentLastName;

        public string AssignmentName;

        public int Points;
        public AssignmentTypesEnum AssignmentType;

        public int StudentId;
        public int AssignmentId;

        public StudentGradeInfo(
            int studentId, int assignmentId, 
            string studentFirstName, string studentLastName, string assignmentName,
            int points, AssignmentTypesEnum assignmentType)
        {
            StudentId = studentId;
            AssignmentId = assignmentId;

            StudentFirstName = studentFirstName;
            StudentLastName = studentLastName;
            AssignmentName = assignmentName;

            Points = points;
            AssignmentType = assignmentType;
        }

        public override string ToString()
        {
            switch (AssignmentType)
            {
                case AssignmentTypesEnum.Credit:
                    string ans = Points == 1 ? Strings.Crd_Pass : Strings.Crd_Failed;
                    return $"{AssignmentName} -> {StudentFirstName} {StudentLastName}: {ans}";
                case AssignmentTypesEnum.EAP:
                    return $"{AssignmentName} -> {StudentFirstName} {StudentLastName}: {Points} EAP";
                case AssignmentTypesEnum.Grade:
                    return $"{AssignmentName} -> {StudentFirstName} {StudentLastName}: {Points} %";
                default:
                    return $"{AssignmentName} -> {StudentFirstName} {StudentLastName}: {Points}";
            }
            
        }
    }
}
