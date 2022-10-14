using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire.Extensions
{
    /// <summary>
    /// 反射拓展
    /// </summary>
    public static class ReflectionExtensions
    {
		public static string? GetRecurringJobId(this MethodInfo method)
		{
			return $"{method.DeclaringType?.ToGenericTypeString()}.{method.Name}";
		}
		/// <summary>
		/// Fork the extension method from 
		/// https://github.com/HangfireIO/Hangfire/blob/master/src/Hangfire.Core/Common/TypeExtensions.cs
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string? ToGenericTypeString(this Type type)
		{
			if (!type.GetTypeInfo().IsGenericType)
			{
				return type.GetFullNameWithoutNamespace()
						?.ReplacePlusWithDotInNestedTypeName();
			}

			return type.GetGenericTypeDefinition()
					?.GetFullNameWithoutNamespace()
					?.ReplacePlusWithDotInNestedTypeName()
					.ReplaceGenericParametersInGenericTypeName(type);
		}

		private static string? GetFullNameWithoutNamespace(this Type type)
		{
			if (type.IsGenericParameter)
			{
				return type.Name;
			}

			const int dotLength = 1;
			// ReSharper disable once PossibleNullReferenceException
			return !String.IsNullOrEmpty(type.Namespace)
				? type.FullName?[(type.Namespace.Length + dotLength)..]
				: type.FullName;
		}

		private static string ReplacePlusWithDotInNestedTypeName(this string typeName)
		{
			return typeName.Replace('+', '.');
		}

		private static string ReplaceGenericParametersInGenericTypeName(this string typeName, Type type)
		{
			var genericArguments = type.GetTypeInfo().GetAllGenericArguments();

			const string regexForGenericArguments = @"`[1-9]\d*";

			var rgx = new Regex(regexForGenericArguments);

			typeName = rgx.Replace(typeName, match =>
			{
				var currentGenericArgumentNumbers = int.Parse(match.Value[1..]);
				var currentArguments = string.Join(",", genericArguments.Take(currentGenericArgumentNumbers).Select(ToGenericTypeString));
				genericArguments = genericArguments.Skip(currentGenericArgumentNumbers).ToArray();
				return string.Concat("<", currentArguments, ">");
			});

			return typeName;
		}
		public static Type[] GetAllGenericArguments(this TypeInfo type)
		{
			return type.GenericTypeArguments.Length > 0 ? type.GenericTypeArguments : type.GenericTypeParameters;
		}

		/// <summary>
		/// Checks whether <paramref name="givenType"/> implements/inherits <paramref name="genericType"/>.
		/// </summary>
		/// <param name="givenType">Type to check</param>
		/// <param name="genericType">Generic type</param>
		public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
		{
			var givenTypeInfo = givenType.GetTypeInfo();

			if (givenTypeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
			{
				return true;
			}

			foreach (var interfaceType in givenTypeInfo.GetInterfaces())
			{
				if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
				{
					return true;
				}
			}

			if (givenTypeInfo.BaseType == null)
			{
				return false;
			}

			return IsAssignableToGenericType(givenTypeInfo.BaseType, genericType);
		}
	}
}
