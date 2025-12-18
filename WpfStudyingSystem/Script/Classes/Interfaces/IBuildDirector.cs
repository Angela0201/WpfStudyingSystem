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
        Human BuildHuman(IHumanBuilder builder, string firstName, string lastName, int age);
        Course BuildCourse(ICourseBuilder builder, string name, int teacherId);
        Assignment BuildAssignment(IAssignmentBuilder builder, string name, string description, DateTime date);
    }
}
