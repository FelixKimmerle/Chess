using System.Collections.Generic;
using Chess.src.core.moves;

namespace Chess.src.core.pieces
{
    public class King : FirstMover
    {
        public King(Location location, PieceColor pieceColor) : base(location, pieceColor, PieceType.King)
        {
        }

        protected override HashSet<Move> CalculatePossibleMoves(Position position)
        {
            HashSet<Move> moves = new HashSet<Move>();

            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    if (x != 0 || y != 0)
                    {
                        Location destination = location.Offset(x, y);

                        if (destination.IsValid() && (position.IsFree(destination) || position.IsEnemy(destination, pieceColor)))
                        {
                            moves.Add(new AtomicMove(pieceType, location, destination));
                        }
                    }
                }
            }
            if (!WasMoved())
            {
                //Short Rochade
                Location current = location.Offset(1, 0);
                while (current.IsValid() && position.IsFree(current))
                {
                    current = current.Offset(1, 0);
                }

                Piece other = position.Get(current);
                if (current.IsValid() && other is Rook shortRook && !shortRook.WasMoved())
                {
                    moves.Add(new KingsideCastling(pieceColor));
                }

                //Long Rochade
                current = location.Offset(-1, 0);

                while (current.IsValid() && position.IsFree(current))
                {
                    current = current.Offset(-1, 0);
                }

                other = position.Get(current);
                if (current.IsValid() && other is Rook longRook && !longRook.WasMoved())
                {
                    moves.Add(new QueensideCastling(pieceColor));
                }
            }
            return moves;
        }
    }
}