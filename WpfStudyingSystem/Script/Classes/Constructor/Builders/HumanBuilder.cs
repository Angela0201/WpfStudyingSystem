using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Classes.Instances.Humans;
using WpfStudyingSystem.Script.Classes.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Usables;

namespace WpfStudyingSystem.Script.Classes.Constructor.Builders
{
    public class HumanBuilder: IHumanBuilder
    {
        private int nId = -1;
        private string nFirstName = "NotImplementedName";
        private string nLastName = "NotImplementedLastname";
        private int nAge = 0;

        public Human Build()
        {
            //somehoq, i cant get it in one line, only separated by this,
            //and "app" is considered as static, so i cant acces it directly
            if (nId < 0)
            {
                var app = (App)Application.Current;
                IDatabaseGetter getter = app.Services.GetService<IDatabaseGetter>();

                nId = getter.GetUniqueId(TableNameSet.STUDENTS);
            }

            return new BasicHuman(nId, nFirstName, nLastName, nAge);
        }

        public void Reset()
        {
            nId = -1;
            nFirstName = "NotImplementedName";
            nLastName = "NotImplementedLastname";
            nAge = 0;
        }

        public void SetAge(int age)
        {
            nAge = age;
        }

        public void SetId(int id)
        {
            nId = id;
        }

        public void SetName(string firstName, string lastName)
        {
            nFirstName = firstName;
            nLastName = lastName;
        }

    }
}
