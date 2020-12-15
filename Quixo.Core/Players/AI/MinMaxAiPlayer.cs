using Quixo.Core.Players.AI.MinMax;
using System;

namespace Quixo.Core.Players.AI
{
    public abstract class MinMaxAiPlayer : ComputerPlayer
    {
        protected readonly int _maxDepth = 3;
        public int MaxDepth => _maxDepth;

        public MinMaxAiPlayer(int id, string name, PieceType pieceType)
            : base(id, name, pieceType)
        {
        }

        public MinMaxAiPlayer(int id, string name, PieceType pieceType, int depth)
            : base(id, name, pieceType)
        {
            _maxDepth = depth;
        }

        public override bool PlayTurn(QuixoBoard board)
        {
            Node root = new MaxNode(null, PieceType, 0);
            MinMax(board, root);
            Move bestMove = root.PickBestMoveFromChildren();
            board.Play(bestMove, this.PieceType);

            Console.WriteLine($"{this.Name}: Piece #{bestMove.Index + 1}, {bestMove.Direction}");

            return true;
        }

        protected abstract int MinMax(QuixoBoard board, Node node);
    }
}
