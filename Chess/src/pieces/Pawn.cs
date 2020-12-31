using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.src.pieces
{
    public class Pawn : Piece
    {
        public Pawn(BoardLocation location, PieceColor pieceColor) : base(location, pieceColor)
        {

        }

        public override PieceType GetPieceType()
        {
            return PieceType.Pawn;
        }

        public override HashSet<Move> GetPossibleMoves(Field field)
        {
            HashSet<Move> moves = new HashSet<Move>();

            //Normal case
            BoardLocation oneInFront = location.Forward(pieceColor);
            if (oneInFront.IsValid() && field.IsFree(oneInFront))
            {
                moves.Add(new Move(GetPieceType(), location, oneInFront));
            }

            //Doppelschritt
            BoardLocation twoInFront = location.Forward(pieceColor, 2);
            if (twoInFront.IsValid() && field.IsFree(twoInFront) && !moved)
            {
                moves.Add(new Move(GetPieceType(), location, twoInFront));
            }

            //Schlagen
            BoardLocation left = oneInFront.Offset(-1);
            if (left.IsValid() && field.IsEnemy(left, pieceColor))
            {
                moves.Add(new Move(GetPieceType(), location, left));
            }

            BoardLocation right = oneInFront.Offset(1);
            if (right.IsValid() && field.IsEnemy(right, pieceColor))
            {
                moves.Add(new Move(GetPieceType(), location, right));
            }
            //TODO Add all moves https://www.wiki-schacharena.de/Bauer
            return moves;
        }

        
    }
}
