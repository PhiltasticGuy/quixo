using Quixo.Core.Players.AI.MinMax;
using System;

namespace Quixo.Core.Players.AI
{
    public class MinMaxAiPlayer : ComputerPlayer
    {
        protected readonly MinMaxStrategy _minMaxStrategy;

        public MinMaxAiPlayer(int id, string name, PieceType pieceType, MinMaxStrategy minMaxStrategy)
            : base(id, name, pieceType)
        {
            _minMaxStrategy = minMaxStrategy;
        }

        public override bool PlayTurn(QuixoBoard board)
        {
            Node root = new MaxNode(null, PieceType, 0);
            _minMaxStrategy.MinMax(board, root);
            Move bestMove = root.PickBestMoveFromChildren();

            board.Play(bestMove, this.PieceType);

            Console.WriteLine($"{this.Name}: Piece #{bestMove.Index + 1}, {bestMove.Direction}");

            return true;
        }
    }
}
