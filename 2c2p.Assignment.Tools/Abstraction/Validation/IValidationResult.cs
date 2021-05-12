using System.Collections.Generic;

namespace _2c2p.Assignment.Tools.Abstraction.Validation
{
    public interface  IValidationResult
    {
        string Description { get; set; }
        bool IsValid { get; }
        IList<string> Messages { get; set; }
    }
}
