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

        public void Update(Position position)
        {
            possibleMoves = CalculatePossibleMoves(position);
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

        protected abstract HashSet<Move> CalculatePossibleMoves(Position position);

        protected HashSet<Move> checkDirection(int x, int y, Position position)
        {
            HashSet<Move> moves = new HashSet<Move>();

            Location current = location.Offset(x, y);

            while (current.IsValid() && position.IsFree(current))
            {
                moves.Add(new AtomicMove(pieceType, location, current));
                current = current.Offset(x, y);
            }

            if (position.IsEnemy(current, pieceColor))
            {
                moves.Add(new AtomicMove(pieceType, location, current));
            }

            return moves;
        }
    }
}