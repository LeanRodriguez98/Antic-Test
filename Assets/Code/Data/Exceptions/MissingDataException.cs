using System;

namespace AnticTest.Data.Exceptions
{
	public class MissingDataException : Exception
	{
		public MissingDataException() : base() { }
		public MissingDataException(string message) : base(message) { }
	}
}
