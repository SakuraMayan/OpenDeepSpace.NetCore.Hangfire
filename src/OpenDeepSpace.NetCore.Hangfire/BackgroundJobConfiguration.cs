using OpenDeepSpace.NetCore.Hangfire.Attributes;
using OpenDeepSpace.NetCore.Hangfire.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    public class BackgroundJobConfiguration
    {
        public Type ArgsType { get; }

        public Type JobType { get; }

        public string? JobName { get; }

        public BackgroundJobConfiguration(Type jobType)
        {
            JobType = jobType;
            ArgsType = jobType.GetJobArgsType();
            JobName = BackgroundJobNameAttribute.GetName(ArgsType);
        }
    }
}
