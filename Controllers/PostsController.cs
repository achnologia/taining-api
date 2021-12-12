using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using training_api.Contacts.Requests;
using training_api.Domain;
using training_api.Services;

namespace training_api.Controllers
{
    [Route("api/posts")]
    public class PostsController : Controller
    {
        private readonly IPostService _service;

        public PostsController(IPostService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = _service.GetAllAsync();

            return Ok(posts);
        }

        [HttpGet("{idPost}")]
        public async Task<IActionResult> GetPostById([FromRoute] string idPost)
        {
            var post = _service.GetByIdAsync(Guid.Parse(idPost));

            if(post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            if(string.IsNullOrEmpty(request.Id))
            {
                request.Id = Guid.NewGuid().ToString();
            }

            var newPost = new Post
            {
                Id = Guid.Parse(request.Id),
                Name = request.Name
            };

            _service.CreateAsync(newPost);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = $"{baseUrl}/api/posts/{request.Id}";

            return Created(locationUrl, request);
        }

        [HttpPut("{idPost}")]
        public async Task<IActionResult> UpdatePost([FromRoute] string idPost, [FromBody] UpdatePostRequest request)
        {
            var postToUpdate = new Post
            {
                Id = Guid.Parse(idPost),
                Name = request.Name
            };

            var updated = _service.UpdateAsync(postToUpdate);

            if(!updated)
                return NotFound();

            return Ok(postToUpdate);
        }

        [HttpDelete("{idPost}")]
        public async Task<IActionResult> DeletePost([FromRoute] string idPost)
        {
            var deleted = _service.DeleteAsync(Guid.Parse(idPost));

            if(!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
