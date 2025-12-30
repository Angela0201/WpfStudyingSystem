using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WpfStudyingSystem.Script.Classes.BaseEntities;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Usables;
using WpfStudyingSystem.Script.Classes.Interfaces;
using WpfStudyingSystem.Script.Classes.Constructor.Builders;

namespace WpfStudyingSystem.Script.ViewModels
{
    public class MainViewModel
    {
        private readonly IDatabaseGetter getter;
        private readonly IDatabaseSetter setter;
        private readonly IDatabaseComplexGetter complexGetter;
        private readonly IBuildDirector director;

        public ObservableCollection<Course> Courses { get; }
            = new ObservableCollection<Course>();

        public MainViewModel()
        {
            var app = (App)Application.Current;
            getter = app.Services.GetService<IDatabaseGetter>();
            complexGetter = app.Services.GetService<IDatabaseComplexGetter>();
            director = app.Services.GetService<IBuildDirector>();


            UpdateCourses();
        }

        private void UpdateCourses()
        {
            DataTable table = getter.GetTable(TableNameSet.COURSES);

            foreach (DataRow row in table.Rows)
            {
                Courses.Add(director.BuildCourse(new CourseBuilder(),
                    (string)row["Name"],
                    (int)row["TeacherId"],
                    (int)row["Id"]));
            }
        }
    }
}
