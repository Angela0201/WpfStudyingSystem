using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.Interfaces;
using WpfStudyingSystem.Script.Other.Interfaces;

namespace WpfStudyingSystem.Script.Other
{
    public class SpecificListFilter : ISpecificListFilter
    {
        public List<T> SortListByAge<T>(List<T> lst) where T : IAged
        {
            return lst.OrderBy(a => a.Age).ToList();
        }

        public List<T> SortListByFirstName<T>(List<T> lst) where T : ICompositNameHolder
        {
            return lst.OrderByDescending(n => n.FirstName).ToList();
        }

        public List<T> SortListByLastName<T>(List<T> lst) where T : ICompositNameHolder
        {
            return lst.OrderByDescending(n => n.LastName).ToList();
        }

        public List<T> SortListBySimpleNameName<T>(List<T> lst) where T : ISimpleNameHolder
        {
            return lst.OrderByDescending(n => n.Name).ToList();
        }
    }
}
