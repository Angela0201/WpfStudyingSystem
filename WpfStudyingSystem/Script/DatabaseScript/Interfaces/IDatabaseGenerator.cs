using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfStudyingSystem.Script.DatabaseScript.Interfaces
{
    public interface IDatabaseGenerator
    {
        //Need mostly to made code more readable
        void GenerateDatabase(string ConnStr);
    }
}
