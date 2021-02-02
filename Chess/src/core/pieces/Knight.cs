using System.Collections.Generic;
using Chess.src.core.moves;

namespace Chess.src.core.pieces
{
    public class Knight : Piece
    {
        public Knight(Location location, PieceColor pieceColor) : base(location, pieceColor, PieceType.Knight)
        {
        }

        protected override HashSet<Move> CalculatePossibleMoves(Position position)
        {
            HashSet<Move> moves = new HashSet<Move>();

            Location[] destinations =
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

            foreach (Location destination in destinations)
            {
                if (destination.IsValid() && (position.IsFree(destination) || position.IsEnemy(destination, pieceColor)))
                {
                    moves.Add(new AtomicMove(pieceType, location, destination));
                }
            }
            return moves;
        }
    }
}