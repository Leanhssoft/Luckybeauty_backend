using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.HangHoa.LoaiHangHoa.Dto;
using Microsoft.AspNetCore.Mvc;
using BanHangBeautify.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.LoaiHangHoa
{
    public class LoaiHangHoaAppService: SPAAppServiceBase
    {
        private readonly IRepository<DM_LoaiHangHoa, Guid> _repository;
        public LoaiHangHoaAppService(IRepository<DM_LoaiHangHoa, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<ListResultDto<LoaiHangHoaDto>> GetAll()
        {
            ListResultDto<LoaiHangHoaDto> result = new ListResultDto<LoaiHangHoaDto>();
            var loaiHangHoas = await _repository.GetAllListAsync();
            loaiHangHoas = loaiHangHoas.Where(x => x.TrangThai == 0).ToList();
            var data = ObjectMapper.Map<List<LoaiHangHoaDto>>(loaiHangHoas);
            result.Items = data;
            return result;
        } 
        public async Task CreateOrEdit(CreateOrEditLoaiHangHoaDto dto)
        {
           var checkExist = _repository.FirstOrDefault(dto.Id);
            if (checkExist==null)
            {
                 await Create(dto);
            }
            else
            {
                await Edit(dto, checkExist);

            }
             
        }
        [NonAction]
        public async Task Create(CreateOrEditLoaiHangHoaDto dto)
        {
            DM_LoaiHangHoa data = new DM_LoaiHangHoa();
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.NgayTao = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.TrangThai = 0;
            data.TenLoai = dto.TenLoai;
            data.MaLoai= dto.MaLoai;
            await _repository.InsertAsync(data);
        }
        [NonAction]
        public async Task Edit(CreateOrEditLoaiHangHoaDto dto,DM_LoaiHangHoa data)
        {
            data.LastModificationTime = DateTime.Now;
            data.NgaySua = DateTime.Now;
            data.LastModifierUserId = AbpSession.UserId;
            data.TrangThai = dto.TrangThai;
            data.TenLoai = dto.TenLoai;
            data.MaLoai = dto.MaLoai;
            await _repository.UpdateAsync(data);
            var result = ObjectMapper.Map<LoaiHangHoaDto>(data);
        }
        public async Task<LoaiHangHoaDto> Delete(Guid id)
        {
            LoaiHangHoaDto result = new LoaiHangHoaDto();
            var checkExist =await _repository.FirstOrDefaultAsync(id);
            if (checkExist != null)
            {
                checkExist.IsDeleted= true;
                checkExist.NgayXoa = DateTime.Now;
                checkExist.DeleterUserId = AbpSession.UserId;
                checkExist.TrangThai = 1;
                checkExist.DeletionTime= DateTime.Now;
                await _repository.UpdateAsync(checkExist);
                result = ObjectMapper.Map<LoaiHangHoaDto>(checkExist);

            }
            return result;
        }
    }
}
