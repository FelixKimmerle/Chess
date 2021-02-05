using System;
using Chess.src.core;
using Chess.src.core.moves;
using Chess.src.core.rendering;
using ImGuiNET;
using SFML.System;
using SFML.Graphics;

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




                window.KeyPressed += Window_KeyPressed;

                var circle = new SFML.Graphics.CircleShape(100f)
                {
                    FillColor = SFML.Graphics.Color.Blue
                };
                Clock clock = new Clock();


                window.SetActive(true);
                //window.PushGLStates();
                GuiImpl.Init(window);
                //window.PopGLStates();
                window.SetActive(false);


                Image image = new Image(100, 100, Color.Red);
                Texture texture = new Texture(image);
                // Start the game loop
                while (window.IsOpen)
                {
                    // Process events
                    window.DispatchEvents();
                    window.SetActive(true);
                    //window.PushGLStates();

                    GuiImpl.Update(window, clock.Restart());
                    ImGui.BeginMainMenuBar();
                    if (ImGui.BeginMenu("File"))
                    {
                        ImGui.MenuItem("Open");
                        ImGui.EndMenu();
                    }
                    ImGui.EndMainMenuBar();

                    ImGui.Begin("Hello, world!");
                    if (ImGui.Button("Look at this pretty button"))
                    {
                        System.Console.WriteLine("Hi!");
                    }

                    //System.Console.WriteLine(texture.NativeHandle);
                    ImGui.Image(new IntPtr(texture.NativeHandle), new System.Numerics.Vector2(200, 200));
                    ImGui.End();

                    ImGui.Begin("Hello, world!2");
                    if (ImGui.Button("Look at this pretty button22"))
                    {
                        System.Console.WriteLine("Hi2!");
                    }
                    ImGui.Text("Hallo");
                    ImGui.End();
                    ImGui.Button("lll");

                    //window.PopGLStates();
                    window.SetActive(false);


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
