using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using training_api.Services;

namespace training_api.Installers
{
    public class PostInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Singleton because there is a list of Post in the service for testing purposes
            services.AddSingleton<IPostService, PostService>();
        }
    }
}
