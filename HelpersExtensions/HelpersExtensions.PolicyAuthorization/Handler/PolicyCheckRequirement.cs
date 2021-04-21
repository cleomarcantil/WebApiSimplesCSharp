using Microsoft.AspNetCore.Authorization;

namespace HelpersExtensions.PolicyAuthorization.Handler
{
	class PolicyCheckRequirement : IAuthorizationRequirement
	{
		public PolicyCheckRequirement(string policyName) 
			=> PolicyName = policyName;

		public string PolicyName { get; private set; }
	}
}
