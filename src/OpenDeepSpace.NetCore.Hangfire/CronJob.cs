using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
	/// <summary>
	/// The helper class to build <see cref="RecurringJob"/> automatically.
	/// </summary>
	public class CronJob
	{
		/// <summary>
		/// Builds <see cref="RecurringJob"/> automatically within specified interface or class.
		/// </summary>
		/// <param name="typesProvider">The provider to get specified interfaces or class.</param>
		public static void AddOrUpdate(Func<IEnumerable<Type>> typesProvider)
		{
			if (typesProvider == null) throw new ArgumentNullException(nameof(typesProvider));

			IRecurringJobBuilder builder = new RecurringJobBuilder();

			builder.Build(typesProvider);
		}
	}
}
