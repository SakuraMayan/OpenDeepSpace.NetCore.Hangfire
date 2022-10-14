using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
	/// <summary>
	/// Build <see cref="RecurringJob"/> automatically.
	/// </summary>
	public interface IRecurringJobBuilder
	{
		/// <summary>
		/// Create <see cref="RecurringJob"/> with the provider for specified interface or class.
		/// </summary>
		/// <param name="typesProvider">Specified interface or class</param>
		void Build(Func<IEnumerable<Type>> typesProvider);
		/// <summary>
		/// Create <see cref="RecurringJob"/> with the provider for specified list <see cref="RecurringJobInfo"/>.
		/// </summary>
		/// <param name="recurringJobInfoProvider">The provider to get <see cref="RecurringJobInfo"/> list/></param>
		void Build(Func<IEnumerable<RecurringJobInfo>> recurringJobInfoProvider);
	}
}
