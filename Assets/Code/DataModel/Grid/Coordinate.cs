using System;

namespace AnticTest.DataModel.Grid
{
	public struct Coordinate : ICoordinate
	{
		private int x;
		private int y;

		public int X => x;
		public int Y => y;

		public ICoordinate InvalidCoordinate => new Coordinate(int.MinValue, int.MinValue);

		public Coordinate(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public Coordinate((int x, int y) coordinate)
		{
			x = coordinate.x;
			y = coordinate.y;
		}

		public ICoordinate GetCoordinate()
		{
			return this;
		}

		public void Set(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public void Set((int x, int y) coordinate)
		{
			Set(coordinate.x, coordinate.y);
		}

		public void Set(ICoordinate coordinate)
		{
			Set(coordinate.X, coordinate.Y);
		}

		public int Distance(ICoordinate other)
		{
			int diffX = Math.Abs(other.X - X);
			int diffY = Math.Abs(other.Y - Y);
			return (((other.X - X < 0) ^ ((Y % 2) != 0)) ?
				Math.Max(0, diffX - (diffY + 1) / 2) : Math.Max(0, diffX - (diffY) / 2)) + diffY;
		}

		public bool Equals(ICoordinate other)
		{
			return Equals((object)other);
		}

		public static bool operator ==(Coordinate lhs, Coordinate rhs) => lhs.x == rhs.x && lhs.y == rhs.y;

		public static bool operator !=(Coordinate lhs, Coordinate rhs) => !(lhs == rhs);

		public static Coordinate operator /(Coordinate coordinate, int divider) =>
			new Coordinate(coordinate.x / divider, coordinate.y / divider);

		public static Coordinate operator *(Coordinate coordinate, int multiplier) =>
			new Coordinate(coordinate.x * multiplier, coordinate.y * multiplier);

		public override string ToString()
		{
			return "{X = " + x + "; Y = " + y + "}";
		}

		public override bool Equals(object obj)
		{
			return obj is Coordinate coordinate &&
				   x == coordinate.x &&
				   y == coordinate.y;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(x, y);
		}

		public void SetAsInvalid()
		{
			this = (Coordinate)InvalidCoordinate;
		}

		public bool InInvalid()
		{
			return Equals(InvalidCoordinate);
		}
	}
}
