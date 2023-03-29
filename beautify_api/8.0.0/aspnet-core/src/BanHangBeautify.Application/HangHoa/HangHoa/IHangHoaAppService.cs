using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.Data.Entities;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using System;

namespace BanHangBeautify.HangHoa.HangHoa
{
    public interface IHangHoaAppService 
    {
        Task<CreateOrEditHangHoaDto> Create(CreateOrEditHangHoaDto input);
        Task<CreateOrEditHangHoaDto> Edit(CreateOrEditHangHoaDto input, DM_HangHoa objUpdate);
    }
}
