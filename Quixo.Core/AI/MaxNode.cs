using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core.AI
{
    public class MaxNode : Node
    {
        public MaxNode(Move move, PieceType playerPieceType, int depth)
            : base(move, playerPieceType, depth)
        {
        }

        public override PieceType GetCurrentMovePieceType() => PlayerPieceType;
        public override int CompareValues(int v1, int v2) => Math.Max(v1, v2);
        public override Node CreateChild(Move move, int depth) => new MinNode(move, PlayerPieceType, depth);

        public override Move PickBestMoveFromChildren()
        {
            Node best = null;

            foreach (Node node in Children)
            {
                if (best == null || node.Value > best.Value)
                {
                    best = node;
                }
            }

            return best.Move;
        }
    }
}
