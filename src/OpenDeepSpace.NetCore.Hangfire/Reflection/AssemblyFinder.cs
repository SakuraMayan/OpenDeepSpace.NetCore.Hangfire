using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire.Reflection
{
    /// <summary>
    /// 程序集查找者
    /// </summary>
    public static class AssemblyFinder
    {
        //所有程序集
        private static string AllAssemblies = $"{nameof(AllAssemblies)}";
        //排除系统和Nuget的程序集
        private static string ExcludeSystemNugetAllAssemblies = $"{nameof(ExcludeSystemNugetAllAssemblies)}";
        //系统和Nuget的程序集
        private static string SystemNugetAllAssmeblies = $"{nameof(SystemNugetAllAssmeblies)}";

        //缓存
        private static ConcurrentDictionary<string, List<Assembly>> assembliesCache = new ConcurrentDictionary<string, List<Assembly>>();

        /// <summary>
        /// 获取项目所有程序集
        /// </summary>
        /// <returns></returns>
        public static List<Assembly> GetAllAssemblies()
        {

            if (assembliesCache.ContainsKey(AllAssemblies))
                return assembliesCache[AllAssemblies];            
            var libs = GetAllCompilationLibraries();
            assembliesCache[AllAssemblies] = CollectionAssembly(libs);

            return assembliesCache[AllAssemblies];
        }

        


        /// <summary>
        /// 获取项目程序集，排除所有的系统程序集(Microsoft.***、System.***等)、Nuget下载包
        /// </summary>
        /// <returns></returns>
        public static List<Assembly> GetExcludeSystemNugetAllAssemblies()
        {
            if (assembliesCache.ContainsKey(ExcludeSystemNugetAllAssemblies))
                return assembliesCache[ExcludeSystemNugetAllAssemblies];

            //排除所有的系统程序集、Nuget下载包
            var libs = GetAllCompilationLibraries().Where(lib => !lib.Serviceable && lib.Type != "package").ToList();
            
            assembliesCache[ExcludeSystemNugetAllAssemblies] = CollectionAssembly(libs);

            return assembliesCache[ExcludeSystemNugetAllAssemblies];
        }

        /// <summary>
        /// 获取项目依赖的系统和Nuget下载的程序集
        /// </summary>
        /// <returns></returns>
        public static List<Assembly> GetSystemNugetAllAssemblies()
        {
            if(assembliesCache.ContainsKey(SystemNugetAllAssmeblies))
                return assembliesCache[SystemNugetAllAssmeblies];

            //所有的系统程序集、Nuget下载包
            var libs = GetAllCompilationLibraries().Where(lib => lib.Serviceable || lib.Type == "package").ToList();
            
            assembliesCache[SystemNugetAllAssmeblies] = CollectionAssembly(libs);

            return assembliesCache[SystemNugetAllAssmeblies];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static IReadOnlyList<CompilationLibrary> GetAllCompilationLibraries()
        {
            var deps = DependencyContext.Default;
            var libs = deps.CompileLibraries;

            return libs;
        }

        /// <summary>
        /// 收集程序集
        /// </summary>
        /// <param name="libs"></param>
        private static List<Assembly> CollectionAssembly(IReadOnlyList<CompilationLibrary> libs)
        {
            List<Assembly> list = new List<Assembly>();
            foreach (var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(assembly);
                }
                catch
                {
                    //
                }
            }

            return list;
        }

    }
}
