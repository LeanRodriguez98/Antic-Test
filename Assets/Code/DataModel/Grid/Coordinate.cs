using System;

namespace AnticTest.DataModel.Map
{
    public delegate void CoordinateEvent(Coordinate coordinate);

    public struct Coordinate
    {
        public int x;
        public int y;
        
        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
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
	}
}
