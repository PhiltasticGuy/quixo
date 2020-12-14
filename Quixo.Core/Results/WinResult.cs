using Quixo.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core.Results
{
    public class WinResult : GameStateResult
    {
        private Player _winner;

        public Player Winner => _winner;

        public WinResult(Player winner)
        {
            _isSuccessful = true;
            _message = $"Partie gagnée par Player #{winner.Id}!";

            _isGameFinished = true;
            _winner = winner;
        }
    }
}
