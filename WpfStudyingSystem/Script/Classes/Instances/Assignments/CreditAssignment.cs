using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Classes.BaseEntities.Sets;

namespace WpfStudyingSystem.Script.Classes.Instances.Assignments
{
    public class CreditAssignment : Assignment
    {
        public CreditAssignment(int gId, string gName, DateTime gDate, string gDescription, AssignmentTypesEnum gType) : base(gId, gName, gDate, gDescription, gType)
        {
        }

        public override int GetResults(int points)
        {
            if (points > 50) { return 1; }
            else { return 0; }
        }
    }
}
