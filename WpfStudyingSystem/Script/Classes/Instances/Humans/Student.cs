using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.Interfaces;

namespace WpfStudyingSystem.Script.Classes.BaseEntities
{
    public class Student : Human
    {
        public Student(int gIndex, string gFirstName, string glastName, int gAge) : base(gIndex, gFirstName, glastName, gAge)
        {
        }
    }
}
