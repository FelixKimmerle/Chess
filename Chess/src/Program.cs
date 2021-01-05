using System;
using Chess.src.core;
using Chess.src.core.moves;
using Chess.src.core.rendering;

namespace Chess.src
{
    class Program
    {
        static void Main(string[] args)
        {
            ChessWindow window = new ChessWindow();
            window.Run();
            /*
            Game game = new Game();
            string line = "";
            while (line != "Q")
            {
                Console.Write("> ");
                line = Console.ReadLine();

                if (line == "Q")
                {
                    break;
                }

                string[] splitted = line.Split(' ');
                if (splitted[0] == "clear")
                {
                    game.Reset();
                }
                else if (splitted[0] == "undo")
                {
                    game.Undo();
                }
                else
                {
                    Move move = game.Parse(line);
                    game.Do(move);
                }

            }
            */

        }

    }
}
