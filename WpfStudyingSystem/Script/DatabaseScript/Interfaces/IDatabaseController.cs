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
        void GenerateDatabase();
        DataTable GetFullTable(string tableName);
        void ExecuteCommand(string command);
    }
}
