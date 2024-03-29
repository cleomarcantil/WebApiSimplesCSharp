﻿using System.ComponentModel;
using HelpersExtensions.PolicyAuthorization.Discovery;

namespace WebApiSimplesCSharp.Services.Constants.Policies
{
	[PolicyGroup(GROUP_NAME)]
	[Description("Roles")]
	public static class RolesPolicies
	{
		private const string GROUP_NAME = nameof(RolesPolicies);
		public static string GroupName => GROUP_NAME;

		[Description("Consultar roles")]
		public const string Listar = GROUP_NAME + "." + nameof(Listar);

		[Description("Visualizar role")]
		public const string Visualizar = GROUP_NAME + "." + nameof(Visualizar);

		[Description("Criar role")]
		public const string Criar = GROUP_NAME + "." + nameof(Criar);

		[Description("Atualizar role")]
		public const string Atualizar = GROUP_NAME + "." + nameof(Atualizar);

		[Description("Excluir role")]
		public const string Excluir = GROUP_NAME + "." + nameof(Excluir);


		[Description("Adicionar permissões à role")]
		public const string AdicionarPermissoes = GROUP_NAME + "." + nameof(AdicionarPermissoes);

		[Description("Remover permissões da role")]
		public const string RemoverPermissoes = GROUP_NAME + "." + nameof(RemoverPermissoes);


	}

}