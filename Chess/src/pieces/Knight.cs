using System;
using System.Collections.Generic;
using System.Text;
using Chess.src.moves;

namespace Chess.src.pieces
{
    public class Knight : Piece
    {
        public Knight(BoardLocation location, PieceColor pieceColor) : base(location, pieceColor)
        {

        }
        public override PieceType GetPieceType()
        {
            return PieceType.Knight;
        }

        public override HashSet<IMove> GetPossibleMoves(Field field)
        {
            HashSet<IMove> moves = new HashSet<IMove>();

            BoardLocation[] destinations =
            {
                location.Offset(1,2),
                location.Offset(2,1),

                location.Offset(2,-1),
                location.Offset(1,-2),

                location.Offset(-1,-2),
                location.Offset(-2,-1),

                location.Offset(-2,1),
                location.Offset(-1,2),
            };

            foreach (BoardLocation destination in destinations)
            {
                if (destination.IsValid() && (field.IsFree(destination) || field.IsEnemy(destination, pieceColor)))
                {
                    moves.Add(new AtomicMove(GetPieceType(), location, destination));
                }
            }
            return moves;
        }

    }
}
