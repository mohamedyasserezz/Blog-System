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
using BlogSystem.Shared.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
		public async Task<AuthResponse> Login(LoginAuthRequest request, CancellationToken cancellationToken = default)
		{
			var user = await _userManger.FindByEmailAsync(request.Email);
			if(user is null)
				return null;

			var validUserPassword = await _userManger.CheckPasswordAsync(user, request.Password);

			if (!validUserPassword)
				return null;

			var userRole = await _userManger.GetRolesAsync(user);
			var (token,expireIn) = _jwtProvider.GenerateToken(user, userRole);

			var refreshToken = GenerateRefreshToken();
			var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

			user.RefreshTokens.Add(new RefreshToken
			{
				Token = refreshToken,
				ExpiresOn = refreshTokenExpiration
			});
			await _userManger.UpdateAsync(user);
			return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expireIn, refreshToken, refreshTokenExpiration);
		}
		public async Task<bool> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
		{
			var emailIsExist = await _userManger.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
			if (emailIsExist)
				return false;
			var user = _mapper.Map<ApplicationUser>(request);
			user.UserName = request.Email;

			var result = await _userManger.CreateAsync(user, request.Password);
			if (result.Succeeded)
			{
				var code = await _userManger.GenerateEmailConfirmationTokenAsync(user);
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

				return true;
			}
			return false;

		}
		private static string GenerateRefreshToken()
		{
			return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
		}

	}
}
