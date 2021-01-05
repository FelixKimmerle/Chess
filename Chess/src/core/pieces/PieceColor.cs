namespace Chess.src.core.pieces
{
    public enum PieceColor
    {
        White,
        Black,
    }

    public static class PieceColorExtension
    {
        public static PieceColor Other(this PieceColor pieceColor)
        {
            if(pieceColor == PieceColor.White)
            {
                return PieceColor.Black;
            }
            return PieceColor.White;
        }

        public static bool IsWhite(this PieceColor pieceColor)
        {
            return pieceColor == PieceColor.White;
        }

        public static bool IsBlack(this PieceColor pieceColor)
        {
            return pieceColor == PieceColor.Black;
        }
    }
}