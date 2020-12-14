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
        private static int _maxDepth = 3;

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
            Node root = new MaxNode(null, PieceType, 0);
            MinMax(board, root);
            Move bestMove = root.PickBestMoveFromChildren();
            board.Play(bestMove, this.PieceType);

            Console.WriteLine($"{this.Name}: Piece #{bestMove.Index + 1}, {bestMove.Direction}");

            return true;
        }

        private int MinMax(QuixoBoard board, Node node)
        {
            PieceType winner = board.GetWinner();

            if (node.Depth == MinMaxAiPlayer.MaxDepth || winner != PieceType.Empty)
            {
                node.Value = node.Evaluate(board);
            }
            else
            {
                PieceType currentMovePieceType = node.GetCurrentMovePieceType();

                // Charger la liste de mouvements possibles pour ce joueur.
                var moves = board.GetValidMoves(currentMovePieceType);

                var value = 0;
                foreach (var move in moves)
                {
                    QuixoBoard copy = board.DeepClone();
                    copy.Play(move, currentMovePieceType);

                    var child = node.CreateChild(move, node.Depth + 1);
                    node.Children.Add(child);

                    value = node.CompareValues(value, MinMax(copy, child));
                }

                node.Value = value;
            }

            return node.Value;
        }
    }
}
