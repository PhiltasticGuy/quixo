using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core.Results
{
    public abstract class GameStateResult : ControllerResult
    {
        protected bool _isGameFinished = false;

        public bool IsGameFinished => _isGameFinished;

        public GameStateResult()
        {
        }
    }
}
