using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using training_api.Services;

namespace training_api.Installers
{
    public class PostInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPostService, PostService>();
        }
    }
}
