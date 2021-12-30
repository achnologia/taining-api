using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Api.Contacts.Requests;
using Api.Domain;
using Api.Extensions;
using Api.Services;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _service;

        public PostsController(IPostService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _service.GetAllAsync();

            return Ok(posts);
        }

        [HttpGet("{idPost}")]
        public async Task<IActionResult> GetPostById([FromRoute] string idPost)
        {
            var post = await _service.GetByIdAsync(Guid.Parse(idPost));

            if(post is null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            var newPost = new Post
            {
                Name = request.Name,
                IdAuthor = HttpContext.GetIdUser()
            };

            var idCreated = await _service.CreateAsync(newPost);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = $"{baseUrl}/api/posts/{idCreated}";

            return Created(locationUrl, request);
        }

        [HttpPut("{idPost}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid idPost, [FromBody] UpdatePostRequest request)
        {
            var isAuthor = await _service.IsUserPostAuthor(idPost, HttpContext.GetIdUser());

            if (!isAuthor)
            {
                return BadRequest(new { error = "You are not the author of the post." });
            }

            var postToUpdate = await _service.GetByIdAsync(idPost);
            postToUpdate.Name = request.Name;

            var updated = await _service.UpdateAsync(postToUpdate);

            if(!updated)
                return NotFound();

            return Ok(postToUpdate);
        }

        [HttpDelete("{idPost}")]
        public async Task<IActionResult> DeletePost([FromRoute] Guid idPost)
        {
            var isAuthor = await _service.IsUserPostAuthor(idPost, HttpContext.GetIdUser());

            if (!isAuthor)
            {
                return BadRequest(new { error = "You are not the author of the post." });
            }

            var deleted = await _service.DeleteAsync(idPost);

            if(!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
