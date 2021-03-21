using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiSimplesCSharp.Constants.LogEvents;
using WebApiSimplesCSharp.Models.Common;
using WebApiSimplesCSharp.Models.Usuarios;
using WebApiSimplesCSharp.Services.Usuarios;

namespace WebApiSimplesCSharp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuariosController : ControllerBase
	{
		private readonly IConsultaUsuarioService consultaUsuarioService;
		private readonly IManutencaoUsuarioService manutencaoUsuarioService;
		private readonly ILogger<UsuariosController> logger;

		public UsuariosController(
			IConsultaUsuarioService consultaUsuarioService,
			IManutencaoUsuarioService manutencaoUsuarioService,
			ILogger<UsuariosController> logger)
		{
			this.consultaUsuarioService = consultaUsuarioService;
			this.manutencaoUsuarioService = manutencaoUsuarioService;
			this.logger = logger;
		}

		public const int MAX_ITEMS_LIMIT = 1000;

		[HttpGet]
		public ListViewModel<UsuarioViewModel> Listar(string? q = null,
			int? skip = null, [Range(1, MAX_ITEMS_LIMIT)] int? limit = null,
			bool? countTotal = null)
		{
			skip ??= 0;
			limit ??= MAX_ITEMS_LIMIT;
			countTotal ??= false;

			var (usuarios, totalItems) = consultaUsuarioService.GetList(q, skip.Value, limit, countTotal: countTotal.Value);

			return (usuarios.Select(u => u.ToViewModel()), totalItems) switch
			{
				(var items, int totalCount) => new ListViewModel<UsuarioViewModel>.WithCount(items, totalCount),
				(var items, _) => new ListViewModel<UsuarioViewModel>.Flat(items),
			};
		}

		[HttpGet("{id}")]
		public ActionResult<UsuarioViewModel> Get(int id)
		{
			var usuario = consultaUsuarioService.GetById(id);

			if (usuario is null) {
				return NotFound();
			}

			return usuario.ToViewModel();
		}


		[HttpPost]
		public async Task<ActionResult<IdViewModel<int>>> Criar([FromBody] CriarUsuarioInputModel criarUsuarioInputModel)
		{
			int idGerado = await manutencaoUsuarioService.Criar(criarUsuarioInputModel);
			logger.LogInformation(AcessoLogEvents.UsuarioCriado, "Usuário criado: {Id}", idGerado);

			return CreatedAtAction(nameof(Get), new { id = idGerado }, new IdViewModel<int> { Id = idGerado });
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> Atualizar(int id, [FromBody] AtualizarUsuarioInputModel atualizarUsuarioInputModel)
		{
			if (!consultaUsuarioService.Exists(id)) {
				return NotFound();
			}

			await manutencaoUsuarioService.Atualizar(id, atualizarUsuarioInputModel);
			logger.LogInformation(AcessoLogEvents.UsuarioAtualizado, "Usuário atualizado: {Id}", id);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Excluir(int id)
		{
			if (!consultaUsuarioService.Exists(id)) {
				return NotFound();
			}

			await manutencaoUsuarioService.Excluir(id);
			logger.LogInformation(AcessoLogEvents.UsuarioExcluido, "Usuário excluído: {Id}", id);

			return NoContent();
		}

	}
}
