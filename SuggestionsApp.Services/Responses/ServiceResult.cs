using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuggestionsApp.Services.Responses
{
    public class ServiceResult
    {
        public bool Success { get; }
        public bool Failure { get; }

        public IEnumerable<string> Errors { get; } = null!;

        public ServiceResult() {
            Success = true;
        }

        public ServiceResult(IEnumerable<string> errors) 
        {
            Errors = errors;
            Failure = true;
        }
    }
}
