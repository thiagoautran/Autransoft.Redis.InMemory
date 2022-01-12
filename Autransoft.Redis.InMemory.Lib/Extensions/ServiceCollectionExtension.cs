using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Autransoft.Redis.InMemory.Lib.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void ReplaceSingleton(this IServiceCollection serviceCollection, Type i, Type c)
        {
            var descriptor = serviceCollection.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == i);
            if(descriptor != null)
            {
                serviceCollection.Remove(descriptor);
                serviceCollection.AddSingleton(i, c);
            }
        }
   }
}