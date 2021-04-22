using System.Security.Claims;
using HelpersExtensions.JwtAuthentication;
using HelpersExtensions.PolicyAuthorization;
using WebApiSimplesCSharp.Models;
using WebApiSimplesCSharp.Services.Permissoes;

namespace WebApiSimplesCSharp.Adapters
{
	class PolicyAuthorizationCheckerAdapter : IPolicyAuthorizationChecker
	{
		private readonly IPermissaoCheckerService permissaoCheckerService;

		public PolicyAuthorizationCheckerAdapter(IPermissaoCheckerService permissaoCheckerService)
			=> this.permissaoCheckerService = permissaoCheckerService;

		public bool IsPolicyAuthorizedForUser(string policy, ClaimsPrincipal user)
			=> (user.ToAuthUserData<AuthUserInfo>()?.Id is int userId) && permissaoCheckerService.HasPermissao(policy, userId);

	}
}
