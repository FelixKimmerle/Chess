using SFML.Graphics;
using SFML.System;

namespace Chess.src.core.rendering
{
    public class PredicitionWidget : Drawable
    {
        private RectangleShape outline = new RectangleShape();
        private RectangleShape valueRect = new RectangleShape();

        private float value = 0f;
        private Vector2f size;

        public PredicitionWidget(Vector2f size)
        {
            this.size = size;
            Resize(size);
            outline.FillColor = Color.Black;
            outline.OutlineColor = new Color(200, 200, 200);
            outline.OutlineThickness = -2;
            valueRect.FillColor = Color.White;
        }

        public void Resize(Vector2f size)
        {
            this.size = size;
            outline.Size = size;
            valueRect.Size = new Vector2f(size.X - 4, size.Y * value - 4);
            valueRect.Origin = new Vector2f(valueRect.Size.X * 0.5f, 0);
            valueRect.Position = new Vector2f(size.X * 0.5f, size.Y - valueRect.Size.Y - 2);
        }

        public void SetValue(float value)
        {
            value = value + 0.5f;
            this.value = value;
            valueRect.Size = new Vector2f(size.X - 4, size.Y * value - 4);
            valueRect.Position = new Vector2f(size.X * 0.5f, size.Y - valueRect.Size.Y - 2);
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(outline, states);
            target.Draw(valueRect, states);
        }
    }
}