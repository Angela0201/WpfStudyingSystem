using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.BaseEntities;

namespace WpfStudyingSystem.Script.Classes.Interfaces
{
    public interface ICourseBuilder
    {
        void Reset();
        void SetName(string name);
        void SetTeacher(int teacherId);
        Course Build();
    }
}
