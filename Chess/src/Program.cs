using System;
using Chess.src.core;
using Chess.src.core.moves;
using Chess.src.core.rendering;
using ImGuiNET;
using SFML.System;

namespace Chess.src
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press ESC key to close window");
            var window = new SimpleWindow();
            window.Run();

            Console.WriteLine("All done");
            //ChessWindow window = new ChessWindow();
            //window.Run();
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
        class SimpleWindow
        {
            public void Run()
            {
                var mode = new SFML.Window.VideoMode(800, 600);
                var window = new SFML.Graphics.RenderWindow(mode, "SFML works!");

                GuiImpl.Init(window);


                window.KeyPressed += Window_KeyPressed;

                var circle = new SFML.Graphics.CircleShape(100f)
                {
                    FillColor = SFML.Graphics.Color.Blue
                };
                Clock clock = new Clock();
                // Start the game loop
                while (window.IsOpen)
                {
                    // Process events
                    window.DispatchEvents();
                    GuiImpl.Update(window, clock.Restart());

                    ImGui.Begin("Hello, world!");
                    ImGui.Button("Look at this pretty button");
                    ImGui.End();

                    window.Clear();
                    window.Draw(circle);
                    GuiImpl.Render();
                    // Finally, display the rendered frame on screen
                    window.Display();
                }
                GuiImpl.Shutdown();
            }

            /// <summary>
            /// Function called when a key is pressed
            /// </summary>
            private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
            {
                var window = (SFML.Window.Window)sender;
                if (e.Code == SFML.Window.Keyboard.Key.Escape)
                {
                    window.Close();
                }
            }
        }

    }
}
