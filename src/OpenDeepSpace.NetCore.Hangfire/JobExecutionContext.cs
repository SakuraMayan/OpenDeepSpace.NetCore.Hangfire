using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    public class JobExecutionContext
    {
        public IServiceProvider ServiceProvider { get; }

        public Type JobType { get; }

        public object JobArgs { get; }

        public JobExecutionContext(IServiceProvider serviceProvider, Type jobType, object jobArgs)
        {
            ServiceProvider = serviceProvider;
            JobType = jobType;
            JobArgs = jobArgs;
        }
    }
}
