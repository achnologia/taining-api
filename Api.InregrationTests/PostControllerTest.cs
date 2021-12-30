using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Domain;
using Xunit;

namespace Api.InregrationTests
{
    public class PostControllerTest : IntegrationTestBase
    {
        [Fact]
        public async Task GetPosts_WithoutAnyPost_ReturnsEmptyResponse()
        {
            await AuthenticateAsync();

            var response = await _testClient.GetAsync("api/posts");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Empty(await response.Content.ReadAsAsync<List<Post>>());
        }
    }
}
