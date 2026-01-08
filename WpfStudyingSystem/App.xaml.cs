using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.DatabaseScript.Usables;
using WpfStudyingSystem.Script.Interfaces;
using WpfStudyingSystem.Script.Services;
using WpfStudyingSystem.Script.Services.Interfaces;

namespace WpfStudyingSystem
{
    public partial class App : Application
    {
        private IServiceHolder services = new ServiceHolder();
        public IServiceProvider Services => services.Services;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var controller = Services.GetService<IDatabaseController>();
            if (controller != null)
            {
                controller.GenerateDatabase();
            }

            var seeder = Services.GetService<IDemoSeeder>();
            if (seeder != null && controller != null)
            {
                seeder.SeedIfEmpty(controller);
            }
        }
    }
}
