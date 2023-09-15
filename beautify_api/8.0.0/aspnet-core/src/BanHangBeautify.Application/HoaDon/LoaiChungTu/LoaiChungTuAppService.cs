using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.ChungTu.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.ChungTu
{
    [AbpAuthorize(PermissionNames.Pages_LoaiChungTu)]
    public class LoaiChungTuAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_LoaiChungTu> _loaiChungTuRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public LoaiChungTuAppService(IRepository<DM_LoaiChungTu> loaiChungTuRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _loaiChungTuRepository = loaiChungTuRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [AbpAuthorize(PermissionNames.Pages_LoaiChungTu_Create, PermissionNames.Pages_LoaiChungTu_Delete)]
        public async Task<LoaiChungTuDto> CreateOrEdit(CreateOrEditLoaiChungTuDto input)
        {
            var checkExist = await _loaiChungTuRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist == null)
            {
                return await Create(input);
            }
            return await Update(input, checkExist);
        }
        [NonAction]
        public async Task<LoaiChungTuDto> Create(CreateOrEditLoaiChungTuDto input)
        {
            LoaiChungTuDto result = new LoaiChungTuDto();
            DM_LoaiChungTu data = new DM_LoaiChungTu();
            var maxId = _loaiChungTuRepository.Count() + 1;
            data.Id = maxId;
            data.TenLoaiChungTu = input.TenLoaiChungTu;
            data.MaLoaiChungTu = input.MaLoaiChungTu;
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId;
            data.IsDeleted = false;
            _loaiChungTuRepository.Insert(data);
            result = ObjectMapper.Map<LoaiChungTuDto>(data);
            return result;
        }
        [NonAction]
        public async Task<LoaiChungTuDto> Update(CreateOrEditLoaiChungTuDto input, DM_LoaiChungTu oldData)
        {
            LoaiChungTuDto result = new LoaiChungTuDto();
            oldData.MaLoaiChungTu = input.MaLoaiChungTu;
            oldData.TenLoaiChungTu = input.TenLoaiChungTu;
            oldData.TrangThai = input.TrangThai;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            _loaiChungTuRepository.Update(oldData);
            result = ObjectMapper.Map<LoaiChungTuDto>(oldData);
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_LoaiChungTu_Delete)]
        public async Task<LoaiChungTuDto> Delete(int id)
        {
            var checkExist = await _loaiChungTuRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (checkExist != null)
            {
                checkExist.IsDeleted = true;
                checkExist.DeleterUserId = AbpSession.UserId;
                checkExist.DeletionTime = DateTime.Now;
                _loaiChungTuRepository.Update(checkExist);
                return ObjectMapper.Map<LoaiChungTuDto>(checkExist);
            }
            return new LoaiChungTuDto();
        }
        public async Task<LoaiChungTuDto> GetDetail(int id)
        {
            var checkExist = await _loaiChungTuRepository.FirstOrDefaultAsync(x => x.Id == id);
            return ObjectMapper.Map<LoaiChungTuDto>(checkExist) as LoaiChungTuDto;

        }
        public async Task<PagedResultDto<DM_LoaiChungTu>> GetAll()
        {
            PagedResultDto<DM_LoaiChungTu> result = new PagedResultDto<DM_LoaiChungTu>();
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))// tắt bộ lọc tenantId
            {
                var data = await _loaiChungTuRepository.GetAll().Where(x => x.IsDeleted == false).ToListAsync();
                result.TotalCount = data.Count;
                result.Items = data;
            }
            return result;
        }

    }
}
