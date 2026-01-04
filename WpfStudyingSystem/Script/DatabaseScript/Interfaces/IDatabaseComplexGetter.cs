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
        //gets items from database by correesponding id
        Human GetHuman(int id, string from);
        Assignment GetAssignment(int id);
        Course GetCourse(int id);
    }
}
