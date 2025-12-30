using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.Interfaces;

namespace WpfStudyingSystem.Script.Classes.BaseEntities
{
    public class Course : IUnique, ISimpleNameHolder
    {
        private int id;
        private string name;
        private int teacherId;

        public int Id => id;
        public string Name => name;
        public int TeacherId => teacherId;

        public Course(int gId, string gName, int gTeacherId)
        {
            id = gId;
            name = gName;
            teacherId = gTeacherId;
        }
    }
}
