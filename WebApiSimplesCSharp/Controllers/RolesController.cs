using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiSimplesCSharp.Constants.LogEvents;
using WebApiSimplesCSharp.Models.Common;
using WebApiSimplesCSharp.Models.Roles;
using WebApiSimplesCSharp.Services.Constants.Policies;
using WebApiSimplesCSharp.Services.Roles;

namespace WebApiSimplesCSharp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RolesController : ControllerBase
	{
		private readonly IConsultaRoleService consultaRoleService;
		private readonly IManutencaoRoleService manutencaoRoleService;
		private readonly ILogger<RolesController> logger;

		public RolesController(
			IConsultaRoleService consultaRoleService,
			IManutencaoRoleService manutencaoRoleService,
			ILogger<RolesController> logger)
		{
			this.consultaRoleService = consultaRoleService;
			this.manutencaoRoleService = manutencaoRoleService;
			this.logger = logger;
		}


		public const int MAX_ITEMS_LIMIT = 1000;

		[Authorize(RolesPolicies.Listar)]
		[HttpGet]
		public ListViewModel<RoleViewModel> Listar(string? q = null,
			int? skip = null, [Range(1, MAX_ITEMS_LIMIT)] int? limit = null,
			bool? countTotal = null)
		{
			skip ??= 0;
			limit ??= MAX_ITEMS_LIMIT;
			countTotal ??= false;

			var (usuarios, totalItems) = consultaRoleService.GetList(q, skip.Value, limit, countTotal: countTotal.Value);

			return (usuarios.Select(u => u.ToViewModel()), totalItems) switch
			{
				(var items, int totalCount) => new ListViewModel<RoleViewModel>.WithCount(items, totalCount),
				(var items, _) => new ListViewModel<RoleViewModel>.Flat(items),
			};
		}

		[Authorize(RolesPolicies.Visualizar)]
		[HttpGet("{id}")]
		public ActionResult<RoleViewModel> Get(int id)
		{
			var role = consultaRoleService.GetById(id);

			if (role is null) {
				return NotFound();
			}

			return role.ToViewModel();
		}


		[Authorize(RolesPolicies.Criar)]
		[HttpPost]
		public async Task<ActionResult<IdViewModel<int>>> Criar([FromBody] CriarRoleInputModel criarRoleInputModel)
		{
			int idGerado = await manutencaoRoleService.Criar(criarRoleInputModel);
			logger.LogInformation(AcessoLogEvents.RoleCriada, "Role criada: {Id}", idGerado);

			return CreatedAtAction(nameof(Get), new { id = idGerado }, new IdViewModel<int> { Id = idGerado });
		}

		[Authorize(RolesPolicies.Atualizar)]
		[HttpPut("{id}")]
		public async Task<ActionResult> Atualizar(int id, [FromBody] AtualizarRoleInputModel atualizarRoleInputModel)
		{
			if (!consultaRoleService.Exists(id)) {
				return NotFound();
			}

			await manutencaoRoleService.Atualizar(id, atualizarRoleInputModel);
			logger.LogInformation(AcessoLogEvents.RoleAtualizada, "Role atualizada: {Id}", id);

			return NoContent();
		}

		[Authorize(RolesPolicies.Excluir)]
		[HttpDelete("{id}")]
		public async Task<ActionResult> Excluir(int id)
		{
			if (!consultaRoleService.Exists(id)) {
				return NotFound();
			}

			await manutencaoRoleService.Excluir(id);
			logger.LogInformation(AcessoLogEvents.RoleExcluida, "Role excluída: {Id}", id);

			return NoContent();
		}

	}
}
