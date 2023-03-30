

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.DonViQuiDoi.Dto;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.HangHoa.HangHoa.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.HangHoa
{
    //[AbpAuthorize(PermissionNames.Pages_DM_LoaiHangHoa)]
    public class HangHoaAppService : SPAAppServiceBase, IHangHoaAppService
    {
        private readonly IRepository<DM_HangHoa, Guid> _dmHangHoa;
        private readonly IRepository<DM_DonViQuiDoi, Guid> _dmDonViQuiDoi;
        private readonly IHangHoaRepository _repository;
        public HangHoaAppService(IRepository<DM_HangHoa, Guid> repository,
            IHangHoaRepository productRepo,
            IRepository<DM_DonViQuiDoi, Guid> dvqd
            )
        {
            _dmHangHoa = repository;
            _dmDonViQuiDoi = dvqd;
            _repository = productRepo;
        }
        public async Task<CreateOrEditHangHoaDto> CreateOrEdit(CreateOrEditHangHoaDto dto)
        {
            var findHangHoa = await _dmHangHoa.FirstOrDefaultAsync(h => h.Id == dto.Id);
            if (findHangHoa == null)
            {
                return await Create(dto);
            }
            else
            {
                return await Edit(dto, findHangHoa);
            }
        }
        [NonAction]
        public async Task<CreateOrEditHangHoaDto> Create(CreateOrEditHangHoaDto dto)
        {
            List<DM_DonViQuiDoi> lstDVT = new();
            DM_HangHoa hangHoa = ObjectMapper.Map<DM_HangHoa>(dto);
            Guid productId = Guid.NewGuid();
            hangHoa.Id = productId;
            hangHoa.TenantId = AbpSession.TenantId ?? 1;
            hangHoa.CreatorUserId = AbpSession.UserId;
            hangHoa.CreationTime = DateTime.Now;
            await _dmHangHoa.InsertAsync(hangHoa);

            if (dto.DonViQuiDois != null && dto.DonViQuiDois.Count > 0)
            {
                foreach (var item in dto.DonViQuiDois)
                {
                    DM_DonViQuiDoi dvt = ObjectMapper.Map<DM_DonViQuiDoi>(item);
                    dvt.Id = Guid.NewGuid();
                    dvt.TenantId = hangHoa.TenantId;
                    dvt.IdHangHoa = productId;
                    dvt.MaHangHoa = await _repository.GetProductCode(dto.IdLoaiHangHoa, hangHoa.TenantId);
                    lstDVT.Add(dvt);
                    await _dmDonViQuiDoi.InsertAsync(dvt);
                }
            }
            else
            {
                DM_DonViQuiDoi dvt = new()
                {
                    Id = Guid.NewGuid(),
                    IdHangHoa = productId,
                    TenantId = hangHoa.TenantId,
                    MaHangHoa = await _repository.GetProductCode(dto.IdLoaiHangHoa, hangHoa.TenantId),
                    TenDonViTinh = string.Empty,
                };
                lstDVT.Add(dvt);
                await _dmDonViQuiDoi.InsertAsync(dvt);
            }

            hangHoa.DonViQuiDois = lstDVT;
            var result = ObjectMapper.Map<CreateOrEditHangHoaDto>(hangHoa);
            return result;
        }

        [NonAction]
        public async Task<CreateOrEditHangHoaDto> Edit(CreateOrEditHangHoaDto dto, DM_HangHoa hangHoa)
        {
            hangHoa.IdLoaiHangHoa = dto.IdLoaiHangHoa;
            hangHoa.TenHangHoa = dto.TenHangHoa;
            hangHoa.TrangThai = dto.TrangThai;
            hangHoa.LastModificationTime = DateTime.Now;
            hangHoa.LastModifierUserId = AbpSession.UserId;
            var result = ObjectMapper.Map<CreateOrEditHangHoaDto>(hangHoa);
            await _dmHangHoa.UpdateAsync(hangHoa);
            return result;
        }
        public async Task<DM_HangHoa> getDetail(Guid id)
        {
            return await _dmHangHoa.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<PagedResultDto<DM_HangHoa>> GetAll(HangHoaPagedResultRequestDto input)
        {
            PagedResultDto<DM_HangHoa> result = new PagedResultDto<DM_HangHoa>();
            var lstHangHoa = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstHangHoa.Count;
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                lstHangHoa = lstHangHoa.Where(x => x.TenHangHoa.Contains(input.CommonParam.TextSearch) || x.TenHangHoa.Contains(input.CommonParam.TextSearch)).ToList();
            }
            if (input.SkipCount > 0)
            {
                input.SkipCount *= 10;
            }
            result.Items = lstHangHoa.Skip(input.SkipCount).Take(input.SkipCount).ToList();
            return result;
        }
        public async Task<PagedResultDto<HangHoaDto>> GetDMHangHoa(HangHoaPagedResultRequestDto input)
        {
            return await _repository.GetDMHangHoa(input, AbpSession.TenantId ?? 1);
        }
        [HttpPost]
        public async Task<CreateOrEditHangHoaDto> Delete(Guid id)
        {
            CreateOrEditHangHoaDto result = new();
            var findHangHoa = await _dmHangHoa.FirstOrDefaultAsync(h => h.Id == id);
            if (findHangHoa != null)
            {
                findHangHoa.IsDeleted = true;
                findHangHoa.TrangThai = 0;
                findHangHoa.DeletionTime = DateTime.Now;
                findHangHoa.DeleterUserId = AbpSession.UserId;
                _dmHangHoa.Update(findHangHoa);
                result = ObjectMapper.Map<CreateOrEditHangHoaDto>(findHangHoa);
            }
            return result;
        }
    }
}
