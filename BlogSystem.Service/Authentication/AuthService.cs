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

namespace BlogSystem.Service.Authentication
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManger;
		private readonly IJwtProvider _jwtProvider;
		private readonly int _refreshTokenExpiryDays = 14;
		private readonly IMapper _mapper;
		public AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider, IMapper mapper)
		{
			_userManger = userManager;
			_jwtProvider = jwtProvider;
			_mapper = mapper;
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
				var code = await _userManger.GenerateEmailConfirmationTokenAsync(user);
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

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
		private static string GenerateRefreshToken()
		{
			return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
		}

	}
}
