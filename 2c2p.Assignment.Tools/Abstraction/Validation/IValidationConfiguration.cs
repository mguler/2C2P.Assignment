using _2c2p.Assignment.Tools.Abstraction.Validation;

namespace _2c2p.Assignment.Tools.Abstraction.Mapping
{
    /// <summary>
    /// abstract definition for mapping configuration class
    /// </summary>
    public interface IValidationConfiguration
    {
        /// <summary>
        /// this method adds configuration to the given service provider instance 
        /// </summary>
        /// <param name="validationServiceProvider">mapping service</param>
        void Configure(IValidationServiceProvider validationServiceProvider);
    }
}
