// See https://aka.ms/new-console-template for more information

using System.Text;

namespace Build.iSukces.Code;

internal class Program
{
    public static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var cfg = new Script();
        
        //cfg.UpdateVersion();
        cfg.Run();
    }
}