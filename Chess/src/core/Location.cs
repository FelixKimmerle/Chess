using System;
using Chess.src.core.pieces;

namespace Chess.src.core
{
    public class Location
    {
        private readonly int file;
        private readonly int rank;

        public Location(int file, int rank)
        {
            this.file = file;
            this.rank = rank;
        }

        public Location(string source)
        {
            file = char.ToLower(source[0]) - (int)'a';
            rank = source[1] - (int)'0' - 1;
        }

        public int GetFile()
        {
            return file;
        }

        public int GetRank()
        {
            return rank;
        }

        public char GetFileChar()
        {
            return (char)('a' + file);
        }

        public char GetRankChar()
        {
            return (char)('1' + rank);
        }

        public bool IsValid()
        {
            return file >= 0 && file <= 7 && rank >= 0 && rank <= 7;
        }

        public int EulerDistance(Location location)
        {
            return Math.Abs(file - location.file) + Math.Abs(rank - location.rank);
        }

        public Location Forward(PieceColor pieceColor, int number = 1)
        {
            if (pieceColor == PieceColor.White)
            {
                return new Location(file, rank + number);
            }
            else
            {
                return new Location(file, rank - number);
            }
        }

        public Location Offset(int x, int y = 0)
        {
            return new Location(this.file + x, this.rank + y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Location other)
            {
                return file == other.file && rank == other.rank;
            }
            return false;
        }

        public override string ToString()
        {
            return "" + (char)('a' + file) + (rank + 1);
        }

        public override int GetHashCode()
        {
            return file.GetHashCode() + rank.GetHashCode();
        }
    }
}