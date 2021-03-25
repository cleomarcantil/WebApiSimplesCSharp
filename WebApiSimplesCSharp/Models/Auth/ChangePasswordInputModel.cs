using System.ComponentModel.DataAnnotations;

namespace WebApiSimplesCSharp.Models.Auth
{
	public class ChangePasswordInputModel
	{
		[Required, MaxLength(20)]
		public string SenhaAtual { get; set; }

		[Required, MaxLength(20)]
		public string NovaSenha { get; set; }
	}
}