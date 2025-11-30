using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfStudyingSystem.Script.DatabaseScript;
using WpfStudyingSystem.Script.DatabaseScript.Usables;
using WpfStudyingSystem.Script.Interfaces;

namespace WpfStudyingSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        //All this for test purposes only

        IDatabaseController databaseController = new DatabaseController();
        public App()
        {
            databaseController.GenerateDatabase();
            databaseController.ExecuteCommand("INSERT Teachers (FirstName, LastName) VALUES ('Bill', 'Jackson');");
            DataTable dt = databaseController.ExecuteReturnCommand("SELECT FirstName FROM Teachers;");
            //DataTable dt = databaseController.GetFullTable(TableNameSet.TEACHERS);
            string str = "";
            foreach (DataRow dr in dt.Rows)
            {
                foreach (object o in dr.ItemArray)
                {
                    str += o.ToString() + ", ";
                }
            }
            MessageBox.Show(str);
        }
    }
}
