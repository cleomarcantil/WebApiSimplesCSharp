using Microsoft.AspNetCore.Authorization;

namespace WebApiSimplesCSharp.HelpersExtensions.PolicyAuthorization
{
	public class PolicyCheckRequirement : IAuthorizationRequirement
	{
		public PolicyCheckRequirement(string policyName) => PolicyName = policyName;

		public string PolicyName { get; private set; }
	}
}
