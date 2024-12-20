﻿using BlogSystem.Domain.Contract.Authentication;
using BlogSystem.Domain.Enities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace BlogSystem.Service.Authentication
{
    public class JwtProvider : IJwtProvider
	{
		private readonly JwtOptions _jwtOptions;
		public JwtProvider(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
		}
        public (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles)
		{
			Claim[] claims =
			{
				new(JwtRegisteredClaimNames.Sub,user.Id),
				new(JwtRegisteredClaimNames.Email,user.Email!),
				new(JwtRegisteredClaimNames.GivenName,user.FirstName),
				new(JwtRegisteredClaimNames.FamilyName,user.LastName),
				new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
				new(nameof(roles),JsonSerializer.Serialize(roles),JsonClaimValueTypes.JsonArray)
			};
			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
			var singingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				issuer: _jwtOptions.Issuer,
				audience: _jwtOptions.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireTimeInMinutes),
				signingCredentials: singingCredentials
				);
			return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: _jwtOptions.ExpireTimeInMinutes * 60);
		}
	}
}
