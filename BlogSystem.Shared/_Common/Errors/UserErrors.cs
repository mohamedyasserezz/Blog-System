using BlogSystem.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace BlogSystem.Shared.Common.Errors
{
    public static class UserErrors
    {
		public static readonly Error InvalidCredentails = new("User.InvalidCredentials", "Invalid Email Or Password", StatusCodes.Status401Unauthorized);

		public static readonly Error InvalidJwtToken = new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);

		public static readonly Error InvalidRefreshToken = new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

		public static readonly Error DuplicatedEmail = new("User.DuplicatedEmail", "Another user with the same email is already exists", StatusCodes.Status409Conflict);

		public static readonly Error UserNotFound = new("User.UserNotFound", "User is not found", StatusCodes.Status404NotFound);
	}


}
