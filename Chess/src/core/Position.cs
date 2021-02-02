using System;
using System.Collections.Generic;
using System.Text;
using Chess.src.core.moves;
using Chess.src.core.pieces;

namespace Chess.src.core
{
    public class Position
    {
        private Piece[,] field = new Piece[8, 8];
        private readonly HashSet<Piece> whitePieces = new HashSet<Piece>();
        private readonly HashSet<Piece> blackPieces = new HashSet<Piece>();

        private readonly Stack<Piece> capturedPieces = new Stack<Piece>();
        private PieceColor turn = PieceColor.White;
        private readonly List<Move> moves = new List<Move>();
        private int halveMoveClock = 0;
        private Stack<int> halveMoveStack = new Stack<int>();


        //private HelperData whiteData;
        //private HelperData blackData;

        public Position()
        {
            //whiteData = new HelperData(this, PieceColor.White);
            //blackData = new HelperData(this, PieceColor.Black);
            Clean();
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
                move = Castling.Parse(str, turn);
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

            Piece piece = Get(source);
            Piece destPiece = Get(destination);

            if (destPiece != null)
            {
                capturedPieces.Push(destPiece);
                move.Capture();

                if (destPiece.GetPieceColor().IsWhite())
                {
                    whitePieces.Remove(destPiece);
                }
                else
                {
                    blackPieces.Remove(destPiece);
                }
            }

            Set(destination, piece);
            Set(source, null);
            piece.Do(move);
            Update();
        }

        private void ExecuteAtomicMoveReversed(AtomicMove move)
        {
            Location source = move.GetDestination();
            Location destination = move.GetSource();

            Piece piece = Get(source);
            Set(destination, piece);
            piece.Undo(move);
            if (move.DidCapture())
            {
                Piece destPiece = capturedPieces.Pop();
                Add(destPiece);
            }
            else
            {
                Set(source, null);
            }
            Update();
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
            Piece piece = Get(move.GetFirstSource());
            if (piece.IsPossible(move))
            {
                moves.Add(move);
                turn = turn.Other();
                ExecuteMove(move);
                Print();
                System.Console.WriteLine();
                System.Console.WriteLine(ToFen());
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
                Print();
                Console.WriteLine();
                System.Console.WriteLine(ToFen());
                return move;
            }
            return null;
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
                Add(new Pawn(new Location(i, 6), PieceColor.Black));
                Add(new Pawn(new Location(i, 1), PieceColor.White));
            }

            //Rook
            Add(new Rook(new Location(0, 7), PieceColor.Black));
            Add(new Rook(new Location(0, 0), PieceColor.White));
            Add(new Rook(new Location(7, 7), PieceColor.Black));
            Add(new Rook(new Location(7, 0), PieceColor.White));

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
            field[location.GetFile(), location.GetRank()] = piece;
        }

        public Piece Get(Location location)
        {
            if (location.GetFile() < 0 || location.GetFile() > 7 || location.GetRank() < 0 || location.GetRank() > 7)
            {
                return null;
            }
            return field[location.GetFile(), location.GetRank()];
        }

        public Piece Get(int file, int rank)
        {
            if (file < 0 || file > 7 || rank < 0 || rank > 7)
            {
                return null;
            }
            return field[file, rank];
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

            //whiteData.Update();
            //blackData.Update();
        }

        /*
        public HelperData GetWhiteData()
        {
            return whiteData;
        }

        public HelperData GetBlackData()
        {
            return blackData;
        }
        */
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
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                }
                if (printLocation)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
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

        public string ToFen()
        {
            StringBuilder builder = new StringBuilder();

            //Board
            for (int rank = 7; rank >= 0; rank--)
            {
                int empty = 0;
                for (int file = 0; file < 8; file++)
                {
                    Piece piece = Get(file, rank);
                    if (piece != null)
                    {
                        if (empty != 0)
                        {
                            builder.Append(empty);
                        }

                        char pieceChar = piece.GetPieceType().GetLetter(); //is upper case
                        if (piece.GetPieceColor() == PieceColor.Black)
                        {
                            pieceChar = Char.ToLower(pieceChar);
                        }
                        builder.Append(pieceChar);
                        empty = 0;
                    }
                    else
                    {
                        empty++;
                    }
                }
                if (empty != 0)
                {
                    builder.Append(empty);
                }
                if (rank != 0)
                {
                    builder.Append("/");
                }
            }
            builder.Append(" ");
            builder.Append(turn.ToChar());
            builder.Append(" ");

            //Check for castling
            bool isSomethingPossible = false;

            Piece whiteKingLocation = Get(new Location("e1"));
            if (whiteKingLocation != null && whiteKingLocation is King whiteKing && !whiteKing.WasMoved())
            {
                Piece whiteKingsidePiece = Get(new Location("h1"));
                if (whiteKingsidePiece is Rook whiteKingsideRook && !whiteKingsideRook.WasMoved())
                {
                    builder.Append("K");
                    isSomethingPossible = true;
                }

                Piece whiteQueensidePiece = Get(new Location("a1"));
                if (whiteQueensidePiece is Rook whiteQueensideRook && !whiteQueensideRook.WasMoved())
                {
                    builder.Append("Q");
                    isSomethingPossible = true;
                }
            }

            Piece blackKingLocation = Get(new Location("e1"));
            if (blackKingLocation != null && blackKingLocation is King blackKing && !blackKing.WasMoved())
            {

                Piece blackKingsidePiece = Get(new Location("h8"));
                if (blackKingsidePiece is Rook blackKingsideRook && !blackKingsideRook.WasMoved())
                {
                    builder.Append("k");
                    isSomethingPossible = true;
                }

                Piece blackQueensidePiece = Get(new Location("a8"));
                if (blackQueensidePiece is Rook blackQueensideRook && !blackQueensideRook.WasMoved())
                {
                    builder.Append("q");
                    isSomethingPossible = true;
                }
            }

            if (!isSomethingPossible)
            {
                builder.Append("-");
            }
            builder.Append(" ");

            //en pasant
            if (moves.Count != 0 && moves[moves.Count - 1] is AtomicMove lastMove && lastMove.IsDoublePush())
            {
                Location source = lastMove.GetSource();
                Location destination = lastMove.GetDestination();
                Location between = new Location(source.GetFile(), (source.GetRank() + destination.GetRank()) / 2);
                builder.Append(between.ToString());
            }
            else
            {
                builder.Append("-");
            }
            builder.Append(" ");
            builder.Append(halveMoveClock);
            builder.Append(" ");
            builder.Append(moves.Count / 2);

            return builder.ToString();
        }
    }
}