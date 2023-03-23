using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.QuaTrinhCongTac.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.QuaTrinhCongTac
{
    [AbpAuthorize(PermissionNames.Pages_QuaTrinhCongTac)]
    public class NS_QuaTrinhCongTacAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_QuaTrinh_CongTac, Guid> _repository;
        public NS_QuaTrinhCongTacAppService(IRepository<NS_QuaTrinh_CongTac, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<QuaTrinhConTacDto> CreateOrEdit(CreateOrEditQuaTrinhConTacDto dto)
        {
            var quaTrinhCongTac = await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (quaTrinhCongTac == null)
            {
                return await Create(dto);
            }
            else
                return await Edit(dto, quaTrinhCongTac);
        }
        [NonAction]
        public async Task<QuaTrinhConTacDto> Create(CreateOrEditQuaTrinhConTacDto dto)
        {
            NS_QuaTrinh_CongTac data = new NS_QuaTrinh_CongTac();
            data.Id = Guid.NewGuid();
            data.TuNgay = dto.TuNgay;
            data.DenNgay = dto.DenNgay;
            data.IdNhanVien = dto.IdNhanVien;
            data.IdChiNhanh = dto.IdChiNhanh;
            data.IdPhongBan = dto.IdPhongBan;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.CreatorUserId = AbpSession.UserId;
            data.TrangThai = 0;
            data.CreationTime = DateTime.Now;
            data.NgayTao = DateTime.Now;
            var result = ObjectMapper.Map<QuaTrinhConTacDto>(data);
            await _repository.InsertAsync(data);
            return result;
        }
        [NonAction]
        public async Task<QuaTrinhConTacDto> Edit(CreateOrEditQuaTrinhConTacDto dto, NS_QuaTrinh_CongTac data)
        {
            data.TuNgay = dto.TuNgay;
            data.DenNgay = dto.DenNgay;
            data.IdNhanVien = dto.IdNhanVien;
            data.IdChiNhanh = dto.IdChiNhanh;
            data.IdPhongBan = dto.IdPhongBan;
            data.LastModifierUserId = AbpSession.UserId;
            data.TrangThai = 0;
            data.LastModificationTime = DateTime.Now;
            data.NgaySua = DateTime.Now;
            var result = ObjectMapper.Map<QuaTrinhConTacDto>(data);
            await _repository.UpdateAsync(data);
            return result;
        }
        [HttpPost]
        public async Task<QuaTrinhConTacDto> Delete(Guid Id)
        {
            var quaTrinhCongTac = await _repository.FirstOrDefaultAsync(x => x.Id == Id);
            if (quaTrinhCongTac != null)
            {
                quaTrinhCongTac.TrangThai = 1;
                quaTrinhCongTac.IsDeleted = true;
                quaTrinhCongTac.DeleterUserId = AbpSession.UserId;
                quaTrinhCongTac.DeletionTime = DateTime.Now;
                await _repository.UpdateAsync(quaTrinhCongTac);
                return ObjectMapper.Map<QuaTrinhConTacDto>(quaTrinhCongTac);
            }
            return new QuaTrinhConTacDto();
        }
        [HttpPost]
        public async Task DeleteByUser(Guid IdNhanSu)
        {
            var lstQuaTrinhCongTac = await _repository.GetAllListAsync();
            lstQuaTrinhCongTac = lstQuaTrinhCongTac.Where(x => x.IdNhanVien == IdNhanSu).ToList();
            foreach (var item in lstQuaTrinhCongTac)
            {
                item.TrangThai = 1;
                item.IsDeleted = true;
                item.DeleterUserId = AbpSession.UserId;
                item.DeletionTime = DateTime.Now;
                await _repository.UpdateAsync(item);
            }
        }
        public async Task<NS_QuaTrinh_CongTac> GetDetail(Guid id)
        {
            return await _repository.GetAsync(id);
        }
        public async Task<ListResultDto<NS_QuaTrinh_CongTac>> GetAll(PagedResultRequestDto input, string keyWord)
        {
            ListResultDto<NS_QuaTrinh_CongTac> result = new ListResultDto<NS_QuaTrinh_CongTac>();
            var lstQuaTrinhCongTac = await _repository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.IsDeleted == false).ToListAsync();
            if (!string.IsNullOrEmpty(keyWord))
            {
                lstQuaTrinhCongTac = lstQuaTrinhCongTac.ToList();
            }
            input.SkipCount = input.SkipCount > 0 ? input.SkipCount * 10 : 0;
            input.MaxResultCount = 10;
            result.Items = lstQuaTrinhCongTac.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return result;
        }
    }
}
