using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.Models.Roles
{
	public static class RoleToViewModelExtensions
	{
		public static RoleViewModel ToViewModel(this Role role)
			=> new RoleViewModel
			{
				Id = role.Id,
				Nome = role.Nome,
				Descricao = role.Descricao,
			};

	}

}
