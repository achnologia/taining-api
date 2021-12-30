using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Api.Services;

namespace Api.Installers
{
    public class PostInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPostService, PostService>();
        }
    }
}
