using Chess.src.pieces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;

namespace Chess.src
{
    public class Game
    {
        private Field field = new Field();
        private List<Move> whiteMoves = new List<Move>();
        private List<Move> blackMoves = new List<Move>();
        private Piece.PieceColor turn = Piece.PieceColor.White;

        public Game()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string line = (string)e.Result;
            string[] splitted = line.Split(' ');
            if (splitted[0] == "save" && splitted.Length == 2)
            {
                Save(splitted[1]);
            }
            else
            {
                Move move = new Move(line);
                Piece piece = field.Get(move.GetStart());
                if (piece != null && piece.IsPossible(move, field) && piece.GetPieceColor() == turn)
                {
                    ExecuteMove((Move)e.Result);
                }
            }

        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.Write("> ");
            string line = Console.ReadLine();
            e.Result = line;
        }

        public Field GetField()
        {
            return field;
        }

        public Piece.PieceColor WhoIsOnTheTurn()
        {
            return turn;
        }

        public void ExecuteMove(Move move)
        {
            if (turn == Piece.PieceColor.White)
            {
                whiteMoves.Add(move);
                turn = Piece.PieceColor.Black;
            }
            else
            {
                blackMoves.Add(move);
                turn = Piece.PieceColor.White;
            }
            field.ExecuteMove(move);
            Console.WriteLine(move);
            field.Print();
        }

        public void Save(string filename)
        {
            StreamWriter file = new StreamWriter(filename);
            for (int i = 0; i < Math.Max(whiteMoves.Count,blackMoves.Count); i++)
            {
                if (i < whiteMoves.Count)
                {
                    file.Write(whiteMoves[i].ToString());
                }
                file.Write(" # ");
                if (i < blackMoves.Count)
                {
                    file.Write(blackMoves[i].ToString());
                }
                file.WriteLine();
            }
            file.Close();
        }

    }
}
