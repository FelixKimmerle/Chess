using System;
using System.Collections.Generic;
using System.Text;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Chess.src.rendering
{
    class ChessWindow
    {
        private RenderGame renderGame;
        public void Run()
        {
            ContextSettings contextSettings = new ContextSettings(64,64,16);
            VideoMode mode = new VideoMode(800, 800);
            RenderWindow window = new RenderWindow(mode, "Chess", Styles.Default);
            window.SetVerticalSyncEnabled(true);
            window.SetFramerateLimit(241);

            window.KeyPressed += Window_KeyPressed;
            window.Closed += OnClosed;
            window.Resized += OnResize;
            window.MouseButtonPressed += OnMouseButtonPressed;
            window.MouseButtonReleased += OnMouseButtonReleased;
            window.MouseMoved += OnMouseMove;

            renderGame = new RenderGame(new SFML.System.Vector2f(800,800));

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.Black);
                window.Draw(renderGame);
                window.Display();
            }
        }

        private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        private void OnResize(object sender, SizeEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
            renderGame.Resize(new SFML.System.Vector2f(e.Width, e.Height));
        }

        private void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            renderGame.MouseButtonPressed(new SFML.System.Vector2f(e.X, e.Y), e.Button);
        }

        private void OnMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            renderGame.MouseButtonReleased(new SFML.System.Vector2f(e.X, e.Y), e.Button);
        }

        private void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            renderGame.MouseMove(new SFML.System.Vector2f(e.X, e.Y));
        }
    }
}
