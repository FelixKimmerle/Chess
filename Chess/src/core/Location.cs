using System;
using Chess.src.core.pieces;

namespace Chess.src.core
{
    public class Location
    {
        private readonly int x;
        private readonly int y;

        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Location(string source)
        {
            x = char.ToLower(source[0]) - (int)'a';
            y = source[1] - (int)'0' - 1;
        }

        public int GetX()
        {
            return x;
        }

        public int GetY()
        {
            return y;
        }

        public bool IsValid()
        {
            return x >= 0 && x <= 7 && y >= 0 && y <= 7;
        }

        public int EulerDistance(Location location)
        {
            return Math.Abs(x - location.x) + Math.Abs(y - location.y);
        }

        public Location Forward(PieceColor pieceColor, int number = 1)
        {
            if (pieceColor == PieceColor.White)
            {
                return new Location(x, y + number);
            }
            else
            {
                return new Location(x, y - number);
            }
        }

        public Location Offset(int x, int y = 0)
        {
            return new Location(this.x + x, this.y + y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Location other)
            {
                return x == other.x && y == other.y;
            }
            return false;
        }

        public override string ToString()
        {
            return "" + (char)('a' + x) + (y + 1);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode();
        }
    }
}