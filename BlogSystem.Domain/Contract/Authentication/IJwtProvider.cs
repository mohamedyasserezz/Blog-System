using BlogSystem.Domain.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Domain.Contract.Authentication
{
	public interface IJwtProvider
	{
		(string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles);
	}
}
