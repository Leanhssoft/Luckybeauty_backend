using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.CauHinh.CauHinhTichDiemChiTiet.Dto;
using BanHangBeautify.ChietKhau.ChietKhauHoaDonChiTiet.Dto;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BanHangBeautify.ChietKhau.ChietKhauHoaDonChiTiet
{
    [AbpAuthorize(PermissionNames.Pages_ChietKhauHoaDon)]
    public class ChietKhauHoaDonChiTietAppService: SPAAppServiceBase
    {
        private readonly IRepository<NS_ChietKhauHoaDon_ChiTiet, Guid> _repository;
        public ChietKhauHoaDonChiTietAppService(IRepository<NS_ChietKhauHoaDon_ChiTiet, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<ChietKhauHDCTDto> CreateOrEdit(CreateOrEditChietKhauHDCTDto input)
        {
            var check = await _repository.FirstOrDefaultAsync(x=>x.Id== input.Id);
            if (check!=null)
            {
                check.IdChietKhauHD = input.IdChietKhauHD;
                check.IdNhanVien = input.IdNhanVien;
                check.LastModificationTime = DateTime.Now;
                check.LastModifierUserId = AbpSession.UserId;
                await _repository.UpdateAsync(check);
            }
            else
            {
                NS_ChietKhauHoaDon_ChiTiet data = new NS_ChietKhauHoaDon_ChiTiet();
                data.Id = Guid.NewGuid();
                data.IdNhanVien = input.IdNhanVien;
                data.IdChietKhauHD = input.IdChietKhauHD;
                data.CreationTime = DateTime.Now;
                data.CreatorUserId = AbpSession.UserId;
                data.TenantId = AbpSession.TenantId ?? 1;
                await _repository.InsertAsync(data);
            }
            return new ChietKhauHDCTDto()
            {
                Id = input.Id,
                IdChietKhauHD = input.IdChietKhauHD,
                IdNhanVien = input.IdNhanVien
            };
        }
        [HttpPost]
        public async Task<ChietKhauHDCTDto> Delete(Guid id)
        {
            var check = await _repository.FirstOrDefaultAsync(x=>x.Id==id);
            if (check!=null)
            {
                check.IsDeleted = true;
                check.DeleterUserId = AbpSession.UserId;
                check.DeletionTime = DateTime.Now;
                _repository.UpdateAsync(check);
                return new ChietKhauHDCTDto()
                {
                    Id = check.Id,
                    IdChietKhauHD = check.IdChietKhauHD,
                    IdNhanVien = check.IdNhanVien
                };
            }
            return new ChietKhauHDCTDto();
        }
        public async Task<CreateOrEditChietKhauHDCTDto> GetForEdit(Guid id)
        {
            var check = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (check != null)
            {
                check.IsDeleted = true;
                check.DeleterUserId = AbpSession.UserId;
                check.DeletionTime = DateTime.Now;
                _repository.UpdateAsync(check);
                return new CreateOrEditChietKhauHDCTDto()
                {
                    Id = check.Id,
                    IdChietKhauHD = check.IdChietKhauHD,
                    IdNhanVien = check.IdNhanVien
                };
            }
            return new CreateOrEditChietKhauHDCTDto();
        }
        public async Task<PagedResultDto<NS_ChietKhauHoaDon_ChiTiet>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<NS_ChietKhauHoaDon_ChiTiet> result = new PagedResultDto<NS_ChietKhauHoaDon_ChiTiet>();
            var lstData = await _repository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = lstData;
            return result;
        }
    }
}
