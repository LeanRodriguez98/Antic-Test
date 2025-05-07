namespace System.Collections.Generic
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			foreach (object obj in source)
			{
				if (obj is TResult result)
					yield return result;
			}
		}

		public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null)
				throw new ArgumentNullException(nameof(first));
			if (second == null)
				throw new ArgumentNullException(nameof(second));

			EqualityComparer<TSource> comparer = EqualityComparer<TSource>.Default;

			using (IEnumerator<TSource> firstEnumerator = first.GetEnumerator())
			{
				using (IEnumerator<TSource> secondEnumerator = second.GetEnumerator())
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
	}
}
