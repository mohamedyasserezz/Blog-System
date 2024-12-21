using BlogSystem.Shared.Abstractions;
using BlogSystem.Shared.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Domain.Contract.Services.Authentication
{
	public interface IAuthService
	{
		public Task<Result<AuthResponse>> Login(LoginAuthRequest request, CancellationToken cancellationToken = default);
		public Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
		public Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
	}
}
