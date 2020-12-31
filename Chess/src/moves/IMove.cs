using static Chess.src.pieces.Piece;

namespace Chess.src.moves
{
    public interface IMove
    {
        void ExecuteMove(Field field);
        IMove Reverse();

        BoardLocation GetDestination();
        BoardLocation GetStart();

    }
}