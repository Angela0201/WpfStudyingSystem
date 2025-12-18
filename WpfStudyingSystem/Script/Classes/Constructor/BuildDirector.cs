using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Classes.Interfaces;

namespace WpfStudyingSystem.Script.Classes.Constructor
{
    public class BuildDirector : IBuildDirector
    {
        public Assignment BuildAssignment(IAssignmentBuilder builder, string name, string description, DateTime date)
        {
            builder.Reset();

            builder.SetName(name);
            builder.SetDescription(description);
            builder.SetDate(date);

            return builder.Build();
        }

        public Course BuildCourse(ICourseBuilder builder, string name, int teacherId)
        {
            builder.Reset();

            builder.SetName(name);
            builder.SetTeacher(teacherId);

            return builder.Build();
        }

        public Human BuildHuman(IHumanBuilder builder, string firstName, string lastName, int age)
        {
            builder.Reset();

            builder.SetName(firstName, lastName);
            builder.SetAge(age);

            return builder.Build();
        }
    }
}
