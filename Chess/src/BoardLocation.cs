using Chess.src.pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.src
{
    public class BoardLocation
    {
        private readonly int x;
        private readonly int y;

        public BoardLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public BoardLocation(string source)
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

        public int EulerDistance(BoardLocation location)
        {
            return Math.Abs(x - location.x) + Math.Abs(y - location.y);
        }

        public BoardLocation Forward(Piece.PieceColor pieceColor, int number = 1)
        {
            if (pieceColor == Piece.PieceColor.White)
            {
                return new BoardLocation(x, y + number);
            }
            else
            {
                return new BoardLocation(x, y - number);
            }
        }

        public BoardLocation Offset(int x, int y = 0)
        {
            return new BoardLocation(this.x + x, this.y + y);
        }

        public override bool Equals(object obj)
        {
            if (obj is BoardLocation other)
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
            return x + y;
        }
    }
}
