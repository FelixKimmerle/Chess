using System;
using System.Collections.Generic;
using System.Text;
using Chess.src.moves;

namespace Chess.src.pieces
{
    public abstract class Piece
    {
        public enum PieceColor
        {
            White,
            Black,
        }

        protected bool moved = false;
        protected BoardLocation location;
        protected readonly PieceColor pieceColor;
        public abstract PieceType GetPieceType();
        public abstract HashSet<IMove> GetPossibleMoves(Field field);
        public bool IsPossible(IMove move, Field field)
        {
            var moves = GetPossibleMoves(field);
            return moves.Contains(move);
        }
        public virtual void ExecuteMove(AtomicMove move)
        {
            moved = true;
            location = move.GetDestination();
        }

        public bool WasMoved()
        {
            return moved;
        }

        public Piece(BoardLocation location, PieceColor pieceColor)
        {
            this.location = location;
            this.pieceColor = pieceColor;
        }

        public BoardLocation GetBoardLocation()
        {
            return location;
        }

        public PieceColor GetPieceColor()
        {
            return pieceColor;
        }

        protected HashSet<IMove> checkDirection(int x, int y, Field field)
        {
            HashSet<IMove> moves = new HashSet<IMove>();

            BoardLocation current = location.Offset(x, y);

            while (current.IsValid() && field.IsFree(current))
            {
                moves.Add(new AtomicMove(GetPieceType(), location, current));
                current = current.Offset(x, y);
            }

            if (field.IsEnemy(current, pieceColor))
            {
                moves.Add(new AtomicMove(GetPieceType(), location, current));
            }

            return moves;
        }
    }
}
