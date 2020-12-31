using Chess.src.rendering;
using System;

namespace Chess.src
{
    class Program
    {
        static void Main(string[] args)
        {
            Field field = new Field();
            field.Print();
            ChessWindow chessWindow = new ChessWindow();
            chessWindow.Run();
        }

    }
}
