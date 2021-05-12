using System;

namespace _2c2p.Assignment.Tools.Abstraction.Mapping
{
    /// <summary>
    /// Supplies abstract structure for mapping service providers
    /// </summary>
    public interface IMappingServiceProvider
    {
        /// <summary>
        /// default mapper instance
        /// </summary>
        Func<object, object> DefaultMapper { get; }
        /// <summary>
        /// this is the abstract definition of map method with a generic type parameter
        /// </summary>
        /// <typeparam name="TTarget">target type</typeparam>
        /// <param name="source">source instance</param>
        /// <returns>an instance of target type</returns>
        TTarget Map<TTarget>(object source);
        /// <summary>
        /// this is the abstract definition of map method
        /// </summary>
        /// <param name="source">source instance</param>
        /// <param name="targetType">target type</param>
        /// <returns>an instance of target type</returns>
        object Map(object source, Type targetType);
        /// <summary>
        /// This method creates an instance of the type given in the TTarget parameter and maps source object's properties to the target instance
        /// </summary>
        /// <param name="key">mapper function identifier</param>
        /// <typeparam name="TTarget">the target type</typeparam>
        /// <param name="source">object instance to be mapped to target type</param>
        /// <returns>an instance of target type</returns>
        public TTarget Map<TTarget>(string key, object source);
        /// <summary>
        /// this definition supplies abstraction for mapper registration 
        /// </summary>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TTarget">target type</typeparam>
        /// <param name="func">mapper function</param>
        void Register<TSource, TTarget>(Func<TSource, TTarget> func);
        /// <summary>
        /// Registers mapping configurations for specific types
        /// </summary>
        /// <typeparam name="key">name for mapping function</typeparam>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TTarget">target type</typeparam>
        /// <param name="func">the method which supplies mapping service between the types given in the type parameters</param>
        void RegisterByKey<TSource, TTarget>(string key, Func<TSource, TTarget> func);
    }
}
