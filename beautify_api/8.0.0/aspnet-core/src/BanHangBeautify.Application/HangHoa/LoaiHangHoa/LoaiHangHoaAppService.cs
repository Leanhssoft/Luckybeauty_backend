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
        private readonly IRepository<DM_LoaiHangHoa, int> _repository;
        private readonly IRepository<DM_HangHoa, Guid> _hangHoaRepository;
        public LoaiHangHoaAppService(IRepository<DM_LoaiHangHoa, int> repository, IRepository<DM_HangHoa, Guid> hangHoaRepository)
        {
            _repository = repository;
            _hangHoaRepository = hangHoaRepository;
        }
        public async Task<PagedResultDto<LoaiHangHoaDto>> GetAll(LoaiHangHoaPagedResultRequestDto input)
        {
            PagedResultDto<LoaiHangHoaDto> result = new PagedResultDto<LoaiHangHoaDto>();
            var loaiHangHoas = await _repository.GetAll().Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = loaiHangHoas.Count;
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                loaiHangHoas = loaiHangHoas.Where(x => x.TenLoaiHangHoa.Contains(input.Keyword) || x.MaLoaiHangHoa.Contains(input.Keyword)).ToList();
            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            loaiHangHoas = loaiHangHoas.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var data = ObjectMapper.Map<List<LoaiHangHoaDto>>(loaiHangHoas);
            result.Items = data;
            return result;
        }
        public async Task<DM_LoaiHangHoa> GetDetail(int id)
        {
            return await _repository.GetAsync(id);
        }
        [AbpAuthorize(PermissionNames.Pages_DM_LoaiHangHoa_Create, PermissionNames.Pages_DM_LoaiHangHoa_Edit)]
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
            var maxId = _repository.Count();
            DM_LoaiHangHoa data = new DM_LoaiHangHoa();
            data.Id = maxId + 1;
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId;
            data.TrangThai = 0;
            data.TenLoaiHangHoa = dto.TenLoai;
            data.MaLoaiHangHoa = dto.MaLoai;
            data.IsDeleted = false;
            var result = ObjectMapper.Map<LoaiHangHoaDto>(data);
            await _repository.InsertAsync(data);
            return result;
        }
        [NonAction]
        public async Task<LoaiHangHoaDto> Edit(CreateOrEditLoaiHangHoaDto dto, DM_LoaiHangHoa data)
        {
            data.LastModificationTime = DateTime.Now;
            data.LastModifierUserId = AbpSession.UserId;
            data.TrangThai = dto.TrangThai;
            data.TenLoaiHangHoa = dto.TenLoai;
            data.MaLoaiHangHoa = dto.MaLoai;
            data.IsDeleted = false;
            var result = ObjectMapper.Map<LoaiHangHoaDto>(data);
            await _repository.UpdateAsync(data);
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_DM_LoaiHangHoa_Delete)]
        public async Task<LoaiHangHoaDto> Delete(int id)
        {
            LoaiHangHoaDto result = new LoaiHangHoaDto();
            var checkExist = await _repository.FirstOrDefaultAsync(id);
            if (checkExist != null)
            {
                checkExist.IsDeleted = true;
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
            List<LoaiHangHoaInfoDto> lstData = new List<LoaiHangHoaInfoDto>();
            var loaiDichVus = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (loaiDichVus != null || loaiDichVus.Count > 0)
            {
                foreach (var item in loaiDichVus)
                {
                    LoaiHangHoaInfoDto rdo = new LoaiHangHoaInfoDto();
                    rdo.Id = item.Id;
                    rdo.MaLoaiHangHoa = item.MaLoaiHangHoa;
                    rdo.TenLoaiHangHoa = item.TenLoaiHangHoa;
                    var dichVus = _hangHoaRepository.GetAll().Where(x => x.IdLoaiHangHoa == item.Id && x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).Select(x => x.TenHangHoa).ToList();
                    rdo.DichVus = dichVus;
                    lstData.Add(rdo);
                }
            }
            result.Items = lstData;
            return result;
        }
    }
}
