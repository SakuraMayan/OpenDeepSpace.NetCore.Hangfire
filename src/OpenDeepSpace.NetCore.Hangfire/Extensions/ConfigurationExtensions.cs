using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire.Extensions
{
    /// <summary>
    /// IConfiguation拓展
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// 是否存在Key
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static bool IsExistKey(this IConfiguration configuration,string Key)
        {
            return configuration.AsEnumerable().Any(t => t.Key == Key);
        }
    }
}
