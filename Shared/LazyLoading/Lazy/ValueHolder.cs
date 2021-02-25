using System;

namespace Shared.LazyLoading.Lazy
{
    public class ValueHolder<T> : IValueHolder<T>
    {
        private T value;
        private Func<object, T> _getValue; 
        
        public ValueHolder(Func<object, T> getValue)
        {
            _getValue = getValue;
        }

        public T GetValue(object parameter)
        {
            if (value == null)
            {
                value = _getValue(parameter);
            }
            return value;
        }
    }
}
