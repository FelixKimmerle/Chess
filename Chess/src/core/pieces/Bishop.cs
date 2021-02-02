using System.Collections.Generic;
using Chess.src.core.moves;

namespace Chess.src.core.pieces
{
    public class Bishop : Piece
    {
        public Bishop(Location location, PieceColor pieceColor) : base(location, pieceColor, PieceType.Bishop)
        {
        }

        protected override HashSet<Move> CalculatePossibleMoves(Position position)
        {
            HashSet<Move> moves = new HashSet<Move>();

            moves.UnionWith(checkDirection(1, 1, position));
            moves.UnionWith(checkDirection(1, -1, position));
            moves.UnionWith(checkDirection(-1, 1, position));
            moves.UnionWith(checkDirection(-1, -1, position));

            return moves;
        }
    }
}