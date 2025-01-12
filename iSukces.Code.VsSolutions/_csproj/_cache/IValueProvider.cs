namespace iSukces.Code.VsSolutions;

public interface IValueProvider<T>
{
    T? Value { get; set; }
}

