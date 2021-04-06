using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiSimplesCSharp.Constants.LogEvents;
using WebApiSimplesCSharp.Data.Entities;
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


		#region Permissoes

		private const string VALIDATION_MESSAGE_POLICIES_REQUIRED = "Forneca um array com uma ou mais permissões!";
		private const string VALIDATION_MESSAGE_POLICIES_MIN_LENGTH = "Especifique pelo menos uma permissão!";

		[Authorize(RolesPolicies.Visualizar)]
		[HttpGet("{roleId}/permissoes")]
		public ActionResult<IEnumerable<string>> Permissoes(int roleId)
		{
			var role = consultaRoleService.GetById(roleId, new[] { nameof(Role.Permissoes) });

			if (role is null) {
				return NotFound();
			}

			var permissoes = role.Permissoes.Select(p => p.Nome);

			return Ok(permissoes);
		}


		[Authorize(RolesPolicies.AdicionarPermissoes)]
		[HttpPost("{roleId}/permissoes")]
		public async Task<ActionResult> AddPermissoes(int roleId,
			[FromBody,
			Required(ErrorMessage = VALIDATION_MESSAGE_POLICIES_REQUIRED),
			MinLength(1, ErrorMessage = VALIDATION_MESSAGE_POLICIES_MIN_LENGTH)]
			string[] permissoes)
		{
			if (!consultaRoleService.Exists(roleId)) {
				return NotFound();
			}

			await manutencaoRoleService.AdicionarPermissoes(roleId, permissoes);
			logger.LogInformation(AcessoLogEvents.PermissaoAdicionaNaRole, "Permissão(ões) '{Permissoes}' adicionada(s) na role {Id}", string.Join(", ", permissoes), roleId);

			return NoContent();
		}


		[Authorize(RolesPolicies.RemoverPermissoes)]
		[HttpDelete("{roleId}/permissoes")]
		public async Task<ActionResult> RemovePermissoes(int roleId,
			[FromBody,
			Required(ErrorMessage = VALIDATION_MESSAGE_POLICIES_REQUIRED),
			MinLength(1, ErrorMessage = VALIDATION_MESSAGE_POLICIES_MIN_LENGTH)]
			string[] permissoes)
		{
			if (!consultaRoleService.Exists(roleId)) {
				return NotFound();
			}

			await manutencaoRoleService.RemoverPermissoes(roleId, permissoes);
			logger.LogInformation(AcessoLogEvents.PermissaoRemovidaDaRole, "Permissão(ões) '{Permissoes}' removida(s) na role {Id}'", string.Join(", ", permissoes), roleId);

			return NoContent();
		}

		#endregion

		#region Usuários

		[Authorize(RolesPolicies.Visualizar)]
		[HttpGet("{roleId}/usuarios")]
		public ActionResult<IEnumerable<RoleUsuarioViewModel>> Usuarios(int roleId)
		{
			var role = consultaRoleService.GetById(roleId, new[] { nameof(Role.Usuarios) });

			if (role is null) {
				return NotFound();
			}

			var usuarios = role.Usuarios.Select(p => new RoleUsuarioViewModel
			{
				Id = p.Id,
				Nome = p.Nome
			});

			return Ok(usuarios);
		}

		#endregion

	}
}
