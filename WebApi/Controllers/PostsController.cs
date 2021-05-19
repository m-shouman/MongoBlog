using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> Get()
        {
            return await _postService.Get();
        }

        [HttpGet("{id}", Name = "GetPost")]
        public async Task<ActionResult<Post>> Get(string id)
        {
            var post = await _postService.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        [HttpGet("/api/[controller]/search", Name = "Search")]
        public async Task<IActionResult> GetPostsByCommentAuthor(string userId)
        {
            var posts = await _postService.GetPostsByCommentAuthor(userId);

            if (posts == null || posts.Count == 0)
            {
                return NotFound();
            }

            return Ok(posts);
        }

        [HttpGet("/api/[controller]/author/{authorId}")]
        public async Task<ActionResult<List<Post>>> GetPostsByAuthor(string authorId)
        {
            return await _postService.GetPostByAuthor(authorId);
        }

        [HttpPost]
        public async Task<ActionResult<Post>> Create(CreatePostDto post)
        {
            var createdPost = await _postService.Create(post);

            return CreatedAtRoute("GetPost", new { id = createdPost.Id.ToString() }, createdPost);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdatePostDto postIn)
        {
            var post = await _postService.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            await _postService.Update(id, postIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var post = await _postService.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            await _postService.Remove(post.Id);

            return NoContent();
        }

        [HttpGet("/api/[controller]/{id}/comments")]
        public async Task<IActionResult> GetComments(string id)
        {
            var post = await _postService.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post.Comments);
        }

        [HttpPost("/api/[controller]/{id}/comments")]
        public async Task<IActionResult> AddComment(string id, [FromBody] CreateCommentDto comment)
        {
            var post = await _postService.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            await _postService.AddComment(id, comment);

            return NoContent();
        }

        [HttpDelete("/api/[controller]/{id}/comments/{commentId}")]
        public async Task<IActionResult> RemoveComments(string id, string commentId)
        {
            await _postService.RemoveComment(id, commentId);

            return NoContent();
        }
    }
}
