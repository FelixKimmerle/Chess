using Chess.src.core.pieces;

namespace Chess.src.core.moves
{
    public class QueensideCastling : Castling
    {
        public QueensideCastling(PieceColor color)
        {
            int rank = color.GetBackRank();
            kingMove = new AtomicMove(PieceType.King, new Location(4, rank), new Location(2, rank));
            rookMove = new AtomicMove(PieceType.Rook, new Location(0, rank), new Location(3, rank));
        }

        public override string ToString()
        {
            return "0-0-0";
        }
    }
}