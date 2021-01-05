using System.Collections.Generic;
using Chess.src.core.moves;

namespace Chess.src.core.pieces
{
    public class Pawn : FirstMover
    {
        public Pawn(Location location, PieceColor pieceColor) : base(location, pieceColor, PieceType.Pawn)
        {
        }

        protected override HashSet<Move> CalculatePossibleMoves(Board board)
        {
            HashSet<Move> moves = new HashSet<Move>();

            //Normal case
            Location oneInFront = location.Forward(pieceColor);
            if (oneInFront.IsValid() && board.IsFree(oneInFront))
            {
                moves.Add(new AtomicMove(pieceType, location, oneInFront));

                //Doppelschritt
                Location twoInFront = location.Forward(pieceColor, 2);
                if (twoInFront.IsValid() && board.IsFree(twoInFront) && !WasMoved())
                {
                    moves.Add(new AtomicMove(pieceType, location, twoInFront));
                }
            }



            //Schlagen
            Location left = oneInFront.Offset(-1);
            if (left.IsValid() && board.IsEnemy(left, pieceColor))
            {
                moves.Add(new AtomicMove(pieceType, location, left));
            }

            Location right = oneInFront.Offset(1);
            if (right.IsValid() && board.IsEnemy(right, pieceColor))
            {
                moves.Add(new AtomicMove(pieceType, location, right));
            }
            //TODO Add all moves https://www.wiki-schacharena.de/Bauer
            return moves;
        }
    }
}