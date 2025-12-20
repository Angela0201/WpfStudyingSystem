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
        public Assignment BuildAssignment(IAssignmentBuilder builder, string name, string description, DateTime date, int id = -1)
        {
            builder.Reset();

            builder.SetName(name);
            builder.SetDescription(description);
            builder.SetDate(date);

            if (id > -1) { builder.SetId(id); }

            return builder.Build();
        }

        public Course BuildCourse(ICourseBuilder builder, string name, int teacherId, int id = -1)
        {
            builder.Reset();

            builder.SetName(name);
            builder.SetTeacher(teacherId);

            if (id > -1) { builder.SetId(id); }

            return builder.Build();
        }

        public Human BuildHuman(IHumanBuilder builder, string firstName, string lastName, int age, int id = -1)
        {
            builder.Reset();

            builder.SetName(firstName, lastName);
            builder.SetAge(age);

            if (id > -1) { builder.SetId(id); }

            return builder.Build();
        }
    }
}
