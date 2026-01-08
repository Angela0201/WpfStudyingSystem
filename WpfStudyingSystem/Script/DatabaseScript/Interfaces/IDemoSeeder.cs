using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Interfaces;

namespace WpfStudyingSystem.Script.DatabaseScript.Interfaces
{
    public interface IDemoSeeder
    {
        void SeedIfEmpty(IDatabaseController controller);
    }
}
