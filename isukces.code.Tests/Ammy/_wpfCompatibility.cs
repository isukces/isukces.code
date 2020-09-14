namespace iSukces.Code.Tests.Ammy
{
    public class RowDefinition
    {
        
    }
    public class ColumnDefinition
    {
        
    }

    public class TextBox
    {
        public string Text { get; set; }
    }
    public class TextBlock
    {
        
    }
    public class RadComboBox
    {
        
    }
    public class CheckBox
    {
        
    }

    public class DoubleValidation
    {
        public bool CanBeNull    { get; set; }
        public bool OnlyPositive { get; set; }

        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }

        public string ValueName { get; set; }
    }
}