using System;
using Chess.src.core.moves;
using Chess.src.core.pieces;
using SFML.System;

namespace Chess.src.core.rendering
{
    public class Animation
    {
        private float speed;
        private float arg = 0f;
        private AtomicMove move;
        private Piece piece;
        private Clock clock = new Clock();
        private float tileSize;
        private Vector2f start;
        private Vector2f dir;
        private float length;

        public Animation(float speed, AtomicMove move, Piece piece, float tileSize)
        {
            this.speed = speed;
            this.move = move;
            this.piece = piece;
            this.tileSize = tileSize;

            start = new Vector2f(move.GetSource().GetFile() * tileSize, (7 - move.GetSource().GetRank()) * tileSize);
            dir = new Vector2f(move.GetDestination().GetFile() * tileSize, (7 - move.GetDestination().GetRank()) * tileSize) - start;
            length = (float)Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y) / tileSize;
        }

        public void Start()
        {
            clock.Restart();
        }

        public Vector2f Update()
        {
            arg = clock.ElapsedTime.AsSeconds() * 0.5f * (float)Math.PI / speed;
            float ratio = (float)Math.Sin(arg);
            if (ratio >= 1f)
            {
                ratio = 1f;
            }
            return start + dir * ratio;
        }

        public bool IsFinished()
        {
            return arg >= 0.5f * (float)Math.PI;
        }
        public Piece GetPiece()
        {
            return piece;
        }
    }
}