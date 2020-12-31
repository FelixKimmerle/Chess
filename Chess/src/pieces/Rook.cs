using System;
using System.Collections.Generic;
using System.Text;
using Chess.src.moves;

namespace Chess.src.pieces
{
    public class Rook : Piece
    {
        public Rook(BoardLocation location, PieceColor pieceColor) : base(location, pieceColor)
        {

        }
        public override PieceType GetPieceType()
        {
            return PieceType.Rook;
        }

        
        public override HashSet<IMove> GetPossibleMoves(Field field)
        {
            HashSet<IMove> moves = new HashSet<IMove>();

            moves.UnionWith(checkDirection(1, 0, field));
            moves.UnionWith(checkDirection(0, 1, field));
            moves.UnionWith(checkDirection(-1, 0, field));
            moves.UnionWith(checkDirection(0, -1, field));

            return moves;
        }

    }
}
