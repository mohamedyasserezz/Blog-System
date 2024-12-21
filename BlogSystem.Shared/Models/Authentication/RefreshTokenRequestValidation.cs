﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Shared.Models.Authentication
{
	public class RefreshTokenRequestValidation : AbstractValidator<RefreshTokenRequest>
	{
        public RefreshTokenRequestValidation()
        {
			RuleFor(x => x.Token).NotEmpty();
			RuleFor(x => x.RefreshToken).NotEmpty();
		}
    }
}