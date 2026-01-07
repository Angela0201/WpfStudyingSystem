using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Classes.BaseEntities.Sets;

namespace WpfStudyingSystem.Script.Classes.Interfaces
{
    public interface IBuildDirector
    {
        //director eats builders for breckfast and manages the class creation
        Human BuildHuman(IHumanBuilder builder, string firstName, string lastName, int age, int id = -1);
        Course BuildCourse(ICourseBuilder builder, string name, int teacherId, int id = -1);
        Assignment BuildAssignment(IAssignmentBuilder builder, string name, string description, DateTime date, int id = -1);
    }
}
