using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuggestionsApp.Models.Exceptions
{
    public class LogicException : Exception
    {
        public LogicException() { }

        public LogicException(string message) : base(message) { }
    }
}
