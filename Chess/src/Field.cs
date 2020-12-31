using Chess.src.moves;
using Chess.src.pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.src
{
    public class Field
    {
        private Piece[,] field = new Piece[8, 8];
        private readonly HashSet<Piece> whitePieces = new HashSet<Piece>();
        private readonly HashSet<Piece> blackPieces = new HashSet<Piece>();

        public Field()
        {
            //Clean();
            Add(new Rook(new BoardLocation(0, 7), Piece.PieceColor.Black));
            Add(new Rook(new BoardLocation(0, 0), Piece.PieceColor.White));
            Add(new Rook(new BoardLocation(7, 7), Piece.PieceColor.Black));
            Add(new Rook(new BoardLocation(7, 0), Piece.PieceColor.White));

        }
        public void Clean()
        {
            field = new Piece[8, 8];

            //Pawns
            for (int i = 0; i < 8; i++)
            {
                Add(new Pawn(new BoardLocation(i, 6), Piece.PieceColor.Black));
                Add(new Pawn(new BoardLocation(i, 1), Piece.PieceColor.White));
            }

            //Rook
            Add(new Rook(new BoardLocation(0, 7), Piece.PieceColor.Black));
            Add(new Rook(new BoardLocation(0, 0), Piece.PieceColor.White));
            Add(new Rook(new BoardLocation(7, 7), Piece.PieceColor.Black));
            Add(new Rook(new BoardLocation(7, 0), Piece.PieceColor.White));

            //Knight
            Add(new Knight(new BoardLocation(1, 7), Piece.PieceColor.Black));
            Add(new Knight(new BoardLocation(1, 0), Piece.PieceColor.White));
            Add(new Knight(new BoardLocation(6, 7), Piece.PieceColor.Black));
            Add(new Knight(new BoardLocation(6, 0), Piece.PieceColor.White));

            //Bishop
            Add(new Bishop(new BoardLocation(2, 7), Piece.PieceColor.Black));
            Add(new Bishop(new BoardLocation(2, 0), Piece.PieceColor.White));
            Add(new Bishop(new BoardLocation(5, 7), Piece.PieceColor.Black));
            Add(new Bishop(new BoardLocation(5, 0), Piece.PieceColor.White));

            //King
            Add(new King(new BoardLocation(4, 7), Piece.PieceColor.Black));
            Add(new King(new BoardLocation(4, 0), Piece.PieceColor.White));

            //Queen
            Add(new Queen(new BoardLocation(3, 7), Piece.PieceColor.Black));
            Add(new Queen(new BoardLocation(3, 0), Piece.PieceColor.White));
        }

        public void ExecuteAtomicMove(AtomicMove move)
        {
            Piece piece = Get(move.GetStart());
            Piece destPiece = Get(move.GetDestination());

            if(destPiece != null && destPiece.GetPieceColor() == Piece.PieceColor.White)
            {
                whitePieces.Remove(destPiece);
            }
            else
            {
                blackPieces.Remove(destPiece);
            }

            Set(move.GetDestination(), piece);
            Set(move.GetStart(), null);
            piece.ExecuteMove(move);

            //System.Console.WriteLine("W: " + whitePieces.Count + " B: " + blackPieces.Count);
        }

        public HashSet<IMove> GetEnemyMoves(Piece.PieceColor color)
        {
            HashSet<IMove> moves = new HashSet<IMove>();

            HashSet<Piece> pieces;

            if(color == Piece.PieceColor.White)
            {
                pieces = blackPieces;
            }
            else
            {
                pieces = whitePieces;
            }

            foreach (Piece piece in pieces)
            {
                HashSet<IMove> possibleMove = piece.GetPossibleMoves(this);
                foreach (IMove m in possibleMove)
                {
                    Piece other = Get(m.GetDestination());
                    if(other != null && other.GetPieceColor() == color)
                    {
                        moves.Add(m);
                    }
                }
            }

            return moves;
        }

        public void ExecuteMove(IMove move)
        {
            move.ExecuteMove(this);
        }

        private void Add(Piece piece)
        {
            if(piece.GetPieceColor() == Piece.PieceColor.White)
            {
                whitePieces.Add(piece);
            }
            else
            {
                blackPieces.Add(piece);
            }
            BoardLocation location = piece.GetBoardLocation();
            Set(location, piece);
        }
        private void Set(BoardLocation location, Piece piece)
        {
            field[location.GetX(), location.GetY()] = piece;
        }

        public Piece Get(BoardLocation location)
        {
            if (location.GetX() < 0 || location.GetX() > 7 || location.GetY() < 0 || location.GetY() > 7)
            {
                return null;
            }
            return field[location.GetX(), location.GetY()];
        }

        public Piece Get(int x, int y)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
            {
                return null;
            }
            return field[x, y];
        }
        public bool IsFree(BoardLocation location)
        {
            return Get(location) == null;
        }

        public bool IsEnemy(BoardLocation location, Piece.PieceColor color)
        {
            return !IsFree(location) && Get(location).GetPieceColor() != color;
        }

        private void PrintLine(int y, bool printPiece = false, bool printLocation = false)
        {
            for (int x = 0; x < 8; x++)
            {
                Piece piece = field[x, 7 - y];

                if ((x + y % 2) % 2 == 0)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                }
                if (printLocation)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    BoardLocation location = new BoardLocation(x, 7 - y);
                    Console.Write(location.ToString() + " ");
                }
                else
                {
                    Console.Write("   ");
                }
                if (printPiece && piece != null)
                {
                    Piece.PieceColor pieceColor = piece.GetPieceColor();
                    if (pieceColor == Piece.PieceColor.White)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.Write(piece.GetPieceType().GetLetter());
                }
                else
                {
                    Console.Write(" ");
                }
                Console.Write("   ");
            }
            Console.ResetColor();
            Console.WriteLine();
        }
        public void Print()
        {
            for (int y = 0; y < 8; y++)
            {
                PrintLine(y, false, true);
                PrintLine(y, true);
                PrintLine(y);
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Piece piece = field[x, 7 - y];
                    if (piece != null)
                    {
                        sb.Append(piece.GetPieceType().GetLetter() + " ");
                    }
                    else
                    {
                        sb.Append("_");
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
