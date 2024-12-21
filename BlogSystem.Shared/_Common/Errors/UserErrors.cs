using BlogSystem.Shared.Abstractions;

namespace BlogSystem.Shared.Common.Errors
{
    public static class UserErrors
    {
        public static Error InvalidCardentials =
            new("User.InvalidCredentials", "Invalid Email/Password");
    }


}
