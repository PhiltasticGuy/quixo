using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quixo.Core.Players
{
    public abstract class ComputerPlayer : Player
    {
        public override bool IsHuman => false;

        protected ComputerPlayer(int id, string name, PieceType pieceType)
            : base(id, name, pieceType)
        {
        }
    }
}
