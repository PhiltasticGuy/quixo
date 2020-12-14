using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core.AI
{
    public class MinNode : Node
    {
        public MinNode(Move move, PieceType pieceType, int depth) : base(move, pieceType, depth)
        {
        }

        protected override int GetWinValue(PieceType winningPieceType) => (winningPieceType == PieceType ? int.MinValue : int.MaxValue);
        public override int CompareValues(int v1, int v2) => Math.Min(v1, v2);
        public override Node CreateChild(Move move, int depth)
        {
            return new MaxNode(move, OpponentPieceType, depth);
        }

        public override Move PickBestMoveFromChildren()
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
