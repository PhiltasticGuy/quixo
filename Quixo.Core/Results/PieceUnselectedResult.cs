using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core.Results
{
    public class PieceUnselectedResult : ControllerResult
    {
        public PieceUnselectedResult(int? selectedPieceIndex)
        {
            _isSuccessful = true;
            _message = $"Piece #{selectedPieceIndex + 1} désélectionnée!";
        }
    }
}
