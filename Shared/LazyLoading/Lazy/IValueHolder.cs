namespace Shared.LazyLoading.Lazy
{
    // A value holder uses a value loader to retrieve 
    // something in a lazy manner
    public interface IValueHolder<T>
    {
        T GetValue(object parameter);
    }
}
