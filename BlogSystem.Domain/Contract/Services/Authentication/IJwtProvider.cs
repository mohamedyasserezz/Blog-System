using BlogSystem.Domain.Enities;

namespace BlogSystem.Domain.Contract.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles);
		string? ValidateToken(string token);
	}
}
