using System.ComponentModel.DataAnnotations;

namespace BanHangBeautify.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}