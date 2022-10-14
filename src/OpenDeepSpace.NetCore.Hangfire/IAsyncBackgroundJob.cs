using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    /// <summary>
    /// Defines interface of a background job.
    /// </summary>
    public interface IAsyncBackgroundJob<in TArgs>
    {
        /// <summary>
        /// Executes the job with the <paramref name="args"/>.
        /// </summary>
        /// <param name="args">Job arguments.</param>
        Task ExecuteAsync(TArgs args);
    }
}
