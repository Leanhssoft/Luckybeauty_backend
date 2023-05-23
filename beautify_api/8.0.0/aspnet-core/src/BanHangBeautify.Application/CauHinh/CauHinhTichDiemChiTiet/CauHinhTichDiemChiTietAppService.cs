using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.CauHinh.CauHinhTichDiem.Dto;
using BanHangBeautify.CauHinh.CauHinhTichDiemChiTiet.Dto;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BanHangBeautify.CauHinh.CauHinhTichDiemChiTiet
{
    [AbpAuthorize(PermissionNames.Pages_CauHinhTichDiem)]
    public class CauHinhTichDiemChiTietAppService : SPAAppServiceBase
    {
        private readonly IRepository<HT_CauHinh_TichDiemChiTiet, Guid> _repository;
        public CauHinhTichDiemChiTietAppService(IRepository<HT_CauHinh_TichDiemChiTiet, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<CauHinhTichDiemChiTietDto> CreateOrEdit(CreateOrEditCauHinhTichDiemChiTietDto input)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist != null)
            {
                return await Update(input, checkExist);
            }
            return await Create(input);
        }
        [NonAction]
        public async Task<CauHinhTichDiemChiTietDto> Create(CreateOrEditCauHinhTichDiemChiTietDto input)
        {
            CauHinhTichDiemChiTietDto result = new CauHinhTichDiemChiTietDto();
            HT_CauHinh_TichDiemChiTiet data = new HT_CauHinh_TichDiemChiTiet();
            data.Id = Guid.NewGuid();
            data.IdTichDiem = input.IdTichDiem;
            data.IdNhomKhachHang = input.IdNhomKhachHang;
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _repository.InsertAsync(data);
            result.Id = data.Id;
            result.IdTichDiem = input.IdTichDiem;
            result.IdNhomKhachHang = input.IdNhomKhachHang;
            return result;
        }
        [NonAction]
        public async Task<CauHinhTichDiemChiTietDto> Update(CreateOrEditCauHinhTichDiemChiTietDto input, HT_CauHinh_TichDiemChiTiet data)
        {
            CauHinhTichDiemChiTietDto result = new CauHinhTichDiemChiTietDto();
            data.IdTichDiem = input.IdTichDiem;
            data.IdNhomKhachHang = input.IdNhomKhachHang;
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _repository.UpdateAsync(data);
            result.Id = data.Id;
            result.IdTichDiem = input.IdTichDiem;
            result.IdNhomKhachHang = input.IdNhomKhachHang;
            return result;
        }
        [HttpPost]
        public async Task<CauHinhTichDiemChiTietDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                await _repository.UpdateAsync(data);
                return new CauHinhTichDiemChiTietDto()
                {
                    Id = data.Id,
                    IdNhomKhachHang = data.IdNhomKhachHang,
                    IdTichDiem = data.IdTichDiem
                };
            }
            return new CauHinhTichDiemChiTietDto();

        }
        public async Task<CauHinhTichDiemChiTietDto> GetForEdit(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return new CauHinhTichDiemChiTietDto()
                {
                    Id = data.Id,
                    IdNhomKhachHang = data.IdNhomKhachHang,
                    IdTichDiem = data.IdTichDiem
                };
            }
            return new CauHinhTichDiemChiTietDto();
        }
        public async Task<PagedResultDto<CauHinhTichDiemChiTietDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 0 ? input.SkipCount * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<CauHinhTichDiemChiTietDto> result = new PagedResultDto<CauHinhTichDiemChiTietDto>();
            var lstData = await _repository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            List<CauHinhTichDiemChiTietDto> items = new List<CauHinhTichDiemChiTietDto>();
            foreach (var item in lstData)
            {
                items.Add(new CauHinhTichDiemChiTietDto()
                {
                    Id = item.Id,
                    IdNhomKhachHang = item.IdNhomKhachHang,
                    IdTichDiem = item.IdTichDiem
                });
            }
            result.Items = ObjectMapper.Map<List<CauHinhTichDiemChiTietDto>>(lstData);
            return result;
        }
    }
}
