using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.src.core.rendering
{
    class Arrow : Drawable
    {
        Body body;
        Top top;

        public Color FillColor { get { return body.FillColor; } set { body.FillColor = value; top.FillColor = value; } }
        public Color OutlineColor { get { return body.OutlineColor; } set { body.OutlineColor = value; top.OutlineColor = value; } }
        public float OutlineThickness { get { return body.OutlineThickness; } set { body.OutlineThickness = value; top.OutlineThickness = value; } }

        class Body : Shape
        {
            Vector2f[] vertices;
            public Body(Vector2f[] vertices)
            {
                this.vertices = vertices;
                OutlineThickness = 0;
            }
            public override Vector2f GetPoint(uint index)
            {
                return vertices[index];
            }

            public override uint GetPointCount()
            {
                return 4;
            }
        }

        class Top : Shape
        {
            Vector2f[] vertices;

            public Top(Vector2f[] vertices)
            {
                this.vertices = vertices;
                OutlineThickness = 0;
            }
            public override Vector2f GetPoint(uint index)
            {
                return vertices[index];
            }

            public override uint GetPointCount()
            {
                return 3;
            }
        }

        public Arrow(float thickness, Vector2f point1, Vector2f point2, float arrowlenth, float arrowoffset = 0)
        {
            Vector2f direction = point2 - point1;
            float length = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            Vector2f unitDirection = new Vector2f(direction.X / length, direction.Y / length);
            Vector2f unitPerpendicular = new Vector2f(-unitDirection.Y, unitDirection.X);

            Vector2f offset = (thickness / 2.0f) * unitPerpendicular;

            point2 = point2 - unitDirection * arrowoffset;

            Vector2f endpoint = point2 - unitDirection * arrowlenth;

            body = new Body(new Vector2f[4] { point1 + offset, endpoint + offset, endpoint - offset, point1 - offset });
            top = new Top(new Vector2f[3] { endpoint + offset * 3, point2, endpoint - offset * 3 });
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(body,states);
            target.Draw(top,states);
        }
    }
}
