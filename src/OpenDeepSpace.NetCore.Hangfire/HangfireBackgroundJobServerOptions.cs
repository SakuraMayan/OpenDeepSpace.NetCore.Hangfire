using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    /// <summary>
    /// Hangfire后台JobServer选项
    /// </summary>
    public class HangfireBackgroundJobServerOptions
    {
        /// <summary>
        /// 由于Hangfire是一个Storage,Client,Server结构的程序，该字段配置一个进程里面多个Server实例的服务名称
        /// </summary>
        public string? ServerName { get; set; }

        /// <summary>
        /// 初始化工作线程数,可以最多同时执行任务个数
        /// </summary>
        public int WorkerCount { get; set; }

        /// <summary>
        /// Hangfire可以设置工作队列，如果想设置工作的优先级或者根据服务拆分任务，可以使用Queues
        /// </summary>
        public string[] Queues { get; set; }=Array.Empty<string>();

        /// <summary>
        /// 设置该参数之后，如果遇到了服务器的关机事件，Hangfire继续在设置的时间内执行任务，而不是立刻中断
        /// </summary>
        public int StopTimeout { get; set; }

        /// <summary>
        /// 配置心跳检查进程的取消的超时时间
        /// </summary>
        public int ShutdownTimeout { get; set; }

        /// <summary>
        /// 任务调度轮询间隔，Hangfire使用轮询的机制对队列中的Job进行处理，如果队列里面没有Job，Hangfire会默认等待15秒，然后再去队列取数
        /// </summary>
        public int SchedulePollingInterval { get; set; }

        public int HeartbeatInterval { get; set; }

        public int ServerCheckInterval { get; set; }

        public int ServerTimeout { get; set; }

        public int CancellationCheckInterval { get; set; }

    }
}
