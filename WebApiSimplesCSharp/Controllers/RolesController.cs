using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiSimplesCSharp.Models.Common;
using WebApiSimplesCSharp.Models.Roles;
using WebApiSimplesCSharp.Services.Roles;

namespace WebApiSimplesCSharp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RolesController : ControllerBase
	{
		private readonly IConsultaRoleService consultaRoleService;
		private readonly ILogger<RolesController> logger;

		public RolesController(
			IConsultaRoleService consultaRoleService,
			ILogger<RolesController> logger)
		{
			this.consultaRoleService = consultaRoleService;
			this.logger = logger;
		}


		public const int MAX_ITEMS_LIMIT = 1000;

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

		[HttpGet("{id}")]
		public ActionResult<RoleViewModel> Get(int id)
		{
			var role = consultaRoleService.GetById(id);

			if (role is null) {
				return NotFound();
			}

			return role.ToViewModel();
		}

	}
}
