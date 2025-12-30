using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.BaseEntities;

namespace WpfStudyingSystem.Script.DatabaseScript.Interfaces
{
    public interface IDatabaseComplexGetter
    {
        Human GetHuman(int id, string from);
        Assignment GetAssignment(int id);
        Course GetCourse(int id);
    }
}
