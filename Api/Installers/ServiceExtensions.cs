using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Api.Installers
{
    public static class ServiceExtensions
    {
        public static void AddAssemblyInstallers(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(Startup).Assembly;

            var installers = assembly.ExportedTypes.Where(type => typeof(IInstaller).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}