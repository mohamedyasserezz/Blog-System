using BlogSystem.Domain.Contract.Posts;
using BlogSystem.Shared.Models.Posts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;

        [HttpPost("create-post")]
        public async Task<IActionResult> CreatePost(PostToCreateDto postToCreateDto, CancellationToken cancellationToken)
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var response = await _postService.CreatePostAsync(userEmail, postToCreateDto, cancellationToken);

            return response is null ? BadRequest() : Ok(response);
        }

    }
}
