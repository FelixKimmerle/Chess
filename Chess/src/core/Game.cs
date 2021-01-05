using System;
using System.Collections.Generic;
using Chess.src.core.engine;
using Chess.src.core.moves;
using Chess.src.core.pieces;

namespace Chess.src.core
{
    public class Game
    {
        private readonly Board board = new Board();
        private readonly List<Move> moves = new List<Move>();
        private readonly Stack<Piece> capturedPieces = new Stack<Piece>();
        private PieceColor turn = PieceColor.White;



        public Game()
        {
            board.Print();

        }

        public Board GetBoard()
        {
            return board;
        }

        public PieceColor GetTurn()
        {
            return turn;
        }

        public Move Parse(string str)
        {
            Move move;
            if (str[0] == '0')
            {
                move = new Castling(str, turn);
            }
            else
            {
                move = new AtomicMove(str);
            }
            return move;
        }

        private void ExecuteAtomicMove(AtomicMove move)
        {
            Location source = move.GetSource();
            Location destination = move.GetDestination();

            Piece piece = board.Get(source);
            Piece destPiece = board.Get(destination);

            if (destPiece != null)
            {
                capturedPieces.Push(destPiece);
                move.Capture();

                if (destPiece.GetPieceColor().IsWhite())
                {
                    board.RemoveWhite(destPiece);
                }
                else
                {
                    board.RemoveBlack(destPiece);
                }
            }

            board.Set(destination, piece);
            board.Set(source, null);
            piece.Do(move);
            board.Update();
        }

        private void ExecuteAtomicMoveReversed(AtomicMove move)
        {
            Location source = move.GetDestination();
            Location destination = move.GetSource();

            Piece piece = board.Get(source);
            board.Set(destination, piece);
            piece.Undo(move);
            if (move.DidCapture())
            {
                Piece destPiece = capturedPieces.Pop();
                board.Add(destPiece);
            }
            else
            {
                board.Set(source, null);
            }
            board.Update();
        }

        private void ExecuteMove(Move move, bool reversed = false)
        {
            List<AtomicMove> atomicMoves = move.GetAtomicMoves();

            if (reversed)
            {
                atomicMoves.Reverse();
            }

            foreach (AtomicMove atomicMove in atomicMoves)
            {
                if (!reversed)
                {
                    ExecuteAtomicMove(atomicMove);
                }
                else
                {
                    ExecuteAtomicMoveReversed(atomicMove);
                }

            }
        }

        public bool Do(Move move)
        {
            Piece piece = board.Get(move.GetFirstSource());
            if (piece.IsPossible(move))
            {
                moves.Add(move);
                turn = turn.Other();
                ExecuteMove(move);
                board.Print();
                return true;
            }
            return false;
        }

        public Move Undo()
        {
            if (moves.Count > 0)
            {
                Move move = moves[moves.Count - 1];
                moves.RemoveAt(moves.Count - 1);
                turn = turn.Other();
                ExecuteMove(move, true);
                board.Print();
                return move;
            }
            return null;
        }

        public void Reset()
        {
            board.Clean();
            moves.Clear();
            capturedPieces.Clear();
            turn = PieceColor.White;
        }
    }
}