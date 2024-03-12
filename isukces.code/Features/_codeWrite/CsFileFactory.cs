#nullable enable
using System;
using System.Reflection;

namespace iSukces.Code;

public sealed class CsFileFactory
{
    private CsFileFactory()
    {
    }

    public CsFile Create(Assembly? assembly, object? context = null) 
        => Create(assembly, null, context);

    public CsFile Create(Type? requestor, object? context = null)
        => Create(null, requestor, context);

    public CsFile Create(Assembly? assembly, Type? requestor, object? context = null)
    {
        var e = new CreateCsFileEventArgs(assembly, context, requestor);
        CreateCsFile?.Invoke(this, e);
        return e.File ?? new CsFile();
    }

    public static CsFileFactory Instance => CsFileFactoryHolder.SingleIstance;

    public event EventHandler<CreateCsFileEventArgs>? CreateCsFile;

    private static class CsFileFactoryHolder
    {
        public static readonly CsFileFactory SingleIstance = new CsFileFactory();
    }
}

public sealed class CreateCsFileEventArgs : EventArgs
{
    public CreateCsFileEventArgs(Assembly? assembly, object? context, Type? requestor)
    {
        Assembly  = assembly;
        Context   = context;
        Requestor = requestor;
        File      = new CsFile();
    }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Assembly? Assembly  { get; }
    public object?   Context   { get; }
    public Type?     Requestor { get; }

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public CsFile File { get; set; }
}
