using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Shared.Models.Authentication
{
	public class AuthResponse
	(
		string Id,
		string? Email,
		string? FirstName,
		string? LastName,
		string Token,
		int ExpiresIn,
		string RefreshToken,
		DateTime RefreshTokenExpiration
	);
}
