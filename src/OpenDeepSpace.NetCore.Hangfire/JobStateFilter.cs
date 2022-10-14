using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.DependencyInjection;
using OpenDeepSpace.NetCore.Hangfire.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    /// <summary>
    /// Job状态监控
    /// </summary>
    public class JobStateFilter : IElectStateFilter, IServerFilter, IApplyStateFilter
    {
        private IServiceCollection Services { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public JobStateFilter(IServiceCollection services)
        {
            this.Services = services;
        }

        /// <summary>
        /// 执行完成
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnPerformed(PerformedContext filterContext)
        {
            //var retryCount = filterContext.GetJobParameter<int>("RetryCount");
            // you have an option to move all code here on OnPerforming if you want.
        }

        /// <summary>
        /// 执行中
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnPerforming(PerformingContext filterContext)
        {
            // do nothing

        }


        /// <summary>
        /// 状态转换
        /// </summary>
        /// <param name="context"></param>
        public void OnStateElection(ElectStateContext context)
        {
            // all failed job after retry attempts comes here
            // var failedState = context.CandidateState as FailedState;
            // if (failedState == null) return;
            if (context.CandidateState is EnqueuedState enqueuedState)//进入队列状态
            {
                //队列名称变成小写
                if (context.BackgroundJob.Job.Args.Any())
                { 
                    string? queue = BackgroundJobQueueAttribute.GetQueue(context.BackgroundJob.Job.Args[0].GetType());
                    if (!string.IsNullOrWhiteSpace(queue)) enqueuedState.Queue = queue.ToLower();
                }
            }
                    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="transaction"></param>
        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="transaction"></param>
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {

            var serviceProvider = Services.BuildServiceProvider();
            var jobExcuteResultHandler = serviceProvider.GetService<IJobExecuteResultHandler>();
            if (jobExcuteResultHandler == null)
                return;
            //一次Job执行完成最终状态在这里 
            //达到最大重试次数之后 如果状态为失败 表示任务执行失败
            if ((context.NewState as FailedState) != null)
            {
                jobExcuteResultHandler.Failed(context);
            }
            if ((context.NewState as SucceededState) != null)//Job执行成功
            {
                jobExcuteResultHandler.Succeeded(context);

            }

        }

    }
}
