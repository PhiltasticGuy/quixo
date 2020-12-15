using Quixo.Core.Players.AI.MinMax;

namespace Quixo.Core.Players.AI
{
    public class SingleThreadMinMaxAiPlayer : MinMaxAiPlayer
    {
        public SingleThreadMinMaxAiPlayer(int id, string name, PieceType pieceType)
            : base(id, name, pieceType)
        {
        }

        public SingleThreadMinMaxAiPlayer(int id, string name, PieceType pieceType, int depth)
            : base(id, name, pieceType, depth)
        {
        }

        protected override int MinMax(QuixoBoard board, Node node)
        {
            PieceType winner = board.GetWinner();

            if (node.Depth == MaxDepth || winner != PieceType.Empty)
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
