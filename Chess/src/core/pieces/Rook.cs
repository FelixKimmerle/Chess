using System.Collections.Generic;
using Chess.src.core.moves;

namespace Chess.src.core.pieces
{
    public class Rook : FirstMover
    {
        
        public Rook(Location location, PieceColor pieceColor) : base(location, pieceColor, PieceType.Rook)
        {
            
        }

        protected override HashSet<Move> CalculatePossibleMoves(Board board)
        {
            HashSet<Move> moves = new HashSet<Move>();

            moves.UnionWith(checkDirection(1, 0, board));
            moves.UnionWith(checkDirection(0, 1, board));
            moves.UnionWith(checkDirection(-1, 0, board));
            moves.UnionWith(checkDirection(0, -1, board));

            return moves;
        }
    }
}