using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.src.pieces
{
    public class King : Piece
    {

        public King(BoardLocation location, PieceColor pieceColor) : base(location, pieceColor)
        {

        }
        public override PieceType GetPieceType()
        {
            return PieceType.King;
        }

        public override HashSet<Move> GetPossibleMoves(Field field)
        {
            HashSet<Move> moves = new HashSet<Move>();

            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    if (x != 0 || y != 0)
                    {
                        BoardLocation destination = location.Offset(x,y);

                        if (destination.IsValid() && (field.IsFree(destination) || field.IsEnemy(destination, pieceColor)))
                        {
                            moves.Add(new Move(GetPieceType(), location, destination));
                        }
                    }
                }
            }

            BoardLocation current = location.Offset(1, 0);
            
            while(current.IsValid() && field.IsFree(current))
            {
                current = current.Offset(1, 0);
            }

            Piece other = field.Get(current);
            if (current.IsValid() && other.GetPieceType() == PieceType.Rook && !other.WasMoved())
            {
                moves.Add(new Move(GetPieceType(), location, current));
            }

            current = location.Offset(-1, 0);

            while (current.IsValid() && field.IsFree(current))
            {
                current = current.Offset(-1, 0);
            }

            other = field.Get(current);
            if (current.IsValid() && other.GetPieceType() == PieceType.Rook && !other.WasMoved())
            {
                moves.Add(new Move(GetPieceType(), location, current));
            }

            return moves;
        }

    }
}
