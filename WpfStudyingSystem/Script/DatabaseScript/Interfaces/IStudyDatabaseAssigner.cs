using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfStudyingSystem.Script.DatabaseScript.Interfaces
{
    public interface IStudyDatabaseAssigner
    {
        //This will be the script for assigning entities to database, to avoid spagetti
        //but right now idk how we will send data about entities, so it will be modified and implemented later on
        
        void AssignStudent();
        void AssignTeacher();
        void AssignAssignment();
        void AssignCourse();
    }
}
