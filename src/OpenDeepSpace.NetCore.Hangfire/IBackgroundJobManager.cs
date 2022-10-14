using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    public interface IBackgroundJobManager
    {
        /// <summary>
        /// Enqueues a job to be executed.
        /// </summary>
        /// <typeparam name="TArgs">Type of the arguments of job.</typeparam>
        /// <param name="args">Job arguments.</param>
        /// <param name="delay">Job delay (wait duration before first try).</param>
        /// <returns>Unique identifier of a background job.</returns>
        Task<string> EnqueueAsync<TArgs>(
            TArgs args,
            TimeSpan? delay = null
        ) where TArgs:notnull;

        /// <summary>
        /// Enqueues a recurringJob to be executed.
        /// </summary>
        /// <typeparam name="TArgs">Type of the arguments of job.</typeparam>
        /// <param name="args">Job arguments.</param>
        /// <param name="cronExpression">Cron Expression.</param>
        /// <param name="timeZone">TimeZoneInfo.</param>
        /// <returns>Unique identifier of a background job.</returns>
        Task<string> EnqueueAsync<TArgs>(
            TArgs args,
           string cronExpression,
           TimeZoneInfo? timeZone = null
        ) where TArgs:notnull;

        /// <summary>
        /// Enqueues a recurringJob to be executed.
        /// </summary>
        /// <typeparam name="TArgs">Type of the arguments of job.</typeparam>
        /// <param name="args">Job arguments.</param>
        /// <param name="jobId">jobId</param>
        /// <param name="cronExpression">Cron Expression.</param>
        /// <param name="timeZone">TimeZoneInfo.</param>
        /// <returns>Unique identifier of a background job.</returns>
        Task<string> EnqueueAsync<TArgs>(
            TArgs args,
            string jobId,
           string cronExpression,
           TimeZoneInfo? timeZone = null
        ) where TArgs : notnull;

        /// <summary>
        /// Continute Job
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="parentId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<string> ContinueJobWith<TArgs>(string parentId, TArgs args) where TArgs:notnull;

        /// <summary>
        /// Delete a recurringJob
        /// </summary>
        /// <param name="jobId">job id</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string jobId);
    }
}
