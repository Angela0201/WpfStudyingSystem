using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.Classes.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Usables;

namespace WpfStudyingSystem.Script.Classes.Constructor.Builders
{
    public class CourseBuilder : ICourseBuilder
    {
        private int nId = -1;
        private string nName = "NotImplemented";
        private int nTeacherId = 0;

        public Course Build()
        {
            //somehoq, i cant get it in one line, only separated by this,
            //and "app" is considered as static, so i cant acces it directly
            if (nId < 0) 
            {
                var app = (App)Application.Current;
                IDatabaseGetter getter = app.Services.GetService<IDatabaseGetter>();

                nId = getter.GetUniqueId(TableNameSet.COURSES);
            }
            

            return new Course(nId,nName,nTeacherId);
        }

        public void Reset()
        {
            nId = -1;
            nName = "NotImplemented";
            nTeacherId = 0;
        }

        public void SetId(int id)
        {
            nId = id;
        }

        public void SetName(string name)
        {
            nName = name;
        }

        public void SetTeacher(int teacherId)
        {
            nTeacherId = teacherId;
        }
    }
}
