using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiSimplesCSharp.Constants.Policies;
using WebApiSimplesCSharp.Services.Auth;
using WebApiSimplesCSharp.Services.Permissoes;
using static HelpersExtensions.PolicyAuthorization.Discovery.PolicyDiscoverer;

namespace WebApiSimplesCSharp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SecurityController : ControllerBase
	{
		private readonly IPermissaoCheckerService permissaoCheckerService;
		private readonly IAuthService authService;
		private readonly ILogger<SecurityController> logger;

		public SecurityController(
			IPermissaoCheckerService permissaoCheckerService,
			IAuthService authService,
			ILogger<SecurityController> logger)
		{
			this.permissaoCheckerService = permissaoCheckerService;
			this.authService = authService;
			this.logger = logger;
		}

		[Authorize(SecurityPolicies.ListarPolicies)]
		[HttpGet("policies")]
		public IEnumerable<PolicyGroupInfo> GetPolicies()
		{
			var allPolicyGroups = GetAllPolicyGroups();

			return allPolicyGroups;
		}

		[Authorize(SecurityPolicies.ListarPolicies)]
		[HttpGet("policies/{groupName}")]
		public ActionResult<PolicyGroupInfo> GetPolicies(string groupName)
		{
			var groupPolicies = GetPolicyGroup(groupName);

			if (groupPolicies is null) {
				return NotFound($"PolicyGroup não encontrado: {groupName}");
			}

			return groupPolicies;
		}

		[AllowAnonymous]
		[HttpGet("check-permissoes")]
		public ExpandoObject CheckPermissoes([FromQuery(Name = "p")] string[] permissoes, [FromQuery] int? usuarioId = null)
		{
			usuarioId ??= authService.GetCurrentUserId();

			var hasPermissoes = new ExpandoObject();

			foreach (var p in permissoes) {
				var hasPermissao = (usuarioId is not null) && permissaoCheckerService.HasPermissao(p, usuarioId.Value);
				hasPermissoes.TryAdd(p, hasPermissao);
			}

			return hasPermissoes;
		}

	}
}
