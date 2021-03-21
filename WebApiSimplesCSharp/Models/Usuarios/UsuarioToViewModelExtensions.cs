using WebApiSimplesCSharp.Data.Entities;

namespace WebApiSimplesCSharp.Models.Usuarios
{
	public static class UsuarioToViewModelExtensions
	{
		public static UsuarioViewModel ToViewModel(this Usuario usuario)
			=> new UsuarioViewModel
			{
				Id = usuario.Id,
				Login = usuario.Login,
				Nome = usuario.Nome,
			};

	}
}
