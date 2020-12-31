﻿using Chess.src.pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.src
{
    public class Move
    {
        private readonly PieceType piece;
        private readonly BoardLocation from;
        private readonly BoardLocation to;

        public Move(PieceType piece, BoardLocation from, BoardLocation to)
        {
            this.piece = piece;
            this.from = from;
            this.to = to;
        }

        public Move(string source)
        {
            string[] splitted = source.Split(new char[] { ' ', '-' });
            piece = PieceType.Pawn;
            int offset = 0;
            if(splitted.Length == 3)
            {
                piece = PiecesExtension.GetPiece(char.ToUpper(splitted[0][0]));
                offset = 1;
            }
            from = new BoardLocation(splitted[0 + offset]);
            to = new BoardLocation(splitted[1 + offset]);
        }

        public BoardLocation GetStart()
        {
            return from;
        }
        public BoardLocation GetDestination()
        {
            return to;
        }

        public override bool Equals(object obj)
        {
            if (obj is Move other)
            {
                return piece == other.piece && from.Equals(other.from) && to.Equals(other.to);
            }
            return false;
        }

        public override string ToString()
        {
            return piece.GetLetter() + " " + from + "-" + to;
        }

        public override int GetHashCode()
        {
            return piece.GetHashCode() + to.GetHashCode() + from.GetHashCode();
        }
    }
}
