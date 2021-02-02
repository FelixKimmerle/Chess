using System;
using System.Collections.Generic;
using Chess.src.core.pieces;

namespace Chess.src.core.moves
{
    public class Castling : Move
    {
        protected AtomicMove kingMove;
        protected AtomicMove rookMove;

        protected Castling()
        {
        }

        static public Castling Parse(string str, PieceColor color)
        {
            if (str == "0-0")
            {
                return new KingsideCastling(color);
            }
            else if (str == "0-0-0")
            {
                return new QueensideCastling(color);
            }
            else
            {
                throw new ArgumentException("'" + str + "' is not a Castling move.");
            }
        }



        public override List<AtomicMove> GetAtomicMoves()
        {
            return new List<AtomicMove>() { kingMove, rookMove };
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