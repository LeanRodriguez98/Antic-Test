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
			foreach (var item in first)
				yield return item;

			foreach (var item in second)
				yield return item;
		}
	}
}
