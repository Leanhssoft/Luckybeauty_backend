using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.MauIn.Dto
{
    public class CreateOrEditMauInDto
    {
        public Guid Id { get; set; }
        public int LoaiChungTu { set; get; }
        public string TenMauIn{set;get;}
        public bool LaMacDinh{set;get;}
        public string NoiDungMauIn{set;get;}
    }
}
