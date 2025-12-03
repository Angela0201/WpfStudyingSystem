using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfStudyingSystem.Script.Services.Interfaces
{
    public interface IServiceHolder
    {
        IServiceProvider Services { get; }
    }
}
