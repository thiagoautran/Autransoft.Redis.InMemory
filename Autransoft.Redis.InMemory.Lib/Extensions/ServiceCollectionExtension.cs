using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Autransoft.Redis.InMemory.Lib.Extensions
{
    internal static class ServiceCollectionExtension
    {
        public static void ReplaceSingleton<INTERFACE, CLASS>(this IServiceCollection serviceCollection) 
            where CLASS : class, INTERFACE
            where INTERFACE : class 
        {
            var descriptor = serviceCollection.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(INTERFACE));

            if(descriptor != null)
            {
                serviceCollection.Remove(descriptor);
                serviceCollection.AddSingleton<INTERFACE, CLASS>();
            }
        }

        public static void ReplaceTransient<INTERFACE, CLASS>(this IServiceCollection serviceCollection) 
            where CLASS : class, INTERFACE
            where INTERFACE : class 
        {
            var descriptor = serviceCollection.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(INTERFACE));

            if(descriptor != null)
            {
                serviceCollection.Remove(descriptor);
                serviceCollection.AddTransient<INTERFACE, CLASS>();
            }
        }

        public static void ReplaceScoped<INTERFACE, CLASS>(this IServiceCollection serviceCollection) 
            where CLASS : class, INTERFACE
            where INTERFACE : class 
        {
            var descriptor = serviceCollection.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(INTERFACE));

            if(descriptor != null)
            {
                serviceCollection.Remove(descriptor);
                serviceCollection.AddScoped<INTERFACE, CLASS>();
            }
        }
   }
}