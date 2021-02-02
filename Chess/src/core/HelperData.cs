using System.Collections.Generic;
using Chess.src.core.moves;
using Chess.src.core.pieces;

namespace Chess.src.core
{
    public class HelperData
    {
        Position position;

        private HashSet<Location> attacking = new HashSet<Location>();
        private HashSet<Piece> piecesAttacking = new HashSet<Piece>();

        private HashSet<Piece> pieces;
        private PieceColor color;
        private float points = 0f;

        static public float Ratio(HelperData white, HelperData black)
        {
            float whiteBonus = 0f;
            foreach (Piece piece in white.piecesAttacking)
            {
                whiteBonus += piece.GetPieceType().GetPoints() / black.pieces.Count;
            }
            whiteBonus *= 2f;
            whiteBonus += white.attacking.Count / 64f;

            float blackBonus = 0f;
            foreach (Piece piece in black.piecesAttacking)
            {
                blackBonus += piece.GetPieceType().GetPoints() / white.pieces.Count;
            }
            whiteBonus *= 2f;
            blackBonus += black.attacking.Count / 64f;


            return ((white.points + whiteBonus) / 42f) - ((black.points + blackBonus) / 42f);
        }

        public HelperData(Position position, PieceColor color)
        {
            this.color = color;
            this.position = position;
            if (color.IsWhite())
            {
                this.pieces = position.GetWhitePieces();
            }
            else
            {
                this.pieces = position.GetBlackPieces();
            }
        }

        public void Update()
        {
            UpdateAttacking();
            UpdatePoints();
        }

        public bool Attacks(Location location)
        {
            return attacking.Contains(location);
        }

        private void UpdateAttacking()
        {
            attacking.Clear();
            piecesAttacking.Clear();
            foreach (Piece piece in pieces)
            {
                HashSet<Move> possibleMoves = piece.GetPossibleMoves();

                foreach (Move move in possibleMoves)
                {
                    List<AtomicMove> atomicMoves = move.GetAtomicMoves();

                    foreach (AtomicMove atomicMove in atomicMoves)
                    {
                        Location destination = atomicMove.GetDestination();
                        attacking.Add(destination);
                        if (position.IsEnemy(destination, color))
                        {
                            piecesAttacking.Add(position.Get(destination));
                        }
                    }
                }
            }
        }

        private void UpdatePoints()
        {
            points = 0;
            foreach (Piece piece in pieces)
            {
                points += piece.GetPieceType().GetPoints();
            }

        }


    }
}