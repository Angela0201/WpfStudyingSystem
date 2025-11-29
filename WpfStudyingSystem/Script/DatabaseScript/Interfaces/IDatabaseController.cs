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
        void RecordToTable(string tableName, DataRow data);
        DataTable GetFullTable(string tableName);
        DataRow GetNewDataRow(string tableName);
    }
}
