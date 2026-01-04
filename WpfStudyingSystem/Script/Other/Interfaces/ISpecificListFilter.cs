using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Classes.Interfaces;

namespace WpfStudyingSystem.Script.Other.Interfaces
{
    public interface ISpecificListFilter
    {
        //Filters student list by selected criterias
        List<T> SortListByFirstName<T>(List<T> lst) where T: ICompositNameHolder;
        List<T> SortListByLastName<T>(List<T> lst) where T : ICompositNameHolder;

        List<T> SortListBySimpleNameName<T>(List<T> lst) where T : ISimpleNameHolder;

        List<T> SortListByAge<T>(List<T> lst) where T : IAged;
    }
}
