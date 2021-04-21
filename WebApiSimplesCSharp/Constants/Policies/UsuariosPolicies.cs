using System.ComponentModel;
using HelpersExtensions.PolicyAuthorization.Discovery;

namespace WebApiSimplesCSharp.Services.Constants.Policies
{
	[PolicyGroup(GROUP_NAME)]
	[Description("Usuários")]
	public static class UsuariosPolicies
	{
		private const string GROUP_NAME = nameof(UsuariosPolicies);
		public static string GroupName => GROUP_NAME;

		[Description("Consultar usuários")]
		public const string Listar = GROUP_NAME + "." + nameof(Listar);

		[Description("Visualizar usuário")]
		public const string Visualizar = GROUP_NAME + "." + nameof(Visualizar);

		[Description("Criar usuário")]
		public const string Criar = GROUP_NAME + "." + nameof(Criar);

		[Description("Atualizar usuário")]
		public const string Atualizar = GROUP_NAME + "." + nameof(Atualizar);

		[Description("Excluir usuário")]
		public const string Excluir = GROUP_NAME + "." + nameof(Excluir);


		[Description("Adicionar role ao usuário")]
		public const string AdicionarRole = GROUP_NAME + "." + nameof(AdicionarRole);

		[Description("Remover role do usuário")]
		public const string RemoverRole = GROUP_NAME + "." + nameof(RemoverRole);
	}
}