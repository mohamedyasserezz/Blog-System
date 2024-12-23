using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Shared.Models.Authentication
{
	public class ResetPasswordRequestValidation : AbstractValidator<ResetPasswordRequest>
	{
		public ResetPasswordRequestValidation()
		{
			RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress();

			RuleFor(x => x.Code)
				.NotEmpty();

			RuleFor(x => x.NewPassword)
				.NotEmpty()
				.WithMessage("Passwrod should be at least 8 digits and should contains lower case, nonalphanumeric and uppercase");

		}
	}
}
