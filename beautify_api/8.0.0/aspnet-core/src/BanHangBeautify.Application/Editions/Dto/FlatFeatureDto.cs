using Abp.Runtime.Validation;
using Abp.UI.Inputs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Editions.Dto
{
    public class FlatFeatureDto
    {
        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string DefaultValue { get; set; }
        public FeatureInputTypeDto InputType { get; set; }
    }
    public class FlatFeatureSelectDto
    {
        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string DefaultValue { get; set; }

        public IInputType InputType { get; set; }

        public string TextHtmlColor { get; set; }
    }
    public class FeatureInputTypeDto
    {
        public string Name { get; set; }

        public IDictionary<string, object> Attributes { get; set; }

        public IValueValidator Validator { get; set; }

        public LocalizableComboboxItemSourceDto ItemSource { get; set; }
    }
    public class LocalizableComboboxItemSourceDto
    {
        public Collection<LocalizableComboboxItemDto> Items { get; set; }
    }
    public class LocalizableComboboxItemDto
    {
        public string Value { get; set; }

        public string DisplayText { get; set; }
    }
}
