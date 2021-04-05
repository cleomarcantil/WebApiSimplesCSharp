using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WebApiSimplesCSharp.HelpersExtensions.PolicyAuthorization;

namespace WebApiSimplesCSharp.Constants.Policies
{
	[PolicyGroup(GROUP_NAME)]
	[Description("Segurança")]
	public static class SecurityPolicies
	{
		private const string GROUP_NAME = nameof(SecurityPolicies);
		public static string GroupName => GROUP_NAME;

		[Description("Consultar acessos")]
		public const string ListarAcessos = GROUP_NAME + "." + nameof(ListarAcessos);

		[Description("Consultar policies")]
		public const string ListarPolicies = GROUP_NAME + "." + nameof(ListarPolicies);

	}
}
