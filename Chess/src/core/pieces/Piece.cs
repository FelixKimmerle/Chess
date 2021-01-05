using System.Collections.Generic;
using Chess.src.core.moves;

namespace Chess.src.core.pieces
{
    public abstract class Piece
    {
        protected Location location;
        protected readonly PieceColor pieceColor;
        protected readonly PieceType pieceType;
        private HashSet<Move> possibleMoves = new HashSet<Move>();


        protected Piece(Location location, PieceColor pieceColor, PieceType pieceType)
        {
            this.location = location;
            this.pieceColor = pieceColor;
            this.pieceType = pieceType;
        }

        public void Update(Board board)
        {
            possibleMoves = CalculatePossibleMoves(board);
        }

        public bool IsPossible(Move move)
        {
            return possibleMoves.Contains(move);
        }

        public HashSet<Move> GetPossibleMoves()
        {
            return possibleMoves;
        }

        public Location GetLocation()
        {
            return location;
        }

        public PieceColor GetPieceColor()
        {
            return pieceColor;
        }

        public PieceType GetPieceType()
        {
            return pieceType;
        }
        
        public virtual void Do(AtomicMove move)
        {
            this.location = move.GetDestination();
        }

        public virtual void Undo(AtomicMove move)
        {
            this.location = move.GetSource();
        }

        protected abstract HashSet<Move> CalculatePossibleMoves(Board board);

        protected HashSet<Move> checkDirection(int x, int y, Board board)
        {
            HashSet<Move> moves = new HashSet<Move>();

            Location current = location.Offset(x, y);

            while (current.IsValid() && board.IsFree(current))
            {
                moves.Add(new AtomicMove(pieceType, location, current));
                current = current.Offset(x, y);
            }

            if (board.IsEnemy(current, pieceColor))
            {
                moves.Add(new AtomicMove(pieceType, location, current));
            }

            return moves;
        }
    }
}