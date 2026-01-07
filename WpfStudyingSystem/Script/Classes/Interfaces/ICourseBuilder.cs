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
        //builder that need to be passed to director to build courses

        void Reset();
        void SetName(string name);
        void SetTeacher(int teacherId);
        void SetId(int id);
        Course Build();
    }
}
