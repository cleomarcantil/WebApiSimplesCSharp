using System.ComponentModel.DataAnnotations;

namespace WebApiSimplesCSharp.Models.Roles
{
	public class AtualizarRoleInputModel
	{
		[Required, MaxLength(50)]
		public string Nome { get; set; }

		[MaxLength(250)]
		public string? Descricao { get; set; }
	}
}
