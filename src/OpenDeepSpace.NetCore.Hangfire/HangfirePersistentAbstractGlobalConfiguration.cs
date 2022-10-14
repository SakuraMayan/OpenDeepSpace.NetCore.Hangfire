using Hangfire;
using Microsoft.Extensions.Configuration;
using OpenDeepSpace.NetCore.Hangfire.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    /// <summary>
    /// Hangfire持久化抽象全局配置
    /// </summary>
    public abstract class HangfirePersistentAbstractGlobalConfiguration:IGlobalConfiguration
    {
        public string ConnectionString { get; set; } = string.Empty;//字符串连接

        public HangfirePersistentAbstractGlobalConfiguration(IConfiguration configuration)
        {
            if(configuration.IsExistKey("Hangfire:ConnectionString"))
                ConnectionString = configuration["Hangfire:ConnectionString"];
        }

        public abstract IGlobalConfiguration UseStorage(IGlobalConfiguration globalConfiguration);
    }
}
