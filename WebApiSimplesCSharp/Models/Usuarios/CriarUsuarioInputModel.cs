using System.ComponentModel.DataAnnotations;

namespace WebApiSimplesCSharp.Models.Usuarios
{
	public class CriarUsuarioInputModel
	{
		[Required, MaxLength(50)]
		public string Nome { get; set; }

		[Required, MaxLength(50)]
		public string Login { get; set; }

		[Required, MaxLength(20)]
		public string Senha { get; set; }
	}


}
