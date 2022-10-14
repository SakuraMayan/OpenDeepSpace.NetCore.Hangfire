using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire.Attributes
{
    /// <summary>
    /// JobDisplayName适配
    /// </summary>
    public class JobDisplayNameAdapterAttribute:JobDisplayNameAttribute
    {

        /// <summary>
        /// Job名称显示特性
        /// </summary>
        public JobDisplayNameAdapterAttribute(string displayName) : base(displayName)
        {
        }

        /// <summary>
        /// Job Name Format
        /// </summary>
        /// <param name="context"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        public override string Format(DashboardContext context, Job job)
        {
            if (job.Args == null || job.Args.Count == 0) return base.Format(context, job);

            var arg = job.Args[0];

            var jobName = BackgroundJobNameAttribute.GetName(arg.GetType());
            if (string.IsNullOrEmpty(jobName)) return job.ToString();
            var matches = new Regex(@"\{(?<value>.*?)\}", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.ExplicitCapture).Matches(jobName);
            foreach (Match mc in matches)
            {
                jobName = jobName.Replace(mc.Value, GetPropertyValue(arg, mc.Groups["value"].ToString())?.ToString());
            }

            return jobName;
        }

        /// <summary>
        /// 获取一个类指定的属性值
        /// </summary>
        /// <param name="info">object对象</param>
        /// <param name="field">属性名称</param>
        /// <returns></returns>
        public static object? GetPropertyValue(object info, string field)
        {
            if (info == null) return null;
            Type t = info.GetType();
            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            if (!property.Any()) return field;
            return property.First().GetValue(info, null);
        }
    }
}
