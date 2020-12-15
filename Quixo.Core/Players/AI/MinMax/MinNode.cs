using System;

namespace Quixo.Core.Players.AI.MinMax
{
    public class MinNode : Node
    {
        public MinNode(Move move, PieceType playerPieceType, int depth)
            : base(move, playerPieceType, depth)
        {
        }

        public override PieceType GetCurrentMovePieceType() => OpponentPieceType;
        public override int CompareValues(int v1, int v2) => Math.Min(v1, v2);
        public override Node CreateChild(Move move, int depth) => new MaxNode(move, PlayerPieceType, depth);

        public override Move PickBestMoveFromChildren()
        {
            Node best = null;

            foreach (Node node in Children)
            {
                if (best == null || (node?.Value ?? 0) < best.Value)
                {
                    best = node;
                }
            }

            return best?.Move/* ?? this.Move*/;
        }
    }
}
