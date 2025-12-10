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

namespace WpfStudyingSystem.Script.ViewModels
{
    public class MainViewModel
    {
        private readonly IDatabaseGetter getter;

        public ObservableCollection<Course> Courses { get; }
            = new ObservableCollection<Course>();

        public MainViewModel()
        {
            var app = (App)Application.Current;
            getter = app.Services.GetService<IDatabaseGetter>();
            LoadCourses();
        }

        private void LoadCourses()
        {
            try
            {
                DataTable table = getter.GetTable(TableNameSet.COURSES);

                foreach (DataRow row in table.Rows)
                {
                    var course = new Course(
                        (int)row["Id"],
                        (string)row["Name"],
                        (int)row["TeacherId"]);

                    Courses.Add(course);
                }
            }
            catch
            {
            }
        }
    }
}
