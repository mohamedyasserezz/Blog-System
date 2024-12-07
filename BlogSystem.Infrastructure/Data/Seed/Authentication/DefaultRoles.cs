using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Infrastructure.Data.Seed.Authentication
{
	public static class DefaultRoles
	{
		public const string Admin = nameof(Admin);
		public const string AdminRoleId = "92b75286-d8f8-4061-9995-e6e23ccdee94";
		public const string AdminRoleConcurrencyStamp = "f51e5a91-bced-49c2-8b86-c2e170c0846c";

		public const string Editor = nameof(Editor);
		public const string EditorRoleId = "f739fe98-d8d7-4868-9b0c-51a9728863de";
		public const string EditorRoleConcurrencyStamp = "65281949-fea3-4e86-a4b3-d4ca59e465a0";

		public const string Reader = nameof(Reader);
		public const string ReaderRoleId = "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4";
		public const string ReaderRoleConcurrencyStamp = "5ee6bc12-5cb0-4304-91e7-6a00744e042a";
	}
}
