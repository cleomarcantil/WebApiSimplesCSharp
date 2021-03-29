using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace WebApiSimplesCSharp.HelpersExtensions.PolicyAuthorization
{
	public static class PolicyDiscover
	{
		public record PolicyGroupInfo
		{
			public PolicyGroupInfo(string Name, string description, IEnumerable<(string name, string description)> policies)
			{
				this.Name = Name;
				Description = description;
				Policies = policies.Select(p => new PolicyInfo(p.name, p.description) { Group = this }).ToArray();
			}

			public string Name { get; }
			public string Description { get; }
			public PolicyInfo[] Policies { get; }
		}

		public record PolicyInfo(string Name, string Description)
		{
			[JsonIgnore]
			public PolicyGroupInfo Group { get; init; } = null!;
		}

		public static bool IsInitialized => (allPolicies is not null);

		public static void Init(IEnumerable<Assembly> assemblies)
		{
			if (IsInitialized) {
				throw new Exception($"{nameof(PolicyDiscover)} já inicializado!");
			}

			if (!assemblies.Any()) {
				throw new ArgumentException($"{nameof(PolicyDiscover)}.{nameof(Init)} requer que os assemblies sejam especificados!", nameof(assemblies));
			}

			allPolicies = new();
			allPoliciesGroups = new();

			foreach (var asm in assemblies) {
				foreach (var groupType in asm.GetTypes()) {
					if (groupType.GetCustomAttribute<PolicyGroupAttribute>() is { } policiesGroupType) {
						var policyGroup = new PolicyGroupInfo(
							Name: policiesGroupType.GroupName,
							description: GetDescription(groupType),
							policies: groupType.GetFields(BindingFlags.Public | BindingFlags.Static)
								.Select(f => (f.GetValue(null)?.ToString()!, GetDescription(f)))
						);

						allPoliciesGroups.Add(policyGroup.Name, policyGroup);

						foreach (var policy in policyGroup.Policies) {
							allPolicies.Add(policy.Name, policy);
						}
					}
				}
			}

			string GetDescription(MemberInfo m) => m.GetCustomAttribute<DescriptionAttribute>()?.Description ?? m.Name;
		}

		#region Internal

		private static Dictionary<string, PolicyGroupInfo>? allPoliciesGroups = null;
		private static Dictionary<string, PolicyInfo>? allPolicies = null;

		private static T GetInited<T>(T? value) => value ?? throw new Exception($"{nameof(PolicyDiscover)}.{nameof(Init)} não chamado!");

		#endregion



		public static PolicyInfo? GetPolicyInfo(string name)
			=> GetInited(allPolicies).TryGetValue(name, out var x) ? x : null;

		public static IEnumerable<PolicyInfo> GetPoliciesInfo(IEnumerable<string> names)
			=> names.Select(n => GetPolicyInfo(n)).Where(p => p is not null)!;

		public static IEnumerable<PolicyInfo> GetValidPoliciesInfo(IEnumerable<string> names)
		{
			var policiesInfo = GetPoliciesInfo(names);

			if (names.Except(policiesInfo.Select(p => p.Name)) is { } policiesInvalidas && policiesInvalidas.Any()) {
				throw new Exception($"Policy(ies) inválida(s): {string.Join(", ", policiesInvalidas)}");
			}

			return policiesInfo!;
		}

		public static PolicyGroupInfo? GetPolicyGroup(string name)
			=> GetInited(allPoliciesGroups).TryGetValue(name, out var x) ? x : null;

		public static IEnumerable<PolicyGroupInfo> GetAllPolicyGroups()
			=> GetInited(allPoliciesGroups).Values;

	}
}
