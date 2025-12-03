using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.BaseEntities;

namespace WpfStudyingSystem.Script.Classes.Instances.Assignments
{
    public class GradeAssignment : Assignment
    {
        public GradeAssignment(int gId, string gName, DateTime gDate, string gDescription, int gType) : base(gId, gName, gDate, gDescription, gType)
        {
        }

        public override int GetResults(int points)
        {
            return ((int)Math.Round(((double)points / 100.0) * 4.0) + 1);
        }
    }
}
