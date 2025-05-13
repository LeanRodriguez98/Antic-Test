namespace System.Collections.Generic
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> OfType<T>(this IEnumerable source)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			foreach (object obj in source)
			{
				if (obj is T result)
					yield return result;
			}
		}

		public static bool SequenceEqual<T>(this IEnumerable<T> first, IEnumerable<T> second)
		{
			if (first == null)
				throw new ArgumentNullException(nameof(first));
			if (second == null)
				throw new ArgumentNullException(nameof(second));

			EqualityComparer<T> comparer = EqualityComparer<T>.Default;

			using (IEnumerator<T> firstEnumerator = first.GetEnumerator())
			{
				using (IEnumerator<T> secondEnumerator = second.GetEnumerator())
				{
					while (true)
					{
						bool moved1 = firstEnumerator.MoveNext();
						bool moved2 = secondEnumerator.MoveNext();

						if (!moved1 && !moved2)
							return true;

						if (moved1 != moved2)
							return false;

						if (!comparer.Equals(firstEnumerator.Current, secondEnumerator.Current))
							return false;
					}
				}
			}
		}

		public static IEnumerable<T> Concat<T>(IEnumerable<T> first, IEnumerable<T> second)
		{
			foreach (T item in first)
				yield return item;

			foreach (T item in second)
				yield return item;
		}

		public static T[] Add<T>(this T[] source, T item)
		{
			if (source == null)
				return new T[] { item };

			T[] result = new T[source.Length + 1];

			for (int i = 0; i < source.Length; i++)
			{
				result[i] = source[i];
			}

			result[source.Length] = item;
			return result;
		}

		public static T[] AddRange<T>(this T[] source, IEnumerable<T> items)
		{
			if (items == null)
				return source ?? Array.Empty<T>();

			int itemCount = 0;

			foreach (T _ in items)
			{
				itemCount++;
			}

			if (itemCount == 0)
				return source ?? Array.Empty<T>();

			if (source == null || source.Length == 0)
			{
				T[] newArray = new T[itemCount];
				int index = 0;
				foreach (T item in items)
				{ 
					newArray[index++] = item;
				}

				return newArray;
			}

			T[] combined = new T[source.Length + itemCount];
			for (int i = 0; i < source.Length; i++)
			{ 
				combined[i] = source[i];
			}

			int offset = source.Length;
			foreach (T item in items)
			{ 
				combined[offset++] = item;
			}

			return combined;
		}
	}
}
