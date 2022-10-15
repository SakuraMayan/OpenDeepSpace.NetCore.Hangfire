using Hangfire.States;

namespace OpenDeepSpace.NetCore.Hangfire.Demo.Jobs
{
    /// <summary>
    /// Job执行结果处理
    /// </summary>
    public class JobExecuteResultHandler : IJobExecuteResultHandler
    {
        public void Failed(ApplyStateContext context)
        {
            //失败 自己实现处理
        }

        public void Succeeded(ApplyStateContext context)
        {
            Console.WriteLine($"Job执行成功:{context.BackgroundJob.Job.Method}");
        }
    }
}
