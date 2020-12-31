﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.src.pieces
{
    public class Rook : Piece
    {
        public Rook(BoardLocation location, PieceColor pieceColor) : base(location, pieceColor)
        {

        }
        public override PieceType GetPieceType()
        {
            return PieceType.Rook;
        }

        
        public override HashSet<Move> GetPossibleMoves(Field field)
        {
            HashSet<Move> moves = new HashSet<Move>();

            moves.UnionWith(checkDirection(1, 0, field));
            moves.UnionWith(checkDirection(0, 1, field));
            moves.UnionWith(checkDirection(-1, 0, field));
            moves.UnionWith(checkDirection(0, -1, field));

            return moves;
        }

    }
}