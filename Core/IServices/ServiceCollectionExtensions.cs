using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册程序集下实现依赖注入接口的类型
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServicesProvider(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes().Where(item => item.IsClass && item.IsAbstract == false).ToArray();
            var scopeds = types.Where(item => item.IsInheritFrom<IScopeServices>());
            var transients = types.Where(item => item.IsInheritFrom<ITransientServices>());
            var singletons = types.Where(item => item.IsInheritFrom<ISingletonServices>());

            foreach (var item in scopeds)
            {
                services.AddScoped(item);
            }

            foreach (var item in transients)
            {
                services.AddTransient(item);
            }

            foreach (var item in singletons)
            {
                services.AddSingleton(item);
            }

            return services;
        }

        /// <summary>
        /// 是否可以从TBase类型派生
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static bool IsInheritFrom<TBase>(this Type type)
        {
            return typeof(TBase).IsAssignableFrom(type);
        }
    }
}
