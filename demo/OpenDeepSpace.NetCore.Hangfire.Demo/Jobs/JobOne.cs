using Hangfire;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDeepSpace.NetCore.Hangfire.Attributes;

namespace OpenDeepSpace.NetCore.Hangfire.Demo
{
    /// <summary>
    /// 注入到容器
    /// </summary>
    public class JobOne : AsyncBackgroundJob<JobOneArgs>
    {
        private IScopedService _service;

        public JobOne(IScopedService service)
        {
            _service = service;
        }

        [JobDisplayName("第一个Job")]
        public async override Task ExecuteAsync(JobOneArgs args)
        {
            Console.WriteLine("第一个Job额");
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// JobOneArgs
    /// </summary>
    [BackgroundJobQueue("local")]
    [BackgroundJobName("JobOne额")]
    public class JobOneArgs
    {
        public int i { get; set; }
    }
}
