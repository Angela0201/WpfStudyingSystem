using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfStudyingSystem.Script.DatabaseScript.Interfaces
{
    public interface IDatabaseConnectionString
    {
        //Need to manage connection string (does it need "set" on name?)
        string ConnectionString {  get; }
        string DatabaseName { get; set; }
    }
}
