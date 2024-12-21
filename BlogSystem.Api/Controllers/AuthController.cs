﻿using BlogSystem.Domain.Contract.Services.Authentication;
using BlogSystem.Service.Authentication;
using BlogSystem.Shared.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BlogSystem.Shared.Models.Authentication;
using BlogSystem.Shared.Abstractions;
using RegisterRequest = BlogSystem.Shared.Models.Authentication.RegisterRequest;

namespace BlogSystem.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly JwtOptions _jwtOptions;
		private readonly ILogger<AuthController> _logger;
		public AuthController(IAuthService authService, IOptions<JwtOptions> jwtOptions, ILogger<AuthController> logger)
		{
			_authService = authService;
			_jwtOptions = jwtOptions.Value;
			_logger = logger;
		}
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginAuthRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Logging with email: {email} and password:{passwrod}", request.Email, request.Password);
			var authResult = await _authService.Login(request, cancellationToken);
			return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
		}
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
		{
			var result = await _authService.RegisterAsync(request, cancellationToken);
			return result.IsSuccess ? Ok() : result.ToProblem();
		}
		[HttpPost("refresh")]
		public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
		{
			var result = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
			return result.IsSuccess
				? Ok()
				: Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error.Code, detail: result.Error.Description);
		}
	}
}
