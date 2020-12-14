using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quixo.Core.Results
{
    public abstract class ControllerResult
    {
        protected bool _isSuccessful;
        protected string _message;

        public bool IsSuccessful => _isSuccessful;

        public string Message => _message;
    }
}
