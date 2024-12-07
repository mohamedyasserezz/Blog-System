﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Domain.Enities
{
	public class ApplicationRole : IdentityRole
	{
		public bool IsDefault { get; set; }
		public bool IsDeleted { get; set; }
	}
}
