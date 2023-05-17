using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.KhuyenMai.KhuyenMaiApDung.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BanHangBeautify.KhuyenMai.KhuyenMaiApDung
{
    [AbpAuthorize(PermissionNames.Pages_KhuyenMai_ApDung)]
    public class KhuyenMaiApDungAppService: SPAAppServiceBase
    {
        private readonly IRepository<DM_KhuyenMai_ApDung,Guid> _khuyenMaiApDungRepository;
        public KhuyenMaiApDungAppService(IRepository<DM_KhuyenMai_ApDung, Guid> khuyenMaiApDungRepository)
        {
            _khuyenMaiApDungRepository = khuyenMaiApDungRepository;
        }
        public async Task<KhuyenMaiApDungDto> CreateOrEdit(CreateOrEditKhuyenMaiApDungDto input)
        {
            var checkExist = await _khuyenMaiApDungRepository.FirstOrDefaultAsync(x=>x.Id== input.Id);
            if (checkExist!=null)
            {
                return await Update(input, checkExist);
            }
            return await Create(input);
        }
        [NonAction]
        public async Task<KhuyenMaiApDungDto> Create(CreateOrEditKhuyenMaiApDungDto input)
        {
            KhuyenMaiApDungDto result = new KhuyenMaiApDungDto();
            DM_KhuyenMai_ApDung data =new  DM_KhuyenMai_ApDung();
            data.Id = Guid.NewGuid();
            data.IdKhuyenMai = input.IdKhuyenMai;
            data.IdNhanVien = input.IdNhanVien;
            data.IdChiNhanh = input.IdChiNhanh;
            data.IdNhomKhach = input.IdNhomKhach;
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.IsDeleted = false;
            data.TenantId = AbpSession.TenantId??1;
            await _khuyenMaiApDungRepository.InsertAsync(data);
            return result;
        }
        [NonAction]
        public async Task<KhuyenMaiApDungDto> Update(CreateOrEditKhuyenMaiApDungDto input,DM_KhuyenMai_ApDung oldData)
        {
            KhuyenMaiApDungDto result = new KhuyenMaiApDungDto();
            oldData.IdKhuyenMai = input.IdKhuyenMai;
            oldData.IdNhanVien = input.IdNhanVien;
            oldData.IdChiNhanh = input.IdChiNhanh;
            oldData.IdNhomKhach = input.IdNhomKhach;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _khuyenMaiApDungRepository.UpdateAsync(oldData);
            return result;
        }
        [HttpPost]
        public async Task<KhuyenMaiApDungDto> Delete(Guid id)
        {
            var data = await _khuyenMaiApDungRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data!=null)
            {
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                await _khuyenMaiApDungRepository.UpdateAsync(data);
                return new KhuyenMaiApDungDto()
                {
                    Id = data.Id,
                    IdChiNhanh = data.IdChiNhanh,
                    IdKhuyenMai = data.IdKhuyenMai,
                    IdNhanVien = data.IdNhanVien,
                    IdNhomKhach = data.IdNhomKhach
                };
            }
            return new KhuyenMaiApDungDto();
        }
        public async Task<CreateOrEditKhuyenMaiApDungDto> GetForEdit(Guid id)
        {
            var data = await _khuyenMaiApDungRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return new CreateOrEditKhuyenMaiApDungDto()
                {
                    Id = data.Id,
                    IdChiNhanh = data.IdChiNhanh,
                    IdKhuyenMai = data.IdKhuyenMai,
                    IdNhanVien = data.IdNhanVien,
                    IdNhomKhach = data.IdNhomKhach
                };
            }
            return new CreateOrEditKhuyenMaiApDungDto();
        }
        public async Task<PagedResultDto<KhuyenMaiApDungDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 0 ? input.SkipCount * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<KhuyenMaiApDungDto> result = new PagedResultDto<KhuyenMaiApDungDto>();
            var listData = await _khuyenMaiApDungRepository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).ToListAsync();
            result.TotalCount = listData.Count;
            listData = listData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            List<KhuyenMaiApDungDto> items = new List<KhuyenMaiApDungDto>();
            foreach (var item in listData)
            {
                KhuyenMaiApDungDto rdo = new KhuyenMaiApDungDto()
                {
                    Id = item.Id,
                    IdChiNhanh = item.IdChiNhanh,
                    IdKhuyenMai = item.IdKhuyenMai,
                    IdNhanVien = item.IdNhanVien,
                    IdNhomKhach = item.IdNhomKhach
                };
                items.Add(rdo);
            }
            result.Items = items;
            return result;
        }
    }
}
