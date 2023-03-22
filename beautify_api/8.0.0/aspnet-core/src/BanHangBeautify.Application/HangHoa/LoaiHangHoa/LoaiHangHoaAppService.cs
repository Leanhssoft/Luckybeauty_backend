using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.HangHoa.LoaiHangHoa.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.LoaiHangHoa
{
    [AbpAuthorize(PermissionNames.Pages_DM_LoaiHangHoa)]
    public class LoaiHangHoaAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_LoaiHangHoa, Guid> _repository;
        private readonly IRepository<DM_HangHoa, Guid> _hangHoaRepository;
        public LoaiHangHoaAppService(IRepository<DM_LoaiHangHoa, Guid> repository, IRepository<DM_HangHoa, Guid> hangHoaRepository)
        {
            _repository = repository;
            _hangHoaRepository = hangHoaRepository;
        }
        public async Task<ListResultDto<LoaiHangHoaDto>> GetAll(LoaiHangHoaPagedResultRequestDto input)
        {
            ListResultDto<LoaiHangHoaDto> result = new ListResultDto<LoaiHangHoaDto>();
            var loaiHangHoas = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId??1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                loaiHangHoas = loaiHangHoas.Where(x => x.TenLoai.Contains(input.Keyword) || x.MaLoai.Contains(input.Keyword)).ToList();
            }
            if (input.SkipCount > 0)
            {
                input.SkipCount *= 10;
            }
            loaiHangHoas = loaiHangHoas.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var data = ObjectMapper.Map<List<LoaiHangHoaDto>>(loaiHangHoas);
            result.Items = data;
            return result;
        }
        public async Task<DM_LoaiHangHoa> GetDetail(Guid id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<LoaiHangHoaDto> CreateOrEdit(CreateOrEditLoaiHangHoaDto dto)
        {
            var checkExist = _repository.FirstOrDefault(dto.Id);
            if (checkExist == null)
            {
                return await Create(dto);
            }
            else
            {
                return await Edit(dto, checkExist);

            }

        }
        [NonAction]
        public async Task<LoaiHangHoaDto> Create(CreateOrEditLoaiHangHoaDto dto)
        {
            DM_LoaiHangHoa data = new DM_LoaiHangHoa();
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.NgayTao = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.TrangThai = 0;
            data.TenLoai = dto.TenLoai;
            data.MaLoai = dto.MaLoai;
            data.IsDeleted = false;
            var result = ObjectMapper.Map<LoaiHangHoaDto>(data);
            await _repository.InsertAsync(data);
            return result;
        }
        [NonAction]
        public async Task<LoaiHangHoaDto> Edit(CreateOrEditLoaiHangHoaDto dto, DM_LoaiHangHoa data)
        {
            data.LastModificationTime = DateTime.Now;
            data.NgaySua = DateTime.Now;
            data.LastModifierUserId = AbpSession.UserId;
            data.TrangThai = dto.TrangThai;
            data.TenLoai = dto.TenLoai;
            data.MaLoai = dto.MaLoai;
            data.IsDeleted = false;
            var result = ObjectMapper.Map<LoaiHangHoaDto>(data);
            await _repository.UpdateAsync(data);
            return result;
        }
        public async Task<LoaiHangHoaDto> Delete(Guid id)
        {
            LoaiHangHoaDto result = new LoaiHangHoaDto();
            var checkExist = await _repository.FirstOrDefaultAsync(id);
            if (checkExist != null)
            {
                checkExist.IsDeleted = true;
                checkExist.NgayXoa = DateTime.Now;
                checkExist.DeleterUserId = AbpSession.UserId;
                checkExist.TrangThai = 1;
                checkExist.DeletionTime = DateTime.Now;
                await _repository.UpdateAsync(checkExist);
                result = ObjectMapper.Map<LoaiHangHoaDto>(checkExist);

            }
            return result;
        }
        public async Task<ListResultDto<LoaiHangHoaInfoDto>> GetAllLoaiHangHoaInfo()
        {
            var result = new ListResultDto<LoaiHangHoaInfoDto>();
            List<LoaiHangHoaInfoDto> lstData =new List<LoaiHangHoaInfoDto>();
            var loaiDichVus = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1)&&x.IsDeleted==false).ToListAsync();
            if (loaiDichVus!=null||loaiDichVus.Count>0)
            {
                foreach (var item in loaiDichVus)
                {
                    LoaiHangHoaInfoDto rdo = new LoaiHangHoaInfoDto();
                    rdo.Id = item.Id;
                    rdo.MaLoai = item.MaLoai;
                    rdo.TenLoai = item.TenLoai;
                    var dichVus = _hangHoaRepository.GetAll().Where(x=>x.IdLoaiHangHoa==item.Id && x.TenantId ==(AbpSession.TenantId??1)&& x.IsDeleted==false).Select(x=>x.TenHangHoa).ToList();
                    rdo.DichVus = dichVus;
                    lstData.Add(rdo);    
                }
            }
            result.Items = lstData;
            return result;
        }
    }
}
