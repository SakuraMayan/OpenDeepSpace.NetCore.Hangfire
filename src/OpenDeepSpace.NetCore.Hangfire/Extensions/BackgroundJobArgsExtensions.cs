using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire.Extensions
{
    public static class BackgroundJobArgsExtensions
    {
        public static Type GetJobArgsType(this Type jobType)
        {
            foreach (var @interface in jobType.GetInterfaces())
            {
                if (!@interface.IsGenericType)
                {
                    continue;
                }

                if (@interface.GetGenericTypeDefinition() != typeof(IBackgroundJob<>) &&
                    @interface.GetGenericTypeDefinition() != typeof(IAsyncBackgroundJob<>))
                {
                    continue;
                }

                var genericArgs = @interface.GetGenericArguments();
                if (genericArgs.Length != 1)
                {
                    continue;
                }

                return genericArgs[0];
            }

            throw new Exception($"Could not find type of the job args. " +
                                   $"Ensure that given type implements the {typeof(IBackgroundJob<>).AssemblyQualifiedName} or {typeof(IAsyncBackgroundJob<>).AssemblyQualifiedName} interface. " +
                                   $"Given job type: {jobType.AssemblyQualifiedName}");
        }
    }
}
