using Microsoft.Extensions.DependencyInjection;
using System;

namespace ClsFusionViewer.IoC
{
    public class Helper
    {
        public static T GetScopedService<T>(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                return scope.ServiceProvider.GetService<T>();
            }
        }
    }
}
