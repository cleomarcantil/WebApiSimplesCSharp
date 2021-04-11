using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiSimplesCSharp.Data
{
	public static class DependencyInjectionExtensions
	{
		public static void AddPooledDbContextFactory(this IServiceCollection services, string connectionString)
		{
			services.AddPooledDbContextFactory<WebApiSimplesDbContext>(options => options.UseSqlServer(connectionString));
		}
	}
}
