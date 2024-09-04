#nullable enable
#nullable enable
#nullable enable
#nullable enable
namespace iSukces.Code.Ui.DataGrid
{
    public class LookupInfo
    {
        public static LookupInfo Empty
        {
            get { return new LookupInfo(); }
        }

        public object Source            { get; set; }
        public string SelectedValuePath { get; set; }
        public string DisplayMemberPath { get; set; }

        public bool IsEmpty
        {
            get { return Source == null; }
        }
    }
}