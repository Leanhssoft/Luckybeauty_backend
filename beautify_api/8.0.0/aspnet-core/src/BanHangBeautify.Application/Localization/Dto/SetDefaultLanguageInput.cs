using Abp.Localization;
using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Localization.Dto
{
    public class SetDefaultLanguageInput
    {
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public virtual string Name { get; set; }
    }
}
