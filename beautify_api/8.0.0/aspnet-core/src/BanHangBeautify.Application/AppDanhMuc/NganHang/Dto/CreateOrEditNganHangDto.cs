using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.NganHang.Dto
{
    public class CreateOrEditNganHangDto
    {
        public Guid Id{set;get;}
public Guid MaNganHang{set;get;}
public Guid TenNganHang{set;get;}
public Guid ChiPhiThanhToan{set;get;}
public Guid TheoPhanTram{set;get;}
public Guid ThuPhiThanhToan{set;get;}
public Guid GhiChu{set;get;}
public Guid TrangThai{set;get;}
    }
}
