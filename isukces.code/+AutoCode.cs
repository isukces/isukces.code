// ReSharper disable All
using iSukces.Code;
using iSukces.Code.Ammy;
using iSukces.Code.Interfaces.Ammy;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace iSukces.Code.Compatibility.System.Windows.Data
{
    partial class XMultiBinding
    {
        [AutocodeGenerated]
        public XBindingMode? Mode { get; set; }

        [AutocodeGenerated]
        public object Converter { get; set; }

        [AutocodeGenerated]
        public object ConverterCulture { get; set; }

        [AutocodeGenerated]
        public object ConverterParameter { get; set; }

        [AutocodeGenerated]
        public XUpdateSourceTrigger? UpdateSourceTrigger { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnSourceUpdated { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnTargetUpdated { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnValidationError { get; set; }

        [AutocodeGenerated]
        public List<object> ValidationRules { get; } = new List<object>();

        [AutocodeGenerated]
        public bool? ValidatesOnExceptions { get; set; }

        [AutocodeGenerated]
        public bool? ValidatesOnDataErrors { get; set; }

        [AutocodeGenerated]
        public bool? ValidatesOnNotifyDataErrors { get; set; }

        [AutocodeGenerated]
        public string StringFormat { get; set; }

        [AutocodeGenerated]
        public string BindingGroupName { get; set; }

        [AutocodeGenerated]
        public object TargetNullValue { get; set; }

        [AutocodeGenerated]
        public List<object> Bindings { get; set; }

    }
}
