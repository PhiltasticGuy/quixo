using System.Collections.Generic;

namespace Quixo.Core.AI
{
    public class Node
    {
        public Move Move { get; set; }

        public PieceType PieceType { get; set; }

        public bool IsMaxPlayer { get; set; }

        public int Value { get; set; }

        public bool IsVictoryNode => (IsMaxPlayer ? Value == int.MaxValue : Value == int.MinValue);

        public List<Node> Children { get; set; }

        public Node(Move move, bool isMaxPlayer)
        {
            Move = move;
            IsMaxPlayer = isMaxPlayer;
            Children = new List<Node>();
        }

        public Node(Move move, PieceType pieceType, bool isMaxPlayer)
        {
            Move = move;
            PieceType = pieceType;
            IsMaxPlayer = isMaxPlayer;
            Children = new List<Node>();
        }

        public Move GetBestMove()
        {
            if (IsMaxPlayer)
            {
                Node max = null;

                foreach (Node node in Children)
                {
                    if (max == null || node.Value > max.Value)
                    {
                        max = node;
                    }
                }

                return max.Move;
            }
            else
            {
                Node min = null;

                foreach (Node node in Children)
                {
                    if (min == null || node.Value < min.Value)
                    {
                        min = node;
                    }
                }

                return min.Move;
            }
        }
    }
}
