using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Shared.Models.Authentication
{
	public record ResetPasswordRequest(
		string Email,
		string Code,
		string NewPassword
	);
}
