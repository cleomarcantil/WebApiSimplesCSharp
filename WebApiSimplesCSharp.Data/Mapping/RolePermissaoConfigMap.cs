using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.Data.Mapping
{
	class RolePermissaoConfigMap : IEntityTypeConfiguration<RolePermissao>
	{
		public void Configure(EntityTypeBuilder<RolePermissao> builder)
		{
			builder.ToTable("RolesPermissoes");
			builder.HasKey(nameof(RolePermissao.RoleId), nameof(RolePermissao.Nome));

			builder.HasOne<Role>()
				.WithMany(r => r.Permissoes)
				.HasForeignKey(nameof(RolePermissao.RoleId));

			builder.Property(p => p.Nome)
				.HasMaxLength(2000);

		}
	}
}
