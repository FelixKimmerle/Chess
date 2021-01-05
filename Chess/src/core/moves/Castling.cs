using System;
using System.Collections.Generic;
using Chess.src.core.pieces;

namespace Chess.src.core.moves
{
    public class Castling : Move
    {
        private AtomicMove kingMove;
        private AtomicMove rookMove;

        private void InitShort(int rank)
        {
            kingMove = new AtomicMove(PieceType.King, new Location(4, rank), new Location(6, rank));
            rookMove = new AtomicMove(PieceType.Rook, new Location(7, rank), new Location(5, rank));
        }

        private void InitLong(int rank)
        {
            kingMove = new AtomicMove(PieceType.King, new Location(4, rank), new Location(2, rank));
            rookMove = new AtomicMove(PieceType.Rook, new Location(0, rank), new Location(3, rank));
        }

        public Castling(bool shortRochade, PieceColor color)
        {
            int rank = 0;
            if (color == PieceColor.Black)
            {
                rank = 7;
            }
            if (shortRochade)
            {
                InitShort(rank);
            }
            else
            {
                InitLong(rank);
            }
        }

        public Castling(string str, PieceColor color)
        {
            int rank = 0;
            if (color == PieceColor.Black)
            {
                rank = 7;
            }

            if (str == "0-0")
            {
                InitShort(rank);
            }
            else if (str == "0-0-0")
            {
                InitLong(rank);
            }
            else
            {
                throw new ArgumentException("'" + str + "' is not a Castling move.");
            }
        }

        public override List<AtomicMove> GetAtomicMoves()
        {
            return new List<AtomicMove>() { kingMove, rookMove};
        }

        public override bool Equals(object obj)
        {
            if (obj is Castling other)
            {
                return kingMove.Equals(other.kingMove) && rookMove.Equals(other.rookMove);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return kingMove.GetHashCode() + rookMove.GetHashCode();
        }
    }
}