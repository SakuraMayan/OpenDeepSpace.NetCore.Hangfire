using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    public abstract class AsyncBackgroundJob<TArgs> : IAsyncBackgroundJob<TArgs>
    {

        public ILogger<AsyncBackgroundJob<TArgs>> Logger { get; set; }

        protected AsyncBackgroundJob()
        {
            Logger = NullLogger<AsyncBackgroundJob<TArgs>>.Instance;
        }

        public abstract Task ExecuteAsync(TArgs args);
    }
}
