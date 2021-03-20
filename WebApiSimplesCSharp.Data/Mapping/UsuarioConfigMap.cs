using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.Data.Mapping
{
	class UsuarioConfigMap : IEntityTypeConfiguration<Usuario>
	{
		public void Configure(EntityTypeBuilder<Usuario> builder)
		{
			builder.ToTable("Usuarios");
			builder.HasKey(u => u.Id);

			builder.Property(u => u.Nome)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(u => u.Login)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(u => u.HashSenha)
				.IsRequired()
				.HasMaxLength(200);

			builder.HasIndex(ix => ix.Nome).IsUnique();
			builder.HasIndex(ix => ix.Login).IsUnique();

		}
	}
}
