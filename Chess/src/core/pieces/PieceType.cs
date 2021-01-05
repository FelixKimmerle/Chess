using System;

namespace Chess.src.core.pieces
{
    public enum PieceType
    {
        King,
        Queen,
        Knight,
        Bishop,
        Rook,
        Pawn,
        Blank,
    }

    public static class PieceTypeExtension
    {
        private static readonly char[] letters = { 'K', 'Q', 'N', 'B', 'R', 'P' };
        private static readonly float[] points = { 0, 9, 3, 3, 5, 1, 0 };
        public static char GetLetter(this PieceType piece)
        {
            return letters[(int)piece];
        }

        public static float GetPoints(this PieceType piece)
        {
            return points[(int)piece];
        }

        public static PieceType GetPiece(char letter)
        {
            switch (letter)
            {
                case 'K':
                    return PieceType.King;
                case 'Q':
                    return PieceType.Queen;
                case 'N':
                    return PieceType.Knight;
                case 'B':
                    return PieceType.Bishop;
                case 'R':
                    return PieceType.Rook;
                case 'P':
                    return PieceType.Pawn;
                case '_':
                    return PieceType.Blank;
            }
            throw new ArgumentException("The character: " + letter + " is not associated with a piece.");
        }
    }
}