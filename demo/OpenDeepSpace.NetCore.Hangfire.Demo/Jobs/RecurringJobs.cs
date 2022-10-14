using Hangfire;
using OpenDeepSpace.NetCore.Hangfire.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire.Demo
{
    /// <summary>
    /// 周期性Jobs
    /// </summary>
    public class RecurringJobs
    {
        [JobDisplayName("周期性JobOne")]
        [RecurringJob("0 */1 * * * ?")]
        [Queue("recurringjobqueue")]//这个队列要添加到Hangfire的Queues中 不然Job只进入队列无法执行
        public void TestJob1()
        {

        }
        [RecurringJob("0 */1 * * * ?", RecurringJobId = "testjob2")]
        [Queue("recurringjobqueue")]
        public void TestJob2()
        {

        }
        [RecurringJob("0 */1 * * * ?", "China Standard Time", "recurringjobqueue")]
        public void TestJob3()
        {

        }
        [RecurringJob("0 */1 * * * ?", "recurringjobqueue")]
        public void InstanceTestJob()
        {

        }

        [RecurringJob("0 */1 * * * ?", "UTC", "recurringjobqueue")]
        public static void StaticTestJob()
        {

        }
    }
}
