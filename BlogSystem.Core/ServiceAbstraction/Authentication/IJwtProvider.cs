using BlogSystem.Domain.Enities;

namespace BlogSystem.Core.ServiceAbstraction.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
