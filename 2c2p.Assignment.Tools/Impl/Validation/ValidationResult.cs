using _2c2p.Assignment.Tools.Abstraction.Validation;
using System.Collections.Generic;

namespace _2c2p.Assignment.Tools.Impl.Validation
{
    public class ValidationResult: IValidationResult
    {
        public string Description { get; set; }
        public bool IsValid { get; set; } = true;
        public IList<string> Messages { get; set; } = new List<string>();
    }
}
