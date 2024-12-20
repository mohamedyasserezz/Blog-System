﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Shared.Models.Authentication
{
	public record RegisterRequest(
		string Email,
		string Password,
		string FirstName,
		string LastName
	);
}
