using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfStudyingSystem.Script.Classes.Interfaces
{
    public interface ICompositNameHolder
    {
        //for classes with full and second name
        string FirstName { get; }
        string LastName { get; }
        string FullName { get; }
    }
}
