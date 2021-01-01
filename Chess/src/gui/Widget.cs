using SFML.Graphics;

namespace Chess.src.gui
{
    abstract public class Widget : Drawable
    {
        abstract public void Draw(RenderTarget target, RenderStates states);
    }
}