using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.BaseEntities;

namespace WpfStudyingSystem.Script.Classes.Instances.Humans
{
    public class BasicHuman : Human
    {
        public BasicHuman(int gIndex, string gFirstName, string glastName, int gAge) : base(gIndex, gFirstName, glastName, gAge)
        {
        }
    }
}
