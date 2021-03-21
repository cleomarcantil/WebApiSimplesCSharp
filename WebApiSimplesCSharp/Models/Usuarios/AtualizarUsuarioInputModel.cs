using System.ComponentModel.DataAnnotations;

namespace WebApiSimplesCSharp.Models.Usuarios
{
	public class AtualizarUsuarioInputModel
	{
		[Required, MaxLength(50)]
		public string Nome { get; set; }

	}

}
