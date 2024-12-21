using BlogSystem.Domain.Contract.Posts;
using BlogSystem.Shared.Models.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;

        [HttpPost("create-post")]
        public async Task<IActionResult> CreatePost(PostRequest postToCreateDto, CancellationToken cancellationToken)
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var response = await _postService.CreatePostAsync(postToCreateDto, cancellationToken);

            return response.Value is null ? BadRequest(response.Error) : Ok(response.Value);

        }

    }
}
