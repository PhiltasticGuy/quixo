using Quixo.Core.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core.Players
{
    public class MinMaxAiPlayer : ComputerPlayer
    {
        private static int _maxDepth = 1;

        public static int MaxDepth => _maxDepth;

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
            //var tree = BuildDecisionTree(board);

            Node root = new MaxNode(null, PieceType, 0);
            MinMax(board, root);
            Move bestMove = root.PickBestMoveFromChildren();
            board.Play(bestMove, this.PieceType);

            Console.WriteLine($"{this.Name}: Piece #{bestMove.Index + 1}, {bestMove.Direction}");

            return true;
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

        private int MinMax(QuixoBoard board, Node node)
        {
            PieceType winner = board.GetWinner();

            if (node.Depth == MinMaxAiPlayer.MaxDepth || winner != PieceType.Empty)
            {
                node.Value = node.Evalute(board);
                return node.Value;
            }

            // Charger la liste de mouvements possibles pour ce joueur.
            var moves = board.GetValidMoves(node.PieceType);

            var value = 0;
            foreach (var move in moves)
            {
                QuixoBoard copy = board.DeepClone();
                copy.Play(move, node.PieceType);

                var child = node.CreateChild(move, node.Depth + 1);
                node.Children.Add(child);

                value = node.CompareValues(value, MinMax(copy, child));
            }
            node.Value = value;

            return node.Value;
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
                node.Value = EvaluateRows(board, node) +
                    EvaluateColumns(board, node) +
                    EvaluateDiagonals(board, node);
            }

            return node.Value;
        }

        private int EvaluateRows(QuixoBoard board, Node node)
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

        private int EvaluateColumns(QuixoBoard board, Node node)
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

        private int EvaluateDiagonals(QuixoBoard board, Node node)
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
    }
}
