using Chess.src.core.pieces;

namespace Chess.src.core.moves
{
    public class KingsideCastling : Castling
    {
        public KingsideCastling(PieceColor color)
        {
            int rank = color.GetBackRank();
            kingMove = new AtomicMove(PieceType.King, new Location(4, rank), new Location(6, rank));
            rookMove = new AtomicMove(PieceType.Rook, new Location(7, rank), new Location(5, rank));
        }
    }
}