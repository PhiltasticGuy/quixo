using Quixo.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core.Results
{
    public class NextPlayerResult : GameStateResult
    {
        public NextPlayerResult(Player nextPlayer)
        {
            _isSuccessful = true;
            _message = $"C'est le tour du prochain joueur! (Player #{nextPlayer.Id})";
        }
    }
}
