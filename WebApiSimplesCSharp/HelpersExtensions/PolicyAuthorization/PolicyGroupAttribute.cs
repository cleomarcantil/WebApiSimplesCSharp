using System;

namespace WebApiSimplesCSharp.HelpersExtensions.PolicyAuthorization
{
	[System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class PolicyGroupAttribute : Attribute
	{
		public PolicyGroupAttribute(string groupName) => GroupName = groupName;

		public string GroupName { get; }
	}
}
