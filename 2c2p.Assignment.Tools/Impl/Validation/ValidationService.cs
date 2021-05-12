using _2c2p.Assignment.Tools.Abstraction.Mapping;
using _2c2p.Assignment.Tools.Abstraction.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2c2p.Assignment.Tools.Impl.Validation
{
    public class ValidationService : IValidationServiceProvider
    {
        /// <summary>
        /// Contains registered mappings for specific types
        /// </summary>
        private readonly IList<Delegate> _cache = new List<Delegate>();

        /// <summary>
        /// Contains registered mappings for specific names
        /// </summary>
        private readonly Dictionary<string, Delegate> _namedCache = new Dictionary<string, Delegate>();



        public ValidationService()
        {

        }

        public ValidationService(IValidationConfiguration[] validationConfigurations)
        {
            foreach (var validationConfiguration in validationConfigurations)
            {
                validationConfiguration.Configure(this);
            }
        }

        public IValidationResult Validate(object source)
        {
            if (source == null)
            {
                throw new ArgumentException($"Source object cannot be null");
            }

            var type = source.GetType();
            var validator = _cache.FirstOrDefault(mapping => mapping.Method.GetParameters().FirstOrDefault()
                                                                     ?.ParameterType == type);

            if (validator == null)
            {
                throw new Exception($"Validation configuration for {type} does not exists");
            }

            var result = validator.DynamicInvoke(source);
            return (IValidationResult)result;
        }

        /// <summary>
        /// This method creates an instance of the type given in the TTarget parameter and maps source object's properties to the target instance
        /// </summary>
        /// <param name="key">mapper function identifier</param>
        /// <typeparam name="TTarget">the target type</typeparam>
        /// <param name="source">object instance to be mapped to target type</param>
        /// <returns>an instance of target type</returns>
        public IValidationResult Validate(string key, object source)
        {
            if (source == null)
            {
                throw new ArgumentException($"Source object cannot be null");
            }

            var type = source.GetType();

            var sourceType = source.GetType();
            var validator = _namedCache.FirstOrDefault(mapping => mapping.Key == key && mapping.Value.Method.GetParameters().FirstOrDefault()
                                                                     ?.ParameterType == sourceType).Value;

            if (validator == null && _namedCache.ContainsKey(key))
            {
                throw new Exception($"Validator with the given key {key} registered but looks like it could not validate the type {type}");
            }

            if (validator == null)
            {
                throw new Exception($"Validator configuration for {type} does not exists");
            }

            var result = validator.DynamicInvoke(source);
            return (IValidationResult)result;
        }

        /// <summary>
        /// Registers mapping configurations for specific types
        /// </summary>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TTarget">target type</typeparam>
        /// <param name="func">the method which supplies mapping service between the types given in the type parameters</param>
        public void Register<T>(Func<T, IValidationResult> func)
        {
            var alreadyExists = _cache.Any(mapping => mapping.GetType() == func.GetType());
            if (alreadyExists)
            {
                throw new Exception($"Validation configuration for {typeof(T)} has already been defined before");
            }
            _cache.Add(func);
        }

        /// <summary>
        /// Registers mapping configurations for specific types
        /// </summary>
        /// <typeparam name="key">name for mapping function</typeparam>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TTarget">target type</typeparam>
        /// <param name="func">the method which supplies mapping service between the types given in the type parameters</param>
        public void RegisterByKey<T>(string key, Func<T, IValidationResult> func)
        {
            var alreadyExists = _namedCache.Any(mapping => mapping.Key == key);
            if (alreadyExists)
            {
                throw new Exception($"Validation configuration with the key '{key}' has already been defined before");
            }
            _namedCache.Add(key, func);
        }
    }
}
