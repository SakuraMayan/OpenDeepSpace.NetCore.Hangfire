using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    public class HangfireBackgroundJobManager : IBackgroundJobManager
    {
        /// <summary>
        ///  Enqueues a job to be executed.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="args"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public Task<string> EnqueueAsync<TArgs>(TArgs args, TimeSpan? delay = null) where TArgs : notnull
        {
            if (!delay.HasValue)
            {
                return Task.FromResult(
                    BackgroundJob.Enqueue<HangfireJobExecutionAdapter<TArgs>>(
                        adapter =>
                        adapter.ExecuteAsync(args)
                    )
                );
            }
            else
            {
                return Task.FromResult(
                    BackgroundJob.Schedule<HangfireJobExecutionAdapter<TArgs>>(
                        adapter => adapter.ExecuteAsync(args),
                        delay.Value
                    )
                );
            }
        }

        /// <summary>
        /// Enqueues a recurringJob  to be executed.
        /// </summary>
        /// <typeparam name="TArgs">Type of the arguments of job.</typeparam>
        /// <param name="args">Job arguments.</param>
        /// <param name="cronExpression">Cron Expression.</param>
        /// <param name="timeZone">timeZone.</param>
        /// <returns>Unique identifier of a background job.</returns>
        public Task<string> EnqueueAsync<TArgs>(TArgs args, string cronExpression, TimeZoneInfo? timeZone = null) where TArgs: notnull
        {
            string recurringJobId = args.ToString() ?? Guid.NewGuid().ToString();

            RecurringJob.AddOrUpdate<HangfireJobExecutionAdapter<TArgs>>(recurringJobId, adapter => adapter.ExecuteAsync(args), cronExpression, timeZone);
            return Task.FromResult(recurringJobId);
        }

        /// <summary>
        /// Enqueues a recurringJob to be executed.
        /// </summary>
        /// <typeparam name="TArgs">Type of the arguments of job.</typeparam>
        /// <param name="args">Job arguments.</param>
        /// <param name="jobId">jobId</param>
        /// <param name="cronExpression">Cron Expression.</param>
        /// <param name="timeZone">TimeZoneInfo.</param>
        /// <returns>Unique identifier of a background job.</returns>
        public Task<string> EnqueueAsync<TArgs>(TArgs args, string jobId, string cronExpression, TimeZoneInfo? timeZone = null) where TArgs:notnull
        {
            RecurringJob.AddOrUpdate<HangfireJobExecutionAdapter<TArgs>>(jobId, adapter => adapter.ExecuteAsync(args), cronExpression, timeZone);
            return Task.FromResult(jobId);
        }


        /// <summary>
        /// Delete recurringJob
        /// </summary>
        /// <param name="jobId">job id</param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
            return Task.FromResult(true);
        }

        public Task<string> ContinueJobWith<TArgs>(string parentId, TArgs args) where TArgs:notnull
        {
            return Task.FromResult(BackgroundJob.ContinueJobWith<HangfireJobExecutionAdapter<TArgs>>(parentId, adapter => adapter.ExecuteAsync(args)));
        }
    }
}
