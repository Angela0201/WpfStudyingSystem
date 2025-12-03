using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfStudyingSystem.Script.DatabaseScript;
using WpfStudyingSystem.Script.DatabaseScript.Interfaces;
using WpfStudyingSystem.Script.Interfaces;
using WpfStudyingSystem.Script.Services.Interfaces;

namespace WpfStudyingSystem.Script.Services
{
    public class ServiceHolder : IServiceHolder
    {
        
        private IServiceProvider services;

        public IServiceProvider Services => services;

        public ServiceHolder()
        {

            ServiceCollection sc = new ServiceCollection();

            sc.AddSingleton<IDatabaseController, DatabaseController>();
            sc.AddSingleton<IDatabaseGetter, DatabaseGetter>();

            services = sc.BuildServiceProvider();
        }
    }
}
