using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core.Results
{
    public class PieceSelectedResult : ControllerResult
    {
        public PieceSelectedResult(int? selectedPieceIndex)
        {
            _isSuccessful = true;
            _message = $"Piece #{selectedPieceIndex + 1} sélectionnée!";
        }
    }
}
