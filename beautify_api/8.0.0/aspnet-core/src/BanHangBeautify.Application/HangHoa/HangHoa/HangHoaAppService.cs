

using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using Microsoft.AspNetCore.Mvc;
using BanHangBeautify.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.HangHoa
{
    public class HangHoaAppService: SPAAppServiceBase
    {
        private readonly IRepository<DM_HangHoa,Guid> _repository;
        public HangHoaAppService(IRepository<DM_HangHoa, Guid> repository)
        {
            _repository= repository;
        }
        public async Task CreateOrEdit(CreateOrEditHangHoaDto dto)
        {
            var findHangHoa = await _repository.FirstOrDefaultAsync(h => h.Id == dto.Id);
            if (findHangHoa==null)
            {
               await Create(dto);
            }
            else {
               await Edit(dto,findHangHoa);
            }
        }
        [NonAction]
        public async Task Create(CreateOrEditHangHoaDto dto)
        {
            DM_HangHoa hangHoa = new DM_HangHoa();
            hangHoa.Id = Guid.NewGuid();
            hangHoa.IdLoaiHangHoa = dto.IdLoaiHangHoa;
            hangHoa.MaHangHoa = dto.MaHangHoa;
            hangHoa.TenHangHoa = dto.TenHangHoa;
            hangHoa.TrangThai = 0;
            hangHoa.TenantId = AbpSession.TenantId ?? 1;
            hangHoa.CreatorUserId= AbpSession.UserId;
            hangHoa.CreationTime = DateTime.Now;
            hangHoa.NgayTao = DateTime.Now;
            await _repository.InsertAsync(hangHoa);
        }
        [NonAction]
        public async Task Edit(CreateOrEditHangHoaDto dto,DM_HangHoa hangHoa)
        {
            hangHoa.IdLoaiHangHoa = dto.IdLoaiHangHoa;
            hangHoa.MaHangHoa = dto.MaHangHoa;
            hangHoa.TenHangHoa = dto.TenHangHoa;
            hangHoa.TrangThai = dto.TrangThai;
            hangHoa.NgaySua = DateTime.Now;
            hangHoa.LastModificationTime = DateTime.Now;
            hangHoa.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(hangHoa);
        }
        public async Task<ListResultDto<DM_HangHoa>> GetAll()
        {
            ListResultDto<DM_HangHoa> result = new ListResultDto<DM_HangHoa>();
            var getAllHangHoa = await _repository.GetAllListAsync();
            getAllHangHoa = getAllHangHoa.Where(x => x.TrangThai == 0 || x.IsDeleted == true).ToList();
            //var hangHoas = ObjectMapper.Map<List<HangHoaDto>>(getAllHangHoa);
            result.Items = getAllHangHoa;
            return result;
        }
        public async Task<bool> Delete(Guid id)
        {
            bool result = false;
            var findHangHoa =await _repository.FirstOrDefaultAsync(h => h.Id == id);
            if (findHangHoa != null)
            {
                findHangHoa.IsDeleted = true;
                findHangHoa.TrangThai = 1;
                findHangHoa.DeletionTime= DateTime.Now;
                findHangHoa.DeleterUserId = AbpSession.UserId;
                findHangHoa.NgayXoa = DateTime.Now;
                _repository.Update(findHangHoa);
                result = true;
            }
            return result;
        }
    }
}
