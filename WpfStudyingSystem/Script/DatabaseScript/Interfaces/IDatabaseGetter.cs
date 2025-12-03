using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfStudyingSystem.Script.DatabaseScript.Interfaces
{
    public interface IDatabaseGetter
    {
        DataTable GetTable(string tableName);
        DataRow GetRow(int id, string tableName);
        int GetUniqueId(string tableName);
    }
}
