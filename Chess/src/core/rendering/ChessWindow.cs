using System;
using SFML.Graphics;
using SFML.Window;

namespace Chess.src.core.rendering
{
    public class ChessWindow
    {
        GameWidget gameWidget;
        public void Run()
        {
            ContextSettings contextSettings = new ContextSettings(64,64,4);
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
            window.KeyPressed += OnKeyPressed;

            gameWidget = new GameWidget(new SFML.System.Vector2f(800,800));

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.Black);
                window.Draw(gameWidget);
                window.Display();
            }
        }

        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            gameWidget.KeyPressed(e);
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
            gameWidget.Resize(new SFML.System.Vector2f(e.Width, e.Height));
        }

        private void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            gameWidget.MouseButtonPressed(new SFML.System.Vector2f(e.X, e.Y), e.Button);
        }

        private void OnMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            gameWidget.MouseButtonReleased(new SFML.System.Vector2f(e.X, e.Y), e.Button);
        }

        private void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            gameWidget.MouseMove(new SFML.System.Vector2f(e.X, e.Y));
        }
    }
}