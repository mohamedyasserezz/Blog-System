using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Shared.Models.Authentication
{
	public class ForgetPasswrodRequestValidation : AbstractValidator<ForgetPasswrodRequest>
	{
        public ForgetPasswrodRequestValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
