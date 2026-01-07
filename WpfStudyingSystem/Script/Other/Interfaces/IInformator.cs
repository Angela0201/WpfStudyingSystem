using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Other.Sets;

namespace WpfStudyingSystem.Script.Other.Interfaces
{
    public interface IInformator
    {
        //give some statistics about students and grades after student list loading

        void LoadStudentList(List<Human> sLst, int cId);
        List<StudentGradeInfo> GetStudentGradeInfoList();

        int GetAverangeAge();
        int GetHighestAge();
        int GetLowestAge();

        int GetAverangeGrade();
        int GetHighestGrade();
        int GetLowestGrade();
    }
}
