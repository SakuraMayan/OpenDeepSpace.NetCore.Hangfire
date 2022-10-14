using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenDeepSpace.NetCore.Hangfire.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    public class HangfireJobExecutionAdapter<TArgs> where TArgs : notnull
    {

        protected BackgroundJobOptions Options { get; }
        protected IServiceScopeFactory ServiceScopeFactory { get; }
        protected IBackgroundJobExecuter JobExecuter { get; }

        public HangfireJobExecutionAdapter(
            IOptions<BackgroundJobOptions> options,
            IBackgroundJobExecuter jobExecuter,
            IServiceScopeFactory serviceScopeFactory)
        {
            JobExecuter = jobExecuter;
            ServiceScopeFactory = serviceScopeFactory;
            Options = options.Value;
        }

        [JobDisplayNameAdapter("ExecuteAsync")]
        public async Task ExecuteAsync(TArgs args)
        {
            if (!Options.IsJobExecutionEnabled)
            {
                throw new Exception(
                    "Background job execution is disabled. " +
                    "This method should not be called! " +
                    "If you want to enable the background job execution, " +
                    $"set {nameof(BackgroundJobOptions)}.{nameof(BackgroundJobOptions.IsJobExecutionEnabled)} to true! " +
                    "If you've intentionally disabled job execution and this seems a bug, please report it."
                );
            }

            using var scope = ServiceScopeFactory.CreateScope();
            var jobType = Options.GetJob(typeof(TArgs)).JobType;
            var context = new JobExecutionContext(scope.ServiceProvider, jobType, args);
            await JobExecuter.ExecuteAsync(context);
        }
    }
}
