using System;

namespace _2c2p.Assignment.Tools.Abstraction.Validation
{
    public interface IValidationServiceProvider
    {
        IValidationResult Validate(object source);
        IValidationResult Validate(string key, object source);
        void Register<T>(Func<T, IValidationResult> func);
        void RegisterByKey<T>(string key, Func<T, IValidationResult> func);
    }
}
