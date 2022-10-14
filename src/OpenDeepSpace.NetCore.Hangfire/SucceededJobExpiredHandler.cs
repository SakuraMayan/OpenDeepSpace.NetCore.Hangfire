using Hangfire.States;
using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    /// <summary>
    /// 成功的Job在持久化中驻留时间/过期时间
    /// </summary>
    public class SucceededJobExpiredHandler : IStateHandler
    {

        public string StateName => SucceededState.StateName;

        /// <summary>
        /// Job过期时间
        /// </summary>
        private readonly TimeSpan JobExpirationTimeout;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="JobExpirationTimeoutMins">过期分钟数 默认跟随Hangfire的默认过期时间1天 即成功的数据只保留1天前的</param>
        public SucceededJobExpiredHandler(int JobExpirationTimeoutMins = 24 * 60)
        {

            JobExpirationTimeout = TimeSpan.FromMinutes(JobExpirationTimeoutMins);
        }

        public void Apply(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = JobExpirationTimeout;
        }

        public void Unapply(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {

        }
    }
}
