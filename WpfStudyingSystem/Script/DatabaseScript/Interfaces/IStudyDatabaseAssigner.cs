using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfStudyingSystem.Script.DatabaseScript.Interfaces
{
    public interface IStudyDatabaseAssigner
    {
        void AssignStudent();
        void AssignTeacher();
        void AssignAssignment();
        void AssignCourse();
    }
}
