using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Editions.Dto
{
    public class EditionEditDto
    {
        public int? Id { get; set; }

        [Required]
        public string DisplayName { get; set; }
    }
}
