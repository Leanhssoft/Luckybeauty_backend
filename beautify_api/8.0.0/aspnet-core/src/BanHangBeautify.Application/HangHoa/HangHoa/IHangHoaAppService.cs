using BanHangBeautify.Data.Entities;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.HangHoa
{
    public interface IHangHoaAppService
    {
        Task<CreateOrEditHangHoaDto> Create(CreateOrEditHangHoaDto input);
        Task<CreateOrEditHangHoaDto> Edit(CreateOrEditHangHoaDto input, DM_HangHoa objUpdate);
    }
}
