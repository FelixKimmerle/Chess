using System;
using System.Collections.Generic;
using Chess.src.core.moves;
using Chess.src.core.pieces;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Chess.src.core.rendering
{
    public class GameWidget : Drawable
    {
        private readonly Position position = new Position();
        private readonly Sprite[] sprites = new Sprite[12];
        private readonly Sprite blankLight;
        private readonly Sprite blankDark;
        private float tileSize;
        private Vector2f offset;
        private Piece selectedPiece = null;
        private Piece pickedPiece = null;
        private readonly List<Arrow> arrows = new List<Arrow>();
        private Vector2f mousePos = new Vector2f(0, 0);
        private Location startMove;
        private Text text;
        //private HelperData whiteData;
        //private HelperData blackData;
        Font font = new Font("res/arial.ttf");
        RectangleShape rectangleShape = new RectangleShape();
        CircleShape circleShape = new CircleShape();

        //PredicitionWidget predicition;

        HashSet<Piece> animatedPieces = new HashSet<Piece>();

        List<Animation> animations = new List<Animation>();
        public void Resize(Vector2f size)
        {
            tileSize = Math.Min(size.X / 8f, size.Y / 8f);
            //predicition.Resize(new Vector2f(tileSize / 4f, tileSize * 8));

            rectangleShape.Size = new Vector2f(tileSize * 0.5f, tileSize * 0.5f);
            rectangleShape.FillColor = new Color(255, 0, 0, 100);
            rectangleShape.Origin = rectangleShape.Size / 2f;

            circleShape.Radius = tileSize * 0.2f;
            circleShape.FillColor = new Color(255, 200, 00, 200);
            circleShape.Origin = new Vector2f(circleShape.Radius, circleShape.Radius);

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

            text = new Text("", font, (uint)(tileSize / 6 + 5));
            text.FillColor = new Color(200, 200, 200);

            text.Scale = new Vector2f(0.7f, 0.7f);
        }
        public GameWidget(Vector2f size)
        {
            //board = game.GetBoard();
            //whiteData = board.GetWhiteData();
            //blackData = board.GetBlackData();

            for (int i = 0; i < 6; i++)
            {
                Texture whiteTexture = new Texture("res/128px/w_" + ((PieceType)i).ToString().ToLower() + "_png_shadow_128px.png");
                whiteTexture.Smooth = true;
                sprites[i * 2] = new Sprite(whiteTexture);

                Texture blackTexture = new Texture("res/128px/b_" + ((PieceType)i).ToString().ToLower() + "_png_shadow_128px.png");
                blackTexture.Smooth = true;

                sprites[i * 2 + 1] = new Sprite(blackTexture);
            }

            blankLight = new Sprite(new Texture("res/128px/square brown light_png_shadow_128px.png"));
            blankDark = new Sprite(new Texture("res/128px/square brown dark_png_shadow_128px.png"));

            //blankLight = new Sprite(new Texture(new Image(1,1,new Color(255,255,255))));
            //blankDark = new Sprite(new Texture(new Image(1,1,new Color(100,100,100))));

            Resize(size);

        }

        private int GetId(int x, int y)
        {
            return y * 8 + x;
        }

        private int GetId(Location location)
        {
            return (7 - location.GetRank()) * 8 + location.GetFile();
        }

        public void MouseMove(Vector2f position)
        {
            mousePos = position - offset;
        }

        public void MouseButtonPressed(Vector2f location, Mouse.Button button)
        {
            location = location - offset;
            int x = (int)(location.X / tileSize);
            int y = (int)(location.Y / tileSize);

            Piece piece = position.Get(x, 7 - y);
            if (piece != null)
            {
                if (button == Mouse.Button.Middle)
                {

                    SelectPiece(piece);
                }
                else if (button == Mouse.Button.Left)
                {
                    if (piece.GetPieceColor() == position.GetTurn())
                    {
                        startMove = new Location(x, 7 - y);
                        pickedPiece = piece;
                        SelectPiece(pickedPiece);
                    }
                }
            }
        }

        public void MouseButtonReleased(Vector2f location, Mouse.Button button)
        {
            location = location - offset;
            int x = (int)(location.X / tileSize);
            int y = (int)(location.Y / tileSize);

            Location destination = new Location(x, 7 - y);
            if (button == Mouse.Button.Left && pickedPiece != null && !startMove.Equals(destination))
            {
                Move move;
                if (pickedPiece.GetPieceType() == PieceType.King && position.Get(destination) == null && startMove.EulerDistance(destination) > 1 && pickedPiece.GetLocation().GetRank() == destination.GetRank())
                {
                    //Rochade
                    if (startMove.GetFile() < destination.GetFile())
                    {
                        //Small
                        move = new KingsideCastling(position.GetTurn());
                    }
                    else
                    {
                        //Big
                        move = new QueensideCastling(position.GetTurn());
                    }
                }
                else
                {
                    move = new AtomicMove(pickedPiece.GetPieceType(), startMove, destination);
                }
                System.Console.WriteLine("Trigger move");
                if (position.Do(move))
                {
                    AnimateMove(pickedPiece, move);
                }

                SelectPiece(pickedPiece);
                pickedPiece = null;
            }
            else
            {
                pickedPiece = null;
            }
        }

        private void AnimateMove(Piece piece, Move move, bool reversed = false)
        {
            List<AtomicMove> atomicMoves = reversed ? move.Reverse() : move.GetAtomicMoves();
            foreach (AtomicMove atomicMove in atomicMoves)
            {
                Piece atomicPiece = position.Get(atomicMove.GetDestination());
                animatedPieces.Add(atomicPiece);
                animations.Add(new Animation(0.5f, atomicMove, atomicPiece, tileSize));
            }
        }

        private void SelectPiece(Piece piece)
        {
            arrows.Clear();
            selectedPiece = piece;
            if (piece != null)
            {
                foreach (Move move in piece.GetPossibleMoves())
                {
                    Vector2f source = new Vector2f(move.GetFirstSource().GetFile() * tileSize, (7 - move.GetFirstSource().GetRank()) * tileSize);
                    Vector2f destination = new Vector2f(move.GetFirstDestination().GetFile() * tileSize, (7 - move.GetFirstDestination().GetRank()) * tileSize);
                    arrows.Add(new Arrow(10, source, destination, 30, 0));
                    arrows[arrows.Count - 1].FillColor = new Color(0, 128, 0);
                }
            }
        }

        public void KeyPressed(KeyEventArgs e)
        {
            if (e.Control && e.Code == Keyboard.Key.Z)
            {
                Move undoMove = position.Undo();
                if (undoMove != null)
                {
                    AnimateMove(position.Get(undoMove.GetFirstSource()), undoMove, true);
                }
                SelectPiece(selectedPiece);
            }
        }

        public void DrawPiece(Piece piece, Vector2f screenPosition, RenderTarget target, RenderStates states, bool selected = false, bool picked = false)
        {
            PieceColor pieceColor = piece.GetPieceColor();
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

            if (pieceColor == position.GetTurn())
            {
                sprite.Color = new Color(255, 255, 200);
            }

            sprite.Position = screenPosition;

            RenderStates pieceStates = new RenderStates(states);
            if (!picked)
            {
                pieceStates.Transform.Translate(tileSize / 2f, tileSize / 2f);
            }
            target.Draw(sprite, pieceStates);
        }

        private Color mix(Color a, Color b)
        {
            return new Color((byte)(a.R - (a.R - b.R) / 2), (byte)(a.G - (a.G - b.G) / 2), (byte)(a.B - (a.B - b.B) / 2));
            //return new Color(Math.Max(a.R,b.R), Math.Max(a.G,b.G),Math.Max(a.B,b.B));
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Translate(offset);

            RenderStates shapeStates = new RenderStates(states);
            shapeStates.Transform.Translate(tileSize / 2f, tileSize / 2f);

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Location location = new Location(x, 7 - y);
                    Piece piece = position.Get(location);
                    Vector2f screenPosition = new Vector2f(x * tileSize, y * tileSize);
                    Sprite sprite;
                    if ((x + y % 2) % 2 == 0)
                    {
                        sprite = blankLight;
                    }
                    else
                    {
                        sprite = blankDark;
                    }

                    sprite.Color = Color.White;



                    bool drawRectangle = false;
                    bool drawCircle = false;

                    rectangleShape.Position = screenPosition;
                    circleShape.Position = screenPosition;

                    /*
                    if (whiteData.Attacks(location))
                    {
                        if (piece != null)
                        {
                            if (game.GetTurn().IsWhite())
                            {
                                sprite.Color = Color.Cyan;
                            }
                            else
                            {
                                sprite.Color = Color.Red;
                            }
                        }
                        else
                        {
                            if (game.GetTurn().IsBlack())
                            {
                                //drawRectangle = true;
                                sprite.Color = Color.Magenta;
                            }
                            else
                            {
                                //drawCircle = true;
                                sprite.Color = Color.Yellow;
                            }
                        }
                    }

                    if (blackData.Attacks(location))
                    {
                        if (piece != null)
                        {
                            if (game.GetTurn().IsBlack())
                            {
                                sprite.Color = Color.Cyan;
                            }
                            else
                            {
                                sprite.Color = Color.Red;
                            }
                        }
                        else
                        {
                            if (game.GetTurn().IsWhite())
                            {
                                //drawRectangle = true;
                                if (sprite.Color != Color.White)
                                {
                                    sprite.Color = new Color(255, 165, 0);
                                }
                                else
                                {
                                    sprite.Color = Color.Magenta;
                                }
                            }
                            else
                            {
                                //drawCircle = true;
                                if (sprite.Color != Color.White)
                                {
                                    sprite.Color = new Color(255, 165, 0);
                                }
                                else
                                {
                                    sprite.Color = Color.Yellow;
                                }
                            }

                        }
                    }
                    /*
                    if (coloredLocations.Contains(location))
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
                    */

                    sprite.Position = screenPosition;
                    target.Draw(sprite, states);

                    if (drawRectangle)
                    {
                        target.Draw(rectangleShape, shapeStates);
                    }

                    if (drawCircle)
                    {
                        target.Draw(circleShape, shapeStates);
                    }



                    text.DisplayedString = location.ToString();
                    text.Position = screenPosition + new Vector2f(tileSize / 20, tileSize / 20);
                    target.Draw(text, states);

                }
            }

            foreach (Arrow arrow in arrows)
            {
                target.Draw(arrow, shapeStates);
            }

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Location location = new Location(x, 7 - y);
                    Piece piece = position.Get(location);
                    Vector2f screenPosition = new Vector2f(x * tileSize, y * tileSize);
                    if (piece != null && piece != pickedPiece && (!animatedPieces.Contains(piece)))
                    {
                        DrawPiece(piece, screenPosition, target, states, selectedPiece == piece);
                    }
                }

            }

            if (pickedPiece != null)
            {
                DrawPiece(pickedPiece, mousePos, target, states, false, true);
            }



            foreach (Animation animation in animations)
            {
                DrawPiece(animation.GetPiece(), animation.Update(), target, states, selectedPiece == animation.GetPiece());
                if (animation.IsFinished())
                {
                    animatedPieces.Remove(animation.GetPiece());
                }
            }
            animations.RemoveAll(item => item.IsFinished());
            states.Transform.Translate(tileSize * 8, 0);
        }
    }
}