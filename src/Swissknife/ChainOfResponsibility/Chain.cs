using System;
using System.Collections.Generic;

// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ClosureAllocation

namespace Swissknife.ChainOfResponsibility
{
    public class Chain : IAccessor
    {
        private readonly List<IAccessor> _accessors;
        private readonly IAccessorLogger _logger;
        
        public Chain(List<IAccessor> accessors, IAccessorLogger logger)
        {
            _accessors = accessors;
            _logger = logger;
        }

        public object Get(object req)
        {
            foreach (var accessor in _accessors)
            {
                var result = Safe(() => accessor.Get(req));
                if (!(result is null)) return result;
            }
            return null;
        }

        private object Safe(Func<object> func)
        {
            try { return func.Invoke(); }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }

    public interface IAccessor
    {
        object Get(object req);
    }

    public interface IAccessorLogger
    {
        void LogError(string msg);
        void LogError(Exception ex);
    }
}
