﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Editions.Dto
{
    public class EditionCreateDto
    {
        public int? Id { get; set; }

        [Required]
        public string DisplayName { get; set; }
        public decimal? Price { get; set; }
    }
}
