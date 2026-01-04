using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.BaseEntities;

namespace WpfStudyingSystem.Script.Classes.Interfaces
{
    public interface IAssignmentBuilder
    {
        //builder that need to be passed to director to build assignmnet
        void Reset();
        void SetName(string name);
        void SetDescription(string desc);
        void SetDate (DateTime date);
        void SetId(int id);
        Assignment Build();
    }
}
