using System.Collections.Generic;
using Chess.src.core.pieces;

namespace Chess.src.core.moves
{
    public class AtomicMove : Move
    {
        private readonly PieceType piece;
        private readonly Location source;
        private readonly Location destination;

        public AtomicMove(PieceType piece, Location from, Location to)
        {
            this.piece = piece;
            this.source = from;
            this.destination = to;
        }

        public AtomicMove(string source)
        {
            string[] splitted = source.Trim().Split(new char[] { ' ', '-' });
            piece = PieceType.Pawn;
            int offset = 0;
            if (splitted.Length == 3)
            {
                piece = PieceTypeExtension.GetPiece(char.ToUpper(splitted[0][0]));
                offset = 1;
            }
            this.source = new Location(splitted[0 + offset]);
            destination = new Location(splitted[1 + offset]);
        }

        public bool IsDoublePush()
        {
            return piece == PieceType.Pawn && source.EulerDistance(destination) == 2;
        }

        public Location GetSource()
        {
            return source;
        }

        public Location GetDestination()
        {
            return destination;
        }


        public AtomicMove ReverseAtomic()
        {
            return new AtomicMove(piece,destination,source);
        }

        public override List<AtomicMove> GetAtomicMoves()
        {
            return new List<AtomicMove>() { this };
        }

        public override bool Equals(object obj)
        {
            if (obj is AtomicMove other)
            {
                return piece == other.piece && source.Equals(other.source) && destination.Equals(other.destination);
            }
            return false;
        }

        public override string ToString()
        {
            return piece.GetLetter() + " " + source + "-" + destination;
        }

        public override int GetHashCode()
        {
            return piece.GetHashCode() + source.GetHashCode() + destination.GetHashCode();
        }
    }
}