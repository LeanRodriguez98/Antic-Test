namespace System
{
	public static class TypeExtensions
	{
		public static bool InheritsFromRawGeneric(this Type type, Type generic)
		{
			if (type == null || generic == null || !generic.IsGenericTypeDefinition)
				return false;

			while (type != null && type != typeof(object))
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == generic)
					return true;

				foreach (var interfaceType in type.GetInterfaces())
				{
					if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == generic)
						return true;
				}

				type = type.BaseType;
			}

			return false;
		}
	}
}