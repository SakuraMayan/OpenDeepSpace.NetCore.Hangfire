using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BackgroundJobNameAttribute:Attribute
    {
        public string Name { get; }

        public BackgroundJobNameAttribute(string name)
        {
            Name = name??nameof(name);
        }

        public static string? GetName<TJobArgs>()
        {
            return GetName(typeof(TJobArgs));
        }

        public static string? GetName(Type jobArgsType)
        {
           
            return jobArgsType.GetCustomAttribute<BackgroundJobNameAttribute>()?.Name
                   ?? jobArgsType.FullName;
        }
    }
}
