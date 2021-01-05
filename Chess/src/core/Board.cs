using System;
using System.Collections.Generic;
using Chess.src.core.engine;
using Chess.src.core.moves;
using Chess.src.core.pieces;

namespace Chess.src.core
{
    public class Board
    {
        private Piece[,] field = new Piece[8, 8];
        private readonly HashSet<Piece> whitePieces = new HashSet<Piece>();
        private readonly HashSet<Piece> blackPieces = new HashSet<Piece>();

        private HelperData whiteData;
        private HelperData blackData;

        public Board()
        {
            whiteData = new HelperData(this, PieceColor.White);
            blackData = new HelperData(this, PieceColor.Black);
            Clean();
        }

        public HashSet<Piece> GetWhitePieces()
        {
            return whitePieces;
        }

        public HashSet<Piece> GetBlackPieces()
        {
            return blackPieces;
        }

        public void Clean()
        {
            field = new Piece[8, 8];

            //Pawns
            for (int i = 0; i < 8; i++)
            {
                //Add(new Pawn(new Location(i, 6), PieceColor.Black));
                //Add(new Pawn(new Location(i, 1), PieceColor.White));
            }

            //Rook
            Add(new Rook(new Location(0, 7), PieceColor.Black));
            Add(new Rook(new Location(0, 0), PieceColor.White));
            Add(new Rook(new Location(7, 7), PieceColor.Black));
            Add(new Rook(new Location(7, 0), PieceColor.White));
            /*
            //Knight
            Add(new Knight(new Location(1, 7), PieceColor.Black));
            Add(new Knight(new Location(1, 0), PieceColor.White));
            Add(new Knight(new Location(6, 7), PieceColor.Black));
            Add(new Knight(new Location(6, 0), PieceColor.White));

            //Bishop
            Add(new Bishop(new Location(2, 7), PieceColor.Black));
            Add(new Bishop(new Location(2, 0), PieceColor.White));
            Add(new Bishop(new Location(5, 7), PieceColor.Black));
            Add(new Bishop(new Location(5, 0), PieceColor.White));
            */
            //King
            Add(new King(new Location(4, 7), PieceColor.Black));
            Add(new King(new Location(4, 0), PieceColor.White));

            //Queen
            Add(new Queen(new Location(3, 7), PieceColor.Black));
            Add(new Queen(new Location(3, 0), PieceColor.White));
            
            Update();
        }

        public void RemoveWhite(Piece piece)
        {
            whitePieces.Remove(piece);
        }

        public void RemoveBlack(Piece piece)
        {
            blackPieces.Remove(piece);
        }

        public void Add(Piece piece)
        {
            if (piece.GetPieceColor().IsWhite())
            {
                whitePieces.Add(piece);
            }
            else
            {
                blackPieces.Add(piece);
            }
            Location location = piece.GetLocation();
            Set(location, piece);
        }

        public void Set(Location location, Piece piece)
        {
            field[location.GetX(), location.GetY()] = piece;
        }

        public Piece Get(Location location)
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
        public bool IsFree(Location location)
        {
            return Get(location) == null;
        }

        public bool IsEnemy(Location location, PieceColor color)
        {
            return !IsFree(location) && Get(location).GetPieceColor() != color;
        }

        public void Update()
        {
            foreach (Piece piece in whitePieces)
            {
                piece.Update(this);
            }
            foreach (Piece piece in blackPieces)
            {
                piece.Update(this);
            }

            whiteData.Update();
            blackData.Update();
        }

        public HelperData GetWhiteData()
        {
            return whiteData;
        }

        public HelperData GetBlackData()
        {
            return blackData;
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
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Location location = new Location(x, 7 - y);
                    Console.Write(location.ToString() + " ");
                }
                else
                {
                    Console.Write("   ");
                }
                if (printPiece && piece != null)
                {
                    PieceColor pieceColor = piece.GetPieceColor();
                    if (pieceColor == PieceColor.White)
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

    }
}