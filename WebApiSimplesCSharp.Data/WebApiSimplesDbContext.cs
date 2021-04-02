using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data.Entities;
using WebApiSimplesCSharp.Data.Mapping;

namespace WebApiSimplesCSharp.Data
{
	public class WebApiSimplesDbContext : DbContext
	{
		public WebApiSimplesDbContext(DbContextOptions<WebApiSimplesDbContext> options)
			: base(options)
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new UsuarioConfigMap());
			modelBuilder.ApplyConfiguration(new RoleConfigMap());
			modelBuilder.ApplyConfiguration(new RolePermissaoConfigMap());

		}

		public DbSet<Usuario> Usuarios { get; private set; }

		public DbSet<Role> Roles { get; private set; }

	}
}
