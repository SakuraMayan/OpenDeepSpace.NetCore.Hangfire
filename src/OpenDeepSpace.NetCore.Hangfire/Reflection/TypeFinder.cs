using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire.Reflection
{
    /// <summary>
    /// 类型查找者
    /// </summary>
    public static class TypeFinder
    {
        //所有类型
        private static string AllTypes = $"{nameof(AllTypes)}";
        //排除系统和Nuget的所有类型
        private static string ExcludeSystemNugetAllTypes = $"{nameof(ExcludeSystemNugetAllTypes)}";
        //系统和Nuget的所有类型
        private static string SystemNugetAllTypes = $"{nameof(SystemNugetAllTypes)}";

        //缓存
        private static ConcurrentDictionary<string, List<Type>> typesCache = new ConcurrentDictionary<string, List<Type>>();

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetAllTypes()
        {
            if (typesCache.ContainsKey(AllTypes))
                return typesCache[AllTypes];

            var allAssemblies = AssemblyFinder.GetAllAssemblies();
            
            typesCache[AllTypes] = CollectionType(allAssemblies);

            return typesCache[AllTypes];
        }

        /// <summary>
        /// 获取排除了系统和Nuget的所有类型
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetExcludeSystemNugetAllTypes()
        {
            if (typesCache.ContainsKey(ExcludeSystemNugetAllTypes))
                return typesCache[ExcludeSystemNugetAllTypes];

            var allAssemblies = AssemblyFinder.GetExcludeSystemNugetAllAssemblies();

            typesCache[ExcludeSystemNugetAllTypes] = CollectionType(allAssemblies);

            return typesCache[ExcludeSystemNugetAllTypes];
        }

        /// <summary>
        /// 获取系统和Nuget所有类型
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetSystemNugetAllTypes()
        {
            if (typesCache.ContainsKey(SystemNugetAllTypes))
                return typesCache[SystemNugetAllTypes];

            var allAssemblies = AssemblyFinder.GetSystemNugetAllAssemblies();

            typesCache[SystemNugetAllTypes] = CollectionType(allAssemblies);

            return typesCache[SystemNugetAllTypes];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        private static List<Type> CollectionType(List<Assembly> assemblies)
        {
            List<Type> list = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var typeinfos = assembly.DefinedTypes;
                foreach (var typeinfo in typeinfos)
                {
                    list.Add(typeinfo.AsType());
                }
            }

            return list;
        }
    }
}
