using BanHangBeautify.KhachHang.KhachHang.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang
{
    public interface IKhachHangAppService
    {
        Task<bool> CheckExistSoDienThoai(string phone, Guid? id = null);
        Task<KhachHangDto> CreateOrEdit(CreateOrEditKhachHangDto dto);
        Task<List<Guid>> GetListCustomerId_byPhone(string phone);
        Task<bool> Update_IdKhachHangZOA(Guid idCustomer, Guid? idKhachHangZOA);
    }
}
