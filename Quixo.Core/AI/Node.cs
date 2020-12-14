using Quixo.Core.Players;
using System;
using System.Collections.Generic;

namespace Quixo.Core.AI
{
    public abstract class Node
    {
        public int Depth { get; set; }
        public Move Move { get; set; }
        public PieceType PieceType { get; set; }
        public int Value { get; set; }
        public List<Node> Children { get; set; }

        public Node(Move move, PieceType pieceType, int depth)
        {
            Move = move;
            PieceType = pieceType;
            Depth = depth;
            Children = new List<Node>();
        }

        public PieceType OpponentPieceType => (PieceType == PieceType.Circle ? PieceType.Crossmark : PieceType.Circle);
        protected int GetWinValue() => GetWinValue(PieceType);

        protected abstract int GetWinValue(PieceType winningPieceType);
        public abstract int CompareValues(int v1, int v2);
        public abstract Node CreateChild(Move move, int depth);
        public abstract Move PickBestMoveFromChildren();

        private bool IsVictoryNode()
        {
            throw new NotImplementedException();
        }

        public int Evalute(QuixoBoard board)
        {
            var winner = board.GetWinner();
            if (winner != PieceType.Empty)
            {
                return GetWinValue(winner) - this.Depth;
            }

            int random = new Random().Next(100000);
            return random;
        }
    }
}
