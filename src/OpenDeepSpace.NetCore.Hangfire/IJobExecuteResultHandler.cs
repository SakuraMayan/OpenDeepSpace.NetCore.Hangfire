using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    /// <summary>
    /// Job执行的结果处理
    /// </summary>
    public interface IJobExecuteResultHandler
    {

        /// <summary>
        /// Job执行成功的处理
        /// </summary>
        /// <param name="context"></param>
        public void Succeeded(ApplyStateContext context);

        /// <summary>
        /// Job执行失败的处理
        /// </summary>
        /// <param name="context"></param>
        public void Failed(ApplyStateContext context);

    }
}
