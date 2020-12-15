namespace Quixo.Core.Players
{
    public class HumanPlayer : Player
    {
        public override bool IsHuman => true;
        
        public HumanPlayer(int id, string name, PieceType pieceType)
            : base(id, name, pieceType)
        {
        }

        public override bool PlayTurn(QuixoBoard board)
        {
            IsInputRequired = !IsInputRequired;

            return !IsInputRequired;
        }
    }
}
