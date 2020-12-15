using System;
using System.Collections.Generic;
using System.Text;

namespace Quixo.Core.Players.AI.MinMax
{
    public abstract class MinMaxStrategy
    {
        protected readonly int _maxDepth = 3;
        public int MaxDepth => _maxDepth;

        public MinMaxStrategy(int depth)
        {
            _maxDepth = depth;
        }

        public abstract int MinMax(QuixoBoard board, Node node);
    }
}
