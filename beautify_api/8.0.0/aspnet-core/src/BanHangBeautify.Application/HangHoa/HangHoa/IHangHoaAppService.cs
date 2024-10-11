using BanHangBeautify.Data.Entities;
using BanHangBeautify.HangHoa.DonViQuiDoi.Dto;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using System;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.HangHoa
{
    public interface IHangHoaAppService
    {
        Task<CreateOrEditHangHoaDto> Create(CreateOrEditHangHoaDto input);
        Task<CreateOrEditHangHoaDto> Edit(CreateOrEditHangHoaDto input, DM_HangHoa objUpdate);
        Task<bool> CheckExistsMaHangHoa(string mahanghoa, Guid? id = null);
        Task<DonViQuiDoiDto> GetDMQuyDoi_byMaHangHoa(string maHangHoa);
    }
}
