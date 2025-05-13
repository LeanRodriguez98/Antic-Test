using System;

namespace AnticTest.Architecture.Exceptions
{
	public class BrokenGameRuleException : Exception
	{
		public BrokenGameRuleException(string message) : base(message) { }
	}
}
