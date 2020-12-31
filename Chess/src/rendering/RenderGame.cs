using Chess.src.pieces;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.src.rendering
{
    class RenderGame : Drawable
    {
        private readonly Game game = new Game();
        private readonly Field field;
        private readonly Sprite[] sprites = new Sprite[12];
        private readonly Sprite blankLight;
        private readonly Sprite blankDark;
        private float tileSize;
        private Vector2f offset;
        private Piece selectedPiece = null;
        private Piece pickedPiece = null;
        private readonly HashSet<int> coloredIndices = new HashSet<int>();
        private Vector2f mousePos = new Vector2f(0, 0);
        private BoardLocation startMove;

        public void Resize(Vector2f size)
        {
            tileSize = Math.Min(size.X, size.Y) / 8;

            if (size.X > size.Y)
            {
                offset = new Vector2f((size.X - size.Y) / 2, 0);
            }
            else
            {
                offset = new Vector2f(0, (size.Y - size.X) / 2);
            }

            for (int i = 0; i < 12; i++)
            {
                sprites[i].Scale = new Vector2f(tileSize / sprites[i].TextureRect.Width * 0.7f, tileSize / sprites[i].TextureRect.Height * 0.7f);
                sprites[i].Origin = new Vector2f(sprites[i].TextureRect.Width / 2, sprites[i].TextureRect.Width / 2);

                sprites[i].Texture.Smooth = true;
            }
            blankLight.Scale = new Vector2f(tileSize / blankLight.TextureRect.Width, tileSize / blankLight.TextureRect.Height);
            blankDark.Scale = new Vector2f(tileSize / blankDark.TextureRect.Width, tileSize / blankDark.TextureRect.Height);
        }
        public RenderGame(Vector2f size)
        {
            field = game.GetField();

            for (int i = 0; i < 6; i++)
            {
                sprites[i * 2] = new Sprite(new Texture("res/128px/w_" + ((PieceType)i).ToString().ToLower() + "_png_shadow_128px.png"));
                sprites[i * 2 + 1] = new Sprite(new Texture("res/128px/b_" + ((PieceType)i).ToString().ToLower() + "_png_shadow_128px.png"));
            }

            blankLight = new Sprite(new Texture("res/128px/square brown light_png_shadow_128px.png"));
            blankDark = new Sprite(new Texture("res/128px/square brown dark_png_shadow_128px.png"));

            Resize(size);

        }




        private int GetId(int x, int y)
        {
            return y * 8 + x;
        }

        private int GetId(BoardLocation location)
        {
            return (7 - location.GetY()) * 8 + location.GetX();
        }

        public void MouseMove(Vector2f position)
        {
            mousePos = position - offset;
        }

        public void MouseButtonPressed(Vector2f position, Mouse.Button button)
        {
            position = position - offset;
            int x = (int)(position.X / tileSize);
            int y = (int)(position.Y / tileSize);

            Piece piece = field.Get(x, 7 - y);
            if (piece != null)
            {
                if (button == Mouse.Button.Middle)
                {

                    SelectPiece(piece);
                }
                else if (button == Mouse.Button.Left)
                {
                    if (piece.GetPieceColor() == game.WhoIsOnTheTurn())
                    {
                        startMove = new BoardLocation(x, 7 - y);
                        pickedPiece = piece;
                        SelectPiece(pickedPiece);
                    }
                }
            }
        }

        public void MouseButtonReleased(Vector2f position, Mouse.Button button)
        {
            position = position - offset;
            int x = (int)(position.X / tileSize);
            int y = (int)(position.Y / tileSize);

            if (button == Mouse.Button.Left && pickedPiece != null)
            {
                Move move = new Move(pickedPiece.GetPieceType(), startMove, new BoardLocation(x, 7 - y));
                if (pickedPiece.IsPossible(move, field))
                {
                    game.ExecuteMove(move);
                }
                SelectPiece(pickedPiece);
                pickedPiece = null;
            }
        }

        private void SelectPiece(Piece piece)
        {
            coloredIndices.Clear();
            selectedPiece = piece;
            if (piece != null)
            {
                foreach (Move move in piece.GetPossibleMoves(field))
                {
                    coloredIndices.Add(GetId(move.GetDestination()));
                }
            }
        }

        public void DrawPiece(Piece piece, Vector2f position, RenderTarget target, RenderStates states, bool selected = false, bool picked = false)
        {
            Piece.PieceColor pieceColor = piece.GetPieceColor();
            PieceType pieceType = piece.GetPieceType();
            Sprite sprite = sprites[(int)pieceType * 2 + (int)pieceColor];

            if (selected)
            {
                sprite.Color = Color.Cyan;
            }
            else
            {
                sprite.Color = Color.White;
            }

            sprite.Position = position;

            RenderStates pieceStates = new RenderStates(states);
            if (!picked)
            {
                pieceStates.Transform.Translate(tileSize / 2f, tileSize / 2f);
            }
            target.Draw(sprite, pieceStates);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Translate(offset);
            int counter = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Piece piece = field.Get(x, 7 - y);
                    Vector2f position = new Vector2f(x * tileSize, y * tileSize);
                    Sprite sprite;
                    if ((x + y % 2) % 2 == 0)
                    {
                        sprite = blankLight;
                    }
                    else
                    {
                        sprite = blankDark;
                    }

                    if (coloredIndices.Contains(counter))
                    {
                        if (piece != null)
                        {
                            sprite.Color = Color.Red;
                        }
                        else
                        {
                            sprite.Color = Color.Green;
                        }
                    }
                    else
                    {
                        sprite.Color = Color.White;
                    }

                    sprite.Position = position;
                    target.Draw(sprite, states);

                    if (piece != null && piece != pickedPiece)
                    {
                        DrawPiece(piece, position, target, states, selectedPiece == piece);
                    }
                    counter++;
                }
            }
            if (pickedPiece != null)
            {
                DrawPiece(pickedPiece, mousePos, target, states, false, true);
            }
        }
    }
}
