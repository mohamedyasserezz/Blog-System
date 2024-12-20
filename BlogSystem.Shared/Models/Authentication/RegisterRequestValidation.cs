using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Shared.Models.Authentication
{
	public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
	{
		public RegisterRequestValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress();

			RuleFor(x => x.Password)
				.NotEmpty()
				.WithMessage("Passwrod should be at least 8 digits and should contains lower case, nonalphanumeric and uppercase");


			RuleFor(x => x.FirstName)
				.NotEmpty()
				.Length(3, 100);

			RuleFor(x => x.LastName)
				.NotEmpty()
				.Length(3, 100);
		}
	}
}
