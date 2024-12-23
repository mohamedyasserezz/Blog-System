﻿using BlogSystem.Domain.Contract.Posts;
using BlogSystem.Shared.Abstractions;
using BlogSystem.Shared.Common.Errors;
using BlogSystem.Shared.Models.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            var response = await _postService.CreatePostAsync(postToCreateDto, cancellationToken);

            return response.IsSuccess ? Ok(response.Value) : response.ToProblem();

        }
        [HttpPost]
        public async Task<IActionResult> GetPost(int id, CancellationToken cancellationToken)
        {
            var response = await _postService.GetAsync(id, cancellationToken);

            return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id,
            [FromBody] PostRequest Request,
            CancellationToken cancellationToken)
        {
            var result = await _postService.UpdateAsync(id, Request, cancellationToken);

            return result.IsSuccess
                   ? Ok()
                   : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id,
        CancellationToken cancellationToken)
        {
            var isDeleted = await _postService.DeleteAsync(id, cancellationToken);

            if (isDeleted.IsSuccess)
            {
                return NoContent();
            }
            return isDeleted.ToProblem();
        }

    }
}
