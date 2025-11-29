using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfStudyingSystem.Script.DatabaseScript;
using WpfStudyingSystem.Script.Interfaces;

namespace WpfStudyingSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IDatabaseController databaseController = new DatabaseController();
        public App()
        {
            databaseController.GenerateDatabase();
        }
    }
}
