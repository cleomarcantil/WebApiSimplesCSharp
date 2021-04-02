using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.Data.Mapping
{
	class RoleConfigMap : IEntityTypeConfiguration<Role>
	{
		public void Configure(EntityTypeBuilder<Role> builder)
		{
			builder.ToTable("Roles");
			builder.HasKey(r => r.Id);

			builder.Property(r => r.Nome)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(r => r.Descricao)
				.HasMaxLength(250);

			builder.HasIndex(ix => ix.Nome).IsUnique();

			builder.Metadata.FindNavigation(nameof(Role.Permissoes))
				.SetPropertyAccessMode(PropertyAccessMode.Field);

			builder.HasMany(r => r.Usuarios)
				.WithMany(u => u.Roles)
				.UsingEntity<Dictionary<string, object>>(
					"UsuariosRoles",
					j => j.HasOne<Usuario>()
						.WithMany()
						.HasForeignKey("UsuarioId")
						.OnDelete(DeleteBehavior.Cascade),
					j => j.HasOne<Role>()
						.WithMany()
						.HasForeignKey("RoleId")
						.OnDelete(DeleteBehavior.Cascade),
					j => j.HasKey("UsuarioId", "RoleId")
				);
		}
	}
}
