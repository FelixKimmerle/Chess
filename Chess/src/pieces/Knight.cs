using System;
using System.Collections.Generic;
using System.Text;

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

        public override HashSet<Move> GetPossibleMoves(Field field)
        {
            HashSet<Move> moves = new HashSet<Move>();

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
                    moves.Add(new Move(GetPieceType(), location, destination));
                }
            }
            return moves;
        }

    }
}
