using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BackgroundJobQueueAttribute : Attribute
    {
        public string Queue { get; }
        public BackgroundJobQueueAttribute(string queue)
        {
            Queue = queue ?? nameof(queue);
        }

        public static string? GetQueue<TJobArgs>()
        {
            return GetQueue(typeof(TJobArgs));
        }

        public static string? GetQueue(Type jobArgsType)
        {

            return jobArgsType?
                       .GetCustomAttribute<BackgroundJobQueueAttribute>()?.Queue
                   ?? null;
        }
    }
}
