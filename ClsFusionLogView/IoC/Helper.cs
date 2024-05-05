using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

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
        public static void CopyToClipboard(string s)
        {
            Clipboard.SetText(s);
        }
    }
}
