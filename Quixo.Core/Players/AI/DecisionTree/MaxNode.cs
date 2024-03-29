﻿using System;

namespace Quixo.Core.Players.AI.MinMax
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
                if (best == null || (node?.Value ?? 0) > best.Value)
                {
                    best = node;
                }
            }

            return best?.Move/* ?? this.Move*/;
        }
    }
}
