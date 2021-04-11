using System;
using Microsoft.EntityFrameworkCore;
using WebApiSimplesCSharp.Data;

namespace WebApiSimplesCSharp.Tests
{
	class DbContextFactory : IDbContextFactory<WebApiSimplesDbContext>
	{
		private readonly Action<WebApiSimplesDbContext>? initData;

		public DbContextFactory(Action<WebApiSimplesDbContext>? initData = null)
			=> this.initData = initData;

		public WebApiSimplesDbContext CreateDbContext()
		{
			LastCreatedDbContext = DBInit.CreateInMemoryDbContextAsync().Result;

			initData?.Invoke(LastCreatedDbContext);

			return LastCreatedDbContext;
		}

		public WebApiSimplesDbContext LastCreatedDbContext { get; private set; }
	}
}
