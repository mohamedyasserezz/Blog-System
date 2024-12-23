using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlogSystem.Domain.Contract.Authentication;
using BlogSystem.Domain.Contract.Services.Authentication;
using BlogSystem.Domain.Enities;
using BlogSystem.Shared.Abstractions;
using BlogSystem.Shared.Common.Errors;
using BlogSystem.Shared.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using BlogSystem.Infrastructure.Data.Seed.Authentication;
using BlogSystem.Shared.Helpers;
using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BlogSystem.Service.Authentication
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManger;
		private readonly IJwtProvider _jwtProvider;
		private readonly int _refreshTokenExpiryDays = 14;
		private readonly IMapper _mapper;
		private readonly IEmailSender _emailSender;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender)
		{
			_userManger = userManager;
			_jwtProvider = jwtProvider;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_emailSender = emailSender;
		}
		public async Task<Result<AuthResponse>> Login(LoginAuthRequest request, CancellationToken cancellationToken = default)
		{
			var user = await _userManger.FindByEmailAsync(request.Email);
			if (user is null)
				return Result.Failer<AuthResponse>(UserErrors.InvalidCredentails);

			var validUserPassword = await _userManger.CheckPasswordAsync(user, request.Password);

			if (!validUserPassword)
				return Result.Failer<AuthResponse>(UserErrors.InvalidCredentails);

			var userRole = await _userManger.GetRolesAsync(user);
			var (token, expireIn) = _jwtProvider.GenerateToken(user, userRole);

			var refreshToken = GenerateRefreshToken();
			var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

			user.RefreshTokens.Add(new RefreshToken
			{
				Token = refreshToken,
				ExpiresOn = refreshTokenExpiration
			});
			await _userManger.UpdateAsync(user);
			var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expireIn, refreshToken, refreshTokenExpiration);
			return Result.Success(response);
		}
		public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
		{
			var emailIsExist = await _userManger.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
			if (emailIsExist)
				return Result.Failer<AuthResponse>(UserErrors.InvalidJwtToken);

			var user = _mapper.Map<ApplicationUser>(request);
			user.UserName = request.Email;
			user.EmailConfirmed = true;

			var result = await _userManger.CreateAsync(user, request.Password);
			if (result.Succeeded)
			{
				await _userManger.AddToRoleAsync(user, DefaultRoles.Reader);
				return Result.Success();
			}
			var error = result.Errors.First();
			return Result.Failer(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

		}
		public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
		{
			var userId = _jwtProvider.ValidateToken(token);
			if (userId is null)
				return Result.Failer<AuthResponse>(UserErrors.InvalidJwtToken);

			var user = await _userManger.FindByIdAsync(userId);

			if (user is null)
				return Result.Failer<AuthResponse>(UserErrors.InvalidJwtToken);

			var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

			if (userRefreshToken is null)
				return Result.Failer<AuthResponse>(UserErrors.InvalidJwtToken);

			userRefreshToken.RevokedOn = DateTime.UtcNow;
			var userRole = await _userManger.GetRolesAsync(user);
			var (newToken, expiresIn) = _jwtProvider.GenerateToken(user, userRole);

			var newRefreshToken = GenerateRefreshToken();
			var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

			user.RefreshTokens.Add(new RefreshToken
			{
				Token = newRefreshToken,
				ExpiresOn = refreshTokenExpiration
			});
			await _userManger.UpdateAsync(user);
			var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);
			return Result.Success(response);
		}

		public async Task<Result> SendResetPasswordCodeAsync(string email)
		{

			var user = await _userManger.FindByEmailAsync(email);
			if (user is null)
				return Result.Success();

			if (!user.EmailConfirmed)
				return Result.Failer(UserErrors.EmailNotConfirmed);

			var code = await _userManger.GeneratePasswordResetTokenAsync(user);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

			await SendResetPasswordEmailAsync(user, code);

			return Result.Success();
		}

		public async Task<Result> ResetPasswordAsync(Shared.Models.Authentication.ResetPasswordRequest request)
		{
			var user = await _userManger.FindByEmailAsync(request.Email);
			if (user is null || user.EmailConfirmed)
				return Result.Failer(UserErrors.InvalidCode);

			IdentityResult result;
			try
			{
				var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
				result = await _userManger.ResetPasswordAsync(user, code, request.NewPassword);
			}
			catch (FormatException)
			{
				result = IdentityResult.Failed(_userManger.ErrorDescriber.InvalidToken());
			}

			if (result.Succeeded)
				return Result.Success();

			var error = result.Errors.First();
			return Result.Failer(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
		}
		private static string GenerateRefreshToken()
		{
			return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
		}
		private async Task SendResetPasswordEmailAsync(ApplicationUser user, string code)
		{
			var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

			var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
				new Dictionary<string, string>
				{
					{"{{name}}",user.FirstName },
					{ "{{action_url}}",$"{origin}/auth/forgetPassword?email={user.Email}&code={code}" }
				}
			);
			BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Blog System: Reset your password", emailBody));

			await Task.CompletedTask;
		}
	}
}
