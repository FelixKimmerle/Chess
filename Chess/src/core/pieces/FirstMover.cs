using Chess.src.core.moves;

namespace Chess.src.core.pieces
{
    public abstract class FirstMover : Piece
    {
        private bool moved = false;
        private AtomicMove firstMove;
        protected FirstMover(Location location, PieceColor pieceColor, PieceType pieceType) : base(location, pieceColor, pieceType)
        {
        }

        public bool WasMoved()
        {
            return moved;
        }

        public override void Do(AtomicMove move)
        {
            base.Do(move);
            if(!moved)
            {
                firstMove = move;
                moved = true;
            }
        }

        public override void Undo(AtomicMove move)
        {
            base.Undo(move);
            if(move == firstMove)
            {
                moved = false;
                firstMove = null;
            }
        }
    }
}