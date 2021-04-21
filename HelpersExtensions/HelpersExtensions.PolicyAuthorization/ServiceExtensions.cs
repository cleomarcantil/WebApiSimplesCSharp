using System;
using System.Linq;
using HelpersExtensions.PolicyAuthorization.Discovery;
using HelpersExtensions.PolicyAuthorization.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace HelpersExtensions.PolicyAuthorization
{
	public static class ServiceExtensions
	{
		public static void AddAuthorizationWithApplicationPolicies<TPolicyAuthorizationChecker>(this IServiceCollection services, Func<IServiceProvider, TPolicyAuthorizationChecker> implementationFactory)
			where TPolicyAuthorizationChecker : class, IPolicyAuthorizationChecker
		{
			services.AddPoliciesAndAuthorizationHandler();
			services.AddSingleton<IPolicyAuthorizationChecker>(implementationFactory);
		}


		public static void AddAuthorizationWithApplicationPolicies<TPolicyAuthorizationChecker>(this IServiceCollection services)
			where TPolicyAuthorizationChecker : class, IPolicyAuthorizationChecker
		{
			services.AddPoliciesAndAuthorizationHandler();
			services.AddSingleton<IPolicyAuthorizationChecker, TPolicyAuthorizationChecker>();
		}

		private static void AddPoliciesAndAuthorizationHandler(this IServiceCollection services)
		{
			services.AddAuthorizationCore(options =>
			{
				foreach (var policyInfo in PolicyDiscoverer.GetAllPolicyGroups().SelectMany(pg => pg.Policies)) {
					options.AddPolicy(policyInfo.Name, p => p.AddRequirements(new PolicyCheckRequirement(policyInfo.Name)));
				}
			});

			services.AddSingleton<IAuthorizationHandler, PolicyCheckHandler>();
		}
	}
}
