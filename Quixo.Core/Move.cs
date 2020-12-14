using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core
{
    public class Move
    {
        public int Index { get; set; }
        public MoveDirection Direction { get; set; }

        public override string ToString()
        {
            return $"{Index}:{Direction.ToString()}";
        }
    }
}
