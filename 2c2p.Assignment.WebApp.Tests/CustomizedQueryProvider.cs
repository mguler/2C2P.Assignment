using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace _2c2p.Assignment.WebApp.Tests
{
    public class CustomizedQueryProvider : IAsyncQueryProvider
    {
        private readonly IQueryProvider _underlyingQueryProvider;
        private readonly List<Delegate> _callbackCache = new List<Delegate>();


        public CustomizedQueryProvider(IQueryProvider underlyingQueryProvider, params Delegate[] callbacks)
        {
            _underlyingQueryProvider = underlyingQueryProvider;
            callbacks?.ToList().ForEach(callback => _callbackCache.Add(callback));
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            var result = _underlyingQueryProvider.Execute<TResult>(expression);
            InvokeCallback("ExecuteAsync<TResult>", expression, result);
            return result;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            var result = _underlyingQueryProvider.CreateQuery(expression);
            InvokeCallback("CreateQuery", expression, result);
            return _underlyingQueryProvider.CreateQuery(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            var result = _underlyingQueryProvider.CreateQuery<TElement>(expression);
            InvokeCallback("CreateQuery<TElement>", expression, result);
            return result;
        }

        public object Execute(Expression expression)
        {
            var result = _underlyingQueryProvider.Execute(expression);
            InvokeCallback("Execute", expression, result);
            return result;
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var result = _underlyingQueryProvider.Execute<TResult>(expression);
            InvokeCallback("Execute<TResult>", expression, result);
            return result;
        }

        public void RegisterCallback(Action<string, Expression, object>[] callbacks)
        {
            callbacks?.ToList().ForEach(callback => _callbackCache.Add(callback));
        }

        private void InvokeCallback(string methodName, Expression expression, object result)
        {
            if (result == null)
            {
                return;
            }
            var isEnumerable = typeof(IEnumerable<>).IsAssignableFrom(result.GetType());
            var resultType = isEnumerable ? result.GetType().GetGenericArguments().FirstOrDefault() : result.GetType();

            List<Delegate> callbacks = null;
            if (isEnumerable)
            {
                callbacks = _callbackCache.Where(callback =>
                callback.Method.GetParameters().LastOrDefault().ParameterType.GetGenericArguments().FirstOrDefault() == resultType).ToList();
            }
            else
            {
                callbacks = _callbackCache.Where(callback =>
                callback.Method.GetParameters().LastOrDefault().ParameterType == resultType).ToList();
            }

            callbacks.ForEach(callback => callback.DynamicInvoke(methodName, expression, result));
        }
    }
}