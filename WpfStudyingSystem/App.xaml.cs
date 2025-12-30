using Microsoft.Extensions.DependencyInjection;
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
using WpfStudyingSystem.Script.Services;
using WpfStudyingSystem.Script.Services.Interfaces;

namespace WpfStudyingSystem
{
    public partial class App : Application
    {
        private IServiceHolder services = new ServiceHolder();
        public IServiceProvider Services => services.Services;
    }
}
