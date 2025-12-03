using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfStudyingSystem.Script.Classes.Interfaces
{
    public interface ICompositNameHolder
    {
        string FirstName { get; }
        string LastName { get; }
        string FullName { get; }
    }
}
