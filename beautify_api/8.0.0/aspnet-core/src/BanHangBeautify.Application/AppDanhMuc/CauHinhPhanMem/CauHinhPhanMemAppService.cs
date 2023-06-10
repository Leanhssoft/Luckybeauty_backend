using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.AppDanhMuc.CauHinhPhanMem.Dto;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.CaLamViec.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.CauHinhPhanMem
{
    [AbpAuthorize(PermissionNames.Pages_CauHinhPhanMem)]
    public class CauHinhPhanMemAppService:SPAAppServiceBase
    {
        private readonly IRepository<HT_CauHinhPhanMem,Guid> _repository;
        public CauHinhPhanMemAppService(IRepository<HT_CauHinhPhanMem, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<CauHinhPhanMemDto> CreateOrEdit(CreateOrEditCauHinhDto input)
        {
            var checkExist =await _repository.FirstOrDefaultAsync(x=>x.Id==input.Id);
            if (checkExist==null)
            {
               return await Create(input);
            }
            return await Update(input,checkExist);
        }
        [NonAction]
        public async Task<CauHinhPhanMemDto> Create(CreateOrEditCauHinhDto input)
        {
            HT_CauHinhPhanMem data = new HT_CauHinhPhanMem();
            data = ObjectMapper.Map<HT_CauHinhPhanMem>(data);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            _repository.Insert(data);
            var result = ObjectMapper.Map<CauHinhPhanMemDto>(data);
            return result;
        }
        [NonAction]
        public async Task<CauHinhPhanMemDto> Update(CreateOrEditCauHinhDto input,HT_CauHinhPhanMem oldData)
        {
            oldData.MauInMacDinh = input.MauInMacDinh;
            oldData.SuDungMaChungTu = input.SuDungMaChungTu;
            oldData.KhuyenMai = input.KhuyenMai;
            oldData.IdChiNhanh = input.IdChiNhanh;
            oldData.TichDiem = input.TichDiem;
            oldData.QLKhachHangTheoChiNhanh = input.QLKhachHangTheoChiNhanh;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(oldData);
            var result = ObjectMapper.Map<CauHinhPhanMemDto>(oldData);
            return result;
        }
        [HttpPost]
        public async Task<CauHinhPhanMemDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x=>x.Id==id);
            if (data!=null)
            {
                data.IsDeleted = true;
                data.DeleterUserId= AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                _repository.Update(data);
                return ObjectMapper.Map<CauHinhPhanMemDto>(data);
            }
            return new CauHinhPhanMemDto();
        }
        public async Task<CauHinhPhanMemDto> GetEdit(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x=>x.Id== id);
            if (data!=null)
            {
                return ObjectMapper.Map<CauHinhPhanMemDto>(data);
            }
            return new CauHinhPhanMemDto();
        }
        public async Task<PagedResultDto<CauHinhPhanMemDto>> GetAll(PagedRequestDto input)
        {
            PagedResultDto<CauHinhPhanMemDto> result =new PagedResultDto<CauHinhPhanMemDto>();
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            var data = await _repository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 0)).ToListAsync();
            result.TotalCount = data.Count();
            result.Items = ObjectMapper.Map<List<CauHinhPhanMemDto>>(data);
            return result;
        }
    }
}
