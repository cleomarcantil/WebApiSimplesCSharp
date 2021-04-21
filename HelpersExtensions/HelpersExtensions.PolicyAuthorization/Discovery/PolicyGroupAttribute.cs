using System;

namespace HelpersExtensions.PolicyAuthorization.Discovery
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class PolicyGroupAttribute : Attribute
	{
		public PolicyGroupAttribute(string groupName) => GroupName = groupName;

		public string GroupName { get; }
	}
}
