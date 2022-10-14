using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    public class BackgroundJobExecuter : IBackgroundJobExecuter
    {
        //public ILogger<BackgroundJobExecuter> Logger { protected get; set; }

        protected BackgroundJobOptions Options { get; }

        public BackgroundJobExecuter(IOptions<BackgroundJobOptions> options)
        {
            Options = options.Value;

            //Logger = NullLogger<BackgroundJobExecuter>.Instance;
        }

        public virtual async Task ExecuteAsync(JobExecutionContext context)
        {
            var job = context.ServiceProvider.GetService(context.JobType);
            if (job == null)
            {
                throw new Exception("The job type is not registered to DI: " + context.JobType);
            }
            //获取非代理实例 防止被代理之后获取的job实例是代理类型的[或可通过不拦截特性或筛选器不拦截属于Job的类]
            if (ProxyUtil.IsProxy(job))//如果是代理类 获取实际Job类型
            {
                job = ProxyUtil.GetUnproxiedInstance(job);
            }

            var jobExecuteMethod = context.JobType.GetMethod(nameof(IBackgroundJob<object>.Execute)) ??
                                   context.JobType.GetMethod(nameof(IAsyncBackgroundJob<object>.ExecuteAsync));
            if (jobExecuteMethod == null)
            {
                throw new Exception($"Given job type does not implement {typeof(IBackgroundJob<>).Name} or {typeof(IAsyncBackgroundJob<>).Name}. " +
                                       "The job type was: " + context.JobType);
            }

            try
            {
                if (jobExecuteMethod.Name == nameof(IAsyncBackgroundJob<object>.ExecuteAsync))
                {
                    var jobExecuteMethodResult = jobExecuteMethod.Invoke(job, new[] { context.JobArgs });
                    if (jobExecuteMethodResult != null)
                        await (Task)jobExecuteMethodResult;
                    else
                        await Task.CompletedTask;
                }
                else
                {
                    jobExecuteMethod.Invoke(job, new[] { context.JobArgs });
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex.Message);


                throw new Exception($"A background job execution is failed. See inner exception for details.[JobType:{context.JobType.AssemblyQualifiedName},JobArgs:{context.JobArgs}]", ex);
            }
        }
    }
}
