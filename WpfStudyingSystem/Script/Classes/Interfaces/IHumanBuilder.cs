using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.BaseEntities;

namespace WpfStudyingSystem.Script.Classes.Interfaces
{
    public interface IHumanBuilder
    {
        //builder that need to be passed to director to build humans

        void Reset();
        void SetName(string firstName, string lastName);
        void SetAge(int age);
        void SetId(int id);
        Human Build();
    }
}
