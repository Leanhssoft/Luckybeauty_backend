using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.Quy.QuyHoaDonChiTiet.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Quy.QuyHoaDonChiTiet
{
    [AbpAuthorize(PermissionNames.Pages_QuyHoaDon)]
    public class QuyHoaDonChiTietAppService : SPAAppServiceBase
    {
        private readonly IRepository<QuyHoaDon_ChiTiet, Guid> _quyHoaDonCTRepository;
        public QuyHoaDonChiTietAppService(IRepository<QuyHoaDon_ChiTiet, Guid> quyHoaDonCTRepository)
        {
            _quyHoaDonCTRepository = quyHoaDonCTRepository;
        }
        public async Task<QuyHoaDonChiTietDto> CreateOrEdit(QuyHoaDonChiTietDto input)
        {
            var checkExist = await _quyHoaDonCTRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist != null)
            {
                return await Update(input, checkExist);
            }
            return await Create(input);
        }
        [NonAction]
        public async Task<QuyHoaDonChiTietDto> Create(QuyHoaDonChiTietDto input)
        {
            QuyHoaDonChiTietDto result = new QuyHoaDonChiTietDto();
            QuyHoaDon_ChiTiet data = new QuyHoaDon_ChiTiet();
            data = ObjectMapper.Map<QuyHoaDon_ChiTiet>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _quyHoaDonCTRepository.InsertAsync(data);
            return result;
        }
        [NonAction]
        public async Task<QuyHoaDonChiTietDto> Update(QuyHoaDonChiTietDto input, QuyHoaDon_ChiTiet oldData)
        {
            QuyHoaDonChiTietDto result = new QuyHoaDonChiTietDto();
            oldData.IdQuyHoaDon = input.IdQuyHoaDon;
            oldData.IdHoaDonLienQuan = input.IdHoaDonLienQuan;
            oldData.IdKhachHang= input.IdKhachHang;
            oldData.IdNhanVien = input.IdNhanVien;
            oldData.IdTaiKhoanNganHang = input.IdTaiKhoanNganHang;
            oldData.IdKhoanThuChi= input.IdKhoanThuChi;
            oldData.LaPTChiPhiNganHang = input.LaPTChiPhiNganHang;
            oldData.ChiPhiNganHang = input.ChiPhiNganHang;
            oldData.ThuPhiTienGui = input.ThuPhiTienGui;
            oldData.DiemThanhToan = input.DiemThanhToan;
            oldData.TienThu = input.TienThu;
            oldData.HinhThucThanhToan = input.HinhThucThanhToan;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _quyHoaDonCTRepository.UpdateAsync(oldData);
            return result;
        }
        [HttpPost]
        public async Task<QuyHoaDonChiTietDto> Delete(Guid id)
        {
            var data = await _quyHoaDonCTRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                await _quyHoaDonCTRepository.UpdateAsync(data);
                return ObjectMapper.Map<QuyHoaDonChiTietDto>(data);
            }
            return new QuyHoaDonChiTietDto();
        }
        public async Task<QuyHoaDonChiTietDto> GetForEdit(Guid id)
        {
            var data = await _quyHoaDonCTRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<QuyHoaDonChiTietDto>(data);
            }
            return new QuyHoaDonChiTietDto();
        }
        public async Task<PagedResultDto<QuyHoaDonChiTietDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<QuyHoaDonChiTietDto> result = new PagedResultDto<QuyHoaDonChiTietDto>();
            var listData = await _quyHoaDonCTRepository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).ToListAsync();
            result.TotalCount = listData.Count;
            listData = listData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<QuyHoaDonChiTietDto>>(listData);
            return result;
        }
    }
}
