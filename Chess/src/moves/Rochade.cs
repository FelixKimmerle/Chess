using Chess.src.pieces;
using static Chess.src.pieces.Piece;

namespace Chess.src.moves
{
    public class Rochade : IMove
    {
        private AtomicMove first;
        private AtomicMove second;

        public Rochade(AtomicMove first, AtomicMove second)
        {
            this.first = first;
            this.second = second;
        }

        public Rochade(bool shortRochade, PieceColor color)
        {
            int y = 0;
            if (color == PieceColor.Black)
            {
                y = 7;
            }
            if (shortRochade)
            {
                first = new AtomicMove(PieceType.King, new BoardLocation(4, y), new BoardLocation(6, y));
                second = new AtomicMove(PieceType.Rook, new BoardLocation(7, y), new BoardLocation(5, y));
            }
            else
            {
                first = new AtomicMove(PieceType.King, new BoardLocation(4, y), new BoardLocation(2, y));
                second = new AtomicMove(PieceType.Rook, new BoardLocation(0, y), new BoardLocation(3, y));
            }
        }
        public Rochade(string str, PieceColor color)
        {
            int y = 0;
            if (color == PieceColor.Black)
            {
                y = 7;
            }

            if (str.Length == 3)
            {
                //Short
                first = new AtomicMove(PieceType.King, new BoardLocation(4, y), new BoardLocation(6, y));
                second = new AtomicMove(PieceType.Rook, new BoardLocation(7, y), new BoardLocation(5, y));
            }
            else
            {
                //Long
                first = new AtomicMove(PieceType.King, new BoardLocation(4, y), new BoardLocation(2, y));
                second = new AtomicMove(PieceType.Rook, new BoardLocation(0, y), new BoardLocation(3, y));
            }
        }
        public void ExecuteMove(Field field)
        {
            field.ExecuteAtomicMove(first);
            field.ExecuteAtomicMove(second);
        }

        public override string ToString()
        {
            if (first.GetStart().EulerDistance(second.GetStart()) == 3)
            {
                return "0-0";
            }
            else
            {
                return "0-0-0";
            }
        }

        public IMove Reverse()
        {
            return new Rochade(second.AtomicReverse(), first.AtomicReverse());
        }

        public BoardLocation GetDestination()
        {
            return first.GetDestination();
        }

        public BoardLocation GetStart()
        {
            return first.GetStart();
        }

        public override bool Equals(object obj)
        {
            if (obj is Rochade other)
            {
                return first.Equals(other.first) && second.Equals(other.second);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return first.GetHashCode() + second.GetHashCode();
        }
    }
}