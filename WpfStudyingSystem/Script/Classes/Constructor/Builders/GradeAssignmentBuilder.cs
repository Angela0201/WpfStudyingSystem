using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Classes.BaseEntities.Sets;
using WpfStudyingSystem.Script.Classes.Instances.Assignments;
using WpfStudyingSystem.Script.Classes.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Usables;

namespace WpfStudyingSystem.Script.Classes.Constructor.Builders
{
    public class GradeAssignmentBuilder: IAssignmentBuilder
    {
        private int nId = -1;
        private string nName = "NotImplemented";

        private DateTime nDate = DateTime.Now;
        private string nDescription = "";
        private AssignmentTypesEnum nType;
        public Assignment Build()
        {
            //somehoq, i cant get it in one line, only separated by this,
            //and "app" is considered as static, so i cant acces it directly
            if (nId < 0)
            {
                var app = (App)Application.Current;
                IDatabaseGetter getter = app.Services.GetService<IDatabaseGetter>();

                nId = getter.GetUniqueId(TableNameSet.ASSIGNMENTS);
            }
            nType = AssignmentTypesEnum.Grade;

            return new CreditAssignment(nId, nName, nDate, nDescription, nType);
        }

        public void Reset()
        {
            nId = -1;
            nName = "NotImplemented";
            nDate = DateTime.Now;
            nDescription = "";
        }

        public void SetDate(DateTime date)
        {
            nDate = date;
        }

        public void SetDescription(string desc)
        {
            nDescription = desc;
        }

        public void SetId(int id)
        {
            nId = id;
        }

        public void SetName(string name)
        {
            nName = name;
        }
    }
}
