using BlogSystem.Domain.Enities;
using BlogSystem.Infrastructure.Data.Seed.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Infrastructure.Data.Configurations
{
	public class ApplicationRoleConfigurations : IEntityTypeConfiguration<ApplicationRole>
	{
		public void Configure(EntityTypeBuilder<ApplicationRole> builder)
		{
			builder.HasData([
				new ApplicationRole
				{
					Id = DefaultRoles.AdminRoleId,
					Name = DefaultRoles.Admin,
					NormalizedName = DefaultRoles.Admin.ToUpper(),
					ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp
				},
				new ApplicationRole
				{
					Id = DefaultRoles.EditorRoleId,
					Name = DefaultRoles.Editor,
					NormalizedName = DefaultRoles.Editor.ToUpper(),
					ConcurrencyStamp = DefaultRoles.EditorRoleConcurrencyStamp
				},
				new ApplicationRole
				{
					Id = DefaultRoles.ReaderRoleId,
					Name = DefaultRoles.Reader,
					NormalizedName = DefaultRoles.Reader.ToUpper(),
					ConcurrencyStamp = DefaultRoles.ReaderRoleConcurrencyStamp,
					IsDefault = true
				}
			]);
		}
	}
}
