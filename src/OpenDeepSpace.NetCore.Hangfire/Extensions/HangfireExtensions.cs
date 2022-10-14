using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenDeepSpace.NetCore.Hangfire.Attributes;
using OpenDeepSpace.NetCore.Hangfire.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class HangfireExtensions
    {

		/// <summary>
		/// Builds <see cref="RecurringJob"/> automatically within specified interface or class.
		/// To the Hangfire client, alternatively way is to use the class <seealso cref="CronJob"/> to add or update <see cref="RecurringJob"/>.
		/// </summary>
		/// <param name="configuration"><see cref="IGlobalConfiguration"/></param>
		/// <returns><see cref="IGlobalConfiguration"/></returns>
		public static IGlobalConfiguration RegisterRecurringJobs(this IGlobalConfiguration configuration)
		{

			//Get Include RecurringJobAttribute Types 扫描周期性Job
			var types = TypeFinder.GetExcludeSystemNugetAllTypes().Where(t => t.GetMethods().Any(f => f.GetCustomAttributes<RecurringJobAttribute>().Any()));

			return RegisterRecurringJobs(configuration, () => types);
		}

		/// <summary>
		/// Builds <see cref="RecurringJob"/> automatically within specified interface or class.
		/// To the Hangfire client, alternatively way is to use the class <seealso cref="CronJob"/> to add or update <see cref="RecurringJob"/>.
		/// </summary>
		/// <param name="configuration"><see cref="IGlobalConfiguration"/></param>
		/// <param name="assemblies"></param>
		/// <returns><see cref="IGlobalConfiguration"/></returns>
		public static IGlobalConfiguration RegisterRecurringJobs(this IGlobalConfiguration configuration,List<Assembly> assemblies)
		{
			if(assemblies==null)
				throw new ArgumentNullException(nameof(assemblies));

			//Get Include RecurringJobAttribute Types 扫描周期性Job
			var types = assemblies.SelectMany(t=>t.GetTypes()).Where(t => t.GetMethods().Any(f => f.GetCustomAttributes<RecurringJobAttribute>().Any()));

			return RegisterRecurringJobs(configuration, () => types);
		}

		/// <summary>
		/// Builds <see cref="RecurringJob"/> automatically within specified interface or class.
		/// To the Hangfire client, alternatively way is to use the class <seealso cref="CronJob"/> to add or update <see cref="RecurringJob"/>.
		/// </summary>
		/// <param name="configuration"><see cref="IGlobalConfiguration"/></param>
		/// <param name="types"></param>
		/// <returns><see cref="IGlobalConfiguration"/></returns>
		public static IGlobalConfiguration RegisterRecurringJobs(this IGlobalConfiguration configuration,List<Type> types)
		{
			if (types == null)
				throw new ArgumentNullException(nameof(types));

			//Get Include RecurringJobAttribute Types 扫描周期性Job
			types = types.Where(t => t.GetMethods().Any(f => f.GetCustomAttributes<RecurringJobAttribute>().Any())).ToList();

			return RegisterRecurringJobs(configuration, () => types);
		}

		/// <summary>
		/// Builds <see cref="RecurringJob"/> automatically within specified interface or class.
		/// To the Hangfire client, alternatively way is to use the class <seealso cref="CronJob"/> to add or update <see cref="RecurringJob"/>.
		/// </summary>
		/// <param name="configuration"><see cref="IGlobalConfiguration"/></param>
		/// <param name="typesProvider">The provider to get specified interfaces or class.</param>
		/// <returns><see cref="IGlobalConfiguration"/></returns>
		private static IGlobalConfiguration RegisterRecurringJobs(this IGlobalConfiguration configuration, Func<IEnumerable<Type>> typesProvider)
		{
			if (typesProvider == null) throw new ArgumentNullException(nameof(typesProvider));

			CronJob.AddOrUpdate(typesProvider);

			return configuration;
		}


		/// <summary>
		/// 注册参数化Job
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection RegisterParametricJobs(this IServiceCollection services)
        {

			//参数Job执行器管理器
			services.AddTransient<IBackgroundJobManager, HangfireBackgroundJobManager>();
			services.AddTransient<IBackgroundJobExecuter, BackgroundJobExecuter>();

			//扫描所有参数化Job
			var jobTypes = TypeFinder.GetExcludeSystemNugetAllTypes().Where(t => t.IsAssignableToGenericType(typeof(IBackgroundJob<>)) || t.IsAssignableToGenericType(typeof(IAsyncBackgroundJob<>))).Where(t => t.IsClass && !t.IsAbstract);


			//注册参数化Job到容器
			foreach (var jobType in jobTypes)
			{
				services.AddTransient(jobType);
			}


			services.Configure<BackgroundJobOptions>(options =>
			{

				foreach (var jobType in jobTypes)
				{
					options.AddJob(jobType);
					
				}
			});

			return services;
        }

		

    }
}
