using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Api;
using Api.Contacts.Requests;
using Api.Contacts.Responses;
using Api.Data;

namespace Api.InregrationTests
{
    public abstract class IntegrationTestBase : IDisposable
    {
        protected readonly HttpClient _testClient;
        private readonly IServiceProvider _serviceProvider;

        protected IntegrationTestBase()
        {
            var appFactory = new WebApplicationFactory<Api.Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<DataContext>));
                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });

            _serviceProvider = appFactory.Services;
            _testClient = appFactory.CreateClient();
        }

        public void Dispose()
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var dataContext = serviceScope.ServiceProvider.GetService<DataContext>();

            dataContext.Database.EnsureDeleted();
        }

        protected async Task AuthenticateAsync()
        {
            var jwt = await GetJwtAsync();

            _testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwt);
        }

        private async Task<string> GetJwtAsync()
        {
            var request = new UserRegistrationRequest
            {
                Email = "test@email.com",
                Password = "Password1234!"
            };

            var response = await _testClient.PostAsJsonAsync("api/identity/registration", request);

            var registrationResponse = await response.Content.ReadAsAsync<AuthenticationSuccessResponse>();

            return registrationResponse.Token;
        }
    }
}
