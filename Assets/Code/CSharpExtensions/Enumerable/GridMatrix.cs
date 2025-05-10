namespace System.Collections.Generic
{
	[Serializable]
	public class GridMatrix<T>: IEnumerable<T>
	{
		public int width = 0;
		public int height = 0;
		public List<T> data = new List<T>();

		public GridMatrix(int width, int height, T defaultValue = default)
		{
			this.width = width;
			this.height = height;
			data = new List<T>();
			for (int i = 0; i < width * height; i++)
				data.Add(defaultValue);
		}

		public void Resize(int newWidth, int newHeight, T defaultValue = default)
		{
			var newData = new List<T>(newWidth * newHeight);
			for (int y = 0; y < newHeight; y++)
			{
				for (int x = 0; x < newWidth; x++)
				{
					if (x < width && y < height)
						newData.Add(Get(x, y));
					else
						newData.Add(defaultValue);
				}
			}

			width = newWidth;
			height = newHeight;
			data = newData;
		}

		public T this[int x, int y] { get { return Get(x, y); } }

		public T Get(int x, int y)
		{
			if (!IsValid(x, y))
				throw new IndexOutOfRangeException($"Get({x},{y}) out of range ({width}x{height})");

			return data[y * width + x];
		}

		public void Set(int x, int y, T value)
		{
			if (!IsValid(x, y))
				throw new IndexOutOfRangeException($"Set({x},{y}) out of range ({width}x{height})");

			data[y * width + x] = value;
		}

		public bool IsValid(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;

		public IEnumerator<T> GetEnumerator()
		{
			foreach (T element in data)
			{
				yield return element;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public (int x, int y) Lenght => new(width, height);
	}
}