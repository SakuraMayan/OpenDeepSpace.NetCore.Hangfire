using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    public abstract class BackgroundJob<TArgs> : IBackgroundJob<TArgs>
    {

        public ILogger<BackgroundJob<TArgs>> Logger { get; set; }

        protected BackgroundJob()
        {
            Logger = NullLogger<BackgroundJob<TArgs>>.Instance;
        }

        public abstract void Execute(TArgs args);
    }
}
