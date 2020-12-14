using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core.Players
{
    public class MinMaxAiPlayer : ComputerPlayer
    {
        private const int MaxDepth = 2;

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

        public class DecisionTree
        {
            public Node Root { get; set; }

            public DecisionTree(Node root)
            {
                Root = root;
            }
        }

        private DecisionTree BuildDecisionTree(QuixoBoard board)
        {
            var tree = new DecisionTree(new Node(null, this.PieceType, true));

            QuixoBoard copy = board.DeepClone();

            BuildDecisionTree(copy, tree.Root, 0);

            return tree;
        }

        private void BuildDecisionTree(QuixoBoard board, Node node, int depth)
        {
            if (depth > 3)
            {
                return;
            }

            PieceType nodePieceType;
            if (node.IsMaxPlayer)
            {
                nodePieceType = this.PieceType;
            }
            else
            {
                nodePieceType = (this.PieceType == PieceType.Circle ? PieceType.Crossmark : PieceType.Circle);
            }

            var moves = board.GetValidMoves(nodePieceType);

            foreach (var move in moves)
            {
                QuixoBoard copy = board.DeepClone();
                copy.Play(move, nodePieceType);

                var child = new Node(move, !node.IsMaxPlayer);
                node.Children.Add(child);

                BuildDecisionTree(copy, child, depth + 1);
            }
        }

        private int CountPiecesInOpenRows(QuixoBoard board, Node node)
        {
            var value = 0;
            var isStillOpen = true;

            for (int i = 0; i < 5; i++)
            {
                int count = 0;
                for (int j = 0; j < 5; j++)
                {
                    int index = i * 5 + j;
                    if (board.Pieces[index].PieceType == node.PieceType)
                    {
                        count++;
                    }
                    else
                    {
                        isStillOpen = false;
                        break;
                    }
                }

                if (isStillOpen && count > 0)
                {
                    value += count;
                }
            }

            return value;
        }

        private int CountPiecesInOpenColumns(QuixoBoard board, Node node)
        {
            var value = 0;
            var isStillOpen = true;

            for (int i = 0; i < 5; i++)
            {
                int count = 0;
                for (int j = 0; j < 5; j++)
                {
                    int index = j * 5 + i;
                    if (board.Pieces[index].PieceType == node.PieceType)
                    {
                        count++;
                    }
                    else
                    {
                        isStillOpen = false;
                        break;
                    }
                }

                if (isStillOpen && count > 0)
                {
                    value += count;
                }
            }

            return value;
        }

        private int CountPiecesInOpenDiagonals(QuixoBoard board, Node node)
        {
            var value = 0;
            var isStillOpen = true;

            int count = 0;
            for (int i = 0; i < 5; i++)
            {
                if (board.Pieces[i * 5 + i].PieceType == node.PieceType)
                {
                    count++;
                }
                else
                {
                    isStillOpen = false;
                    break;
                }
            }

            if (isStillOpen && count > 0)
            {
                value += count;
            }

            count = 0;
            isStillOpen = true;
            for (int i = 0; i < 5; i++)
            {
                if (board.Pieces[(i + 1) * 4].PieceType == node.PieceType)
                {
                    count++;
                }
                else
                {
                    isStillOpen = false;
                    break;
                }
            }

            if (isStillOpen && count > 0)
            {
                value += count;
            }

            return value;
        }

        private int Evaluate(QuixoBoard board, Node node, PieceType winningPiece)
        {
            if (winningPiece == this.PieceType)
            {
                node.Value = int.MaxValue;
            }
            else if (winningPiece == GetOpponentPieceType())
            {
                node.Value = int.MinValue;
            }
            else
            {
                node.Value = CountPiecesInOpenRows(board, node) +
                    CountPiecesInOpenColumns(board, node) +
                    CountPiecesInOpenDiagonals(board, node);
            }

            return node.Value;
        }

        private PieceType GetOpponentPieceType()
        {
            if (this.PieceType == PieceType.Circle)
            {
                return PieceType.Crossmark;
            }
            else if (this.PieceType == PieceType.Crossmark)
            {
                return PieceType.Circle;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private int MinMax(QuixoBoard board, Node node, int depth)
        {
            PieceType winner = board.GetWinner();

            if (depth == 0 || winner != PieceType.Empty)
            {
                return Evaluate(board, node, winner);
            }

            PieceType nodePieceType;
            if (node.IsMaxPlayer)
            {
                nodePieceType = this.PieceType;
            }
            else
            {
                nodePieceType = (this.PieceType == PieceType.Circle ? PieceType.Crossmark : PieceType.Circle);
            }
            var moves = board.GetValidMoves(nodePieceType);

            if (node.IsMaxPlayer)
            {
                var value = int.MinValue;

                foreach (var move in moves)
                {
                    QuixoBoard copy = board.DeepClone();
                    copy.Play(move, nodePieceType);

                    var child = new Node(move, !node.IsMaxPlayer);
                    node.Children.Add(child);

                    value = Math.Max(value, MinMax(copy, child, depth - 1));
                }

                return value;
            }
            else
            {
                var value = int.MaxValue;

                foreach (var move in moves)
                {
                    QuixoBoard copy = board.DeepClone();
                    copy.Play(move, nodePieceType);

                    var child = new Node(move, !node.IsMaxPlayer);
                    node.Children.Add(child);

                    value = Math.Min(value, MinMax(copy, child, depth - 1));
                }

                return value;
            }
        }

        public override bool PlayTurn(QuixoBoard board)
        {
            //var tree = BuildDecisionTree(board);

            Node root = new Node(null, true);
            MinMax(board, root, MaxDepth);
            Move bestMove = root.GetBestMove();
            board.Play(bestMove, this.PieceType);

            Console.WriteLine($"{this.Name}: Piece #{bestMove.Index + 1}, {bestMove.Direction}");

            return true;
        }

        public MinMaxAiPlayer(int id, string name, PieceType pieceType)
            : base(id, name, pieceType)
        {
        }
    }
}
