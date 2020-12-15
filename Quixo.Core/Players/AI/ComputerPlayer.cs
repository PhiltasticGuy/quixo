namespace Quixo.Core.Players.AI
{
    public abstract class ComputerPlayer : Player
    {
        public override bool IsHuman => false;

        protected ComputerPlayer(int id, string name, PieceType pieceType)
            : base(id, name, pieceType)
        {
        }
    }
}
