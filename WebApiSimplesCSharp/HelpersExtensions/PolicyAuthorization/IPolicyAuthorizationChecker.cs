using System.Security.Claims;

namespace WebApiSimplesCSharp.HelpersExtensions.PolicyAuthorization
{
	public interface IPolicyAuthorizationChecker
	{
		bool IsPolicyAuthorizedForUser(string policy, ClaimsPrincipal user);
	}
}
