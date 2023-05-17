using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.Quy.DM_QuyHoaDon.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.DM_QuyHoaDon
{
    [AbpAuthorize(PermissionNames.Pages_QuyHoaDon)]
    public class QuyHoaDonAppService: SPAAppServiceBase
    {
        private readonly IRepository<QuyHoaDon, Guid> _quyHoaDonRepository;
        private readonly IRepository<DM_LoaiChungTu, int> _loaiChungTuRepository;
        public QuyHoaDonAppService(IRepository<QuyHoaDon, Guid> quyHoaDonRepository, IRepository<DM_LoaiChungTu, int> loaiChungTuRepository)
        {
            _quyHoaDonRepository = quyHoaDonRepository;
            _loaiChungTuRepository = loaiChungTuRepository;
        }
        public async Task<QuyHoaDonDto> CreateOrEdit(CreateOrEditQuyHoaDonDto input)
        {
            var checkExist = await _quyHoaDonRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist != null)
            {
                return await Update(input, checkExist);
            }
            return await Create(input);
        }
        [NonAction]
        public async Task<QuyHoaDonDto> Create(CreateOrEditQuyHoaDonDto input)
        {
            QuyHoaDonDto result = new QuyHoaDonDto();
            QuyHoaDon data = new QuyHoaDon();
            var loaiChungTu = await _loaiChungTuRepository.FirstOrDefaultAsync(x => x.Id == input.IdLoaiChungTu);
            var maxQuyHoaDon = await _quyHoaDonRepository.GetAll().CountAsync();
            data = ObjectMapper.Map<QuyHoaDon>(input);
            data.Id = Guid.NewGuid();
            data.MaHoaDon = string.IsNullOrEmpty(input.MaHoaDon) ? loaiChungTu.MaLoaiChungTu + "00" + (maxQuyHoaDon+1).ToString()  : input.MaHoaDon;
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _quyHoaDonRepository.InsertAsync(data);
            return result;
        }
        [NonAction]
        public async Task<QuyHoaDonDto> Update(CreateOrEditQuyHoaDonDto input, QuyHoaDon oldData)
        {
            QuyHoaDonDto result = new QuyHoaDonDto();
            oldData.NgayLapHoaDon = input.NgayLapHoaDon;
            oldData.MaHoaDon = input.MaHoaDon;
            oldData.IdChiNhanh = input.IdChiNhanh;
            oldData.HachToanKinhDoanh = input.HachToanKinhDoanh;
            oldData.IdLoaiChungTu = input.IdLoaiChungTu;
            oldData.NoiDungThu = input.NoiDungThu;
            oldData.TongTienThu = input.TongTienThu;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _quyHoaDonRepository.UpdateAsync(oldData);
            return result;
        }
        [HttpPost]
        public async Task<QuyHoaDonDto> Delete(Guid id)
        {
            var data = await _quyHoaDonRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                await _quyHoaDonRepository.UpdateAsync(data);
                return ObjectMapper.Map<QuyHoaDonDto>(data);    
            }
            return new QuyHoaDonDto();
        }
        public async Task<CreateOrEditQuyHoaDonDto> GetForEdit(Guid id)
        {
            var data = await _quyHoaDonRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditQuyHoaDonDto>(data);
            }
            return new CreateOrEditQuyHoaDonDto();
        }
        public async Task<PagedResultDto<QuyHoaDonDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 0 ? input.SkipCount * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<QuyHoaDonDto> result = new PagedResultDto<QuyHoaDonDto>();
            var listData = await _quyHoaDonRepository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).ToListAsync();
            result.TotalCount = listData.Count;
            listData = listData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<QuyHoaDonDto>>(listData);
            return result;
        }
    }
}
