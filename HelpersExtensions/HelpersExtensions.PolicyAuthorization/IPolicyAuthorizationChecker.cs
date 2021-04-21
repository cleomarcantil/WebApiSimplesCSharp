using System.Security.Claims;

namespace HelpersExtensions.PolicyAuthorization
{
	public interface IPolicyAuthorizationChecker
	{
		bool IsPolicyAuthorizedForUser(string policy, ClaimsPrincipal user);
	}
}
