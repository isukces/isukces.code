#nullable enable
using System;

namespace iSukces.Code.AutoCode;

public interface IFileSavedNotifier
{
    void FileSaved(object generator, string fileName);
    event EventHandler<FileSavedEventArgs>? OnFileSaved;
}

public sealed class FileSavedEventArgs : EventArgs
{
    public FileSavedEventArgs(object generator, string fileName)
    {
        Generator = generator;
        FileName       = fileName;
    }

    public object Generator { get; }
    public string FileName  { get; }
}


public delegate void FileSavedDelegate(object generator, string fileName);