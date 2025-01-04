#nullable enable
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public interface IValueProvider<T>
{
    T? Value { get; set; }
}

internal abstract class XmlCachedWrapper<T> : XmlWrapper, IValueProvider<T>
{
    protected XmlCachedWrapper(XDocument document)
        : base(document)
    {
    }


    protected abstract T? GetValueInternal();

    public virtual void Invalidate()
    {
        _isValid = false;
    }


    protected abstract T? SetValueInternal(T? value);

    public T? Value
    {
        get
        {
            if (_isValid)
                return cached;
            cached   = GetValueInternal();
            _isValid = true;
            return cached;
        }
        set
        {
            cached   = SetValueInternal(value);
            _isValid = true;
        }
    }

    #region Fields

    private bool _isValid;

    private T? cached;

    #endregion
}
