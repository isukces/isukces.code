namespace isukces.code.interfaces
{
    public interface ICsCodeMaker
    {
        void MakeCode(ICodeWriter writer);
    }

    public interface ICodeWriter
    {
        void AppendText(string text);
        int Indent { get; set; }
    }
}
