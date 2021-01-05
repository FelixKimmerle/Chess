using System.Collections.Generic;

namespace Chess.src.core.moves
{
    public abstract class Move
    {
        protected bool capture = false;

        public bool DidCapture()
        {
            return capture;
        }

        public void Capture()
        {
            this.capture = true;
        }

        public Location GetFirstSource()
        {
            return GetAtomicMoves()[0].GetSource();
        }

        public Location GetFirstDestination()
        {
            return GetAtomicMoves()[0].GetDestination();
        }

        public List<AtomicMove> Reverse()
        {
            List<AtomicMove> moves = new List<AtomicMove>();

            foreach (AtomicMove atomicMove in GetAtomicMoves())
            {
                moves.Add(atomicMove.ReverseAtomic());
            }

            moves.Reverse();
            return moves;
        }
        public abstract List<AtomicMove> GetAtomicMoves();
    }
}