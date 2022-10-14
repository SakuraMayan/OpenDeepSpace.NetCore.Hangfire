using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpenDeepSpace.NetCore.Hangfire.Demo.Controllers
{
    /// <summary>
    /// Hangfire控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {

        private readonly IBackgroundJobManager backgroundJobManager;

        public HangfireController(IBackgroundJobManager backgroundJobManager)
        {
            this.backgroundJobManager = backgroundJobManager;
        }

        /// <summary>
        /// Hangfire测试
        /// </summary>
        [HttpGet]
        public async Task TestHangfire()
        {
            //触发一次
            await backgroundJobManager.EnqueueAsync(new JobOneArgs()
            {

            });

            string job = BackgroundJob.Enqueue(() => Console.WriteLine("haha"));
            BackgroundJob.ContinueJobWith(job, () => Console.WriteLine("hahs"));

            string jobOne = await backgroundJobManager.EnqueueAsync(new JobOneArgs());

            await backgroundJobManager.ContinueJobWith(jobOne, new JobOneArgs());

            await backgroundJobManager.EnqueueAsync(new JobOneArgs(), TimeSpan.FromSeconds(30));

            await backgroundJobManager.EnqueueAsync(new JobOneArgs(), "JobOne周期", "0 */1 * * * ?");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task TestUploadFile(IFormFile formFile)
        {
            var fileStream = formFile.OpenReadStream();
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);

            //byte数据可以支持
            await backgroundJobManager.EnqueueAsync(new FileJobOneArgs()
            {
                data = bytes
            });
        }


    }
}
