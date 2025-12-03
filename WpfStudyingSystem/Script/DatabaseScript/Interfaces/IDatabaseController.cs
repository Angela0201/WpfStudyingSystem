using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfStudyingSystem.Script.Interfaces
{
    public interface IDatabaseController
    {
        //To controll database by hand
        void GenerateDatabase();
        void ExecuteCommand(string command);
        DataTable ExecuteReturnCommand(string command);
    }
}
