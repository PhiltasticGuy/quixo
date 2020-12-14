using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core.Results
{
    public class ErrorResult : ControllerResult
    {
        public ErrorResult(string message)
        {
            _isSuccessful = false;
            _message = message;
        }
    }
}
