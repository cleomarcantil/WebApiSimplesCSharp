using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HelpersExtensions.PolicyAuthorization.Handler
{
	class PolicyCheckHandler : AuthorizationHandler<PolicyCheckRequirement>
	{
		private readonly IPolicyAuthorizationChecker policyCheckService;

		public PolicyCheckHandler(IPolicyAuthorizationChecker policyCheckService)
			=> this.policyCheckService = policyCheckService;

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyCheckRequirement requirement)
		{
			if (policyCheckService.IsPolicyAuthorizedForUser(requirement.PolicyName, context.User)) {
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}

	}
}
