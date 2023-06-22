using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using BanHangBeautify.Quy.DM_QuyHoaDon.Dto;
using BanHangBeautify.Quy.DM_QuyHoaDon.Dto.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BanHangBeautify.Quy.DM_QuyHoaDon
{
    //[AbpAuthorize(PermissionNames.Pages_QuyHoaDon)]
    public class QuyHoaDonAppService: SPAAppServiceBase
    {
        private readonly IRepository<QuyHoaDon, Guid> _quyHoaDonRepository;
        private readonly IRepository<QuyHoaDon_ChiTiet, Guid> _quyHoaDonChiTiet;
        private readonly IRepository<DM_LoaiChungTu, int> _loaiChungTuRepository;
        private readonly IQuyHoaDonRepository _repoQuyHD;
        public QuyHoaDonAppService(IRepository<QuyHoaDon, Guid> quyHoaDonRepository, IRepository<DM_LoaiChungTu, int> loaiChungTuRepository,
            IRepository<QuyHoaDon_ChiTiet, Guid> quyHoaDonChiTiet,
            IQuyHoaDonRepository repoQuyHD)
        {
            _quyHoaDonRepository = quyHoaDonRepository;
            _loaiChungTuRepository = loaiChungTuRepository;
            _quyHoaDonChiTiet = quyHoaDonChiTiet;
            _repoQuyHD = repoQuyHD;
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
       
        public async Task<QuyHoaDonDto> Create(CreateOrEditQuyHoaDonDto input)
        {
            List<QuyHoaDon_ChiTiet> lstCT = new ();

            if (string.IsNullOrEmpty(input.MaHoaDon))
            {
                var maChungTu = await _repoQuyHD.FnGetMaPhieuThuChi(AbpSession.TenantId ?? 1, input.IdChiNhanh, input.IdLoaiChungTu, input.NgayLapHoaDon);
                input.MaHoaDon = maChungTu;
            }
            QuyHoaDon data = ObjectMapper.Map<QuyHoaDon>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;

            // insert quyct
            if (input.QuyHoaDon_ChiTiet!=null && input.QuyHoaDon_ChiTiet.Count> 0)
            {
                foreach (var item in input.QuyHoaDon_ChiTiet)
                {
                    QuyHoaDon_ChiTiet ctNew = ObjectMapper.Map<QuyHoaDon_ChiTiet>(item);
                    ctNew.Id = Guid.NewGuid();
                    ctNew.IdQuyHoaDon = data.Id;
                    ctNew.TenantId = AbpSession.TenantId ?? 1;
                    ctNew.CreatorUserId = AbpSession.UserId;
                    ctNew.CreationTime = DateTime.Now;
                    lstCT.Add(ctNew);
                }
            }
            await _quyHoaDonRepository.InsertAsync(data);
            await _quyHoaDonChiTiet.InsertRangeAsync(lstCT);

            var result = ObjectMapper.Map<QuyHoaDonDto>(data);
            return result;
        }

        public async Task<QuyHoaDonDto> UpdateQuyHoaDon(CreateOrEditQuyHoaDonDto input)
        {
            List<QuyHoaDon_ChiTiet> lstCT = new();
            var oldData = await _quyHoaDonRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (string.IsNullOrEmpty(input.MaHoaDon))
            {
                var maChungTu = await _repoQuyHD.FnGetMaPhieuThuChi(AbpSession.TenantId ?? 1, input.IdChiNhanh, input.IdLoaiChungTu, input.NgayLapHoaDon);
                input.MaHoaDon = maChungTu;
            }
            oldData = ObjectMapper.Map<QuyHoaDon>(input);
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;

            // remove quyct + add again
            var lstRemove = await _quyHoaDonChiTiet.GetAll().Where(x => x.IdQuyHoaDon == input.Id).ToListAsync();
            _quyHoaDonChiTiet.RemoveRange(lstRemove);

            if (input.QuyHoaDon_ChiTiet != null && input.QuyHoaDon_ChiTiet.Count > 0)
            {
                foreach (var item in input.QuyHoaDon_ChiTiet)
                {
                    QuyHoaDon_ChiTiet ctNew = ObjectMapper.Map<QuyHoaDon_ChiTiet>(item);
                    ctNew.Id = Guid.NewGuid();
                    ctNew.IdQuyHoaDon = input.Id;
                    ctNew.TenantId = AbpSession.TenantId ?? 1;
                    ctNew.CreatorUserId = AbpSession.UserId;
                    ctNew.CreationTime = DateTime.Now;
                    lstCT.Add(ctNew);
                }
            }

            await _quyHoaDonRepository.UpdateAsync(oldData);
            await _quyHoaDonChiTiet.InsertRangeAsync(lstCT);

            var result = ObjectMapper.Map<QuyHoaDonDto>(input);
            return result;
        }

        [NonAction]
        public async Task<QuyHoaDonDto> Update(CreateOrEditQuyHoaDonDto input, QuyHoaDon oldData)
        {
            if (string.IsNullOrEmpty(input.MaHoaDon))
            {
                var maChungTu = await _repoQuyHD.FnGetMaPhieuThuChi(AbpSession.TenantId ?? 1, input.IdChiNhanh, input.IdLoaiChungTu, input.NgayLapHoaDon);
                input.MaHoaDon = maChungTu;
            }
            oldData = ObjectMapper.Map<QuyHoaDon>(input);
            //oldData.NgayLapHoaDon = input.NgayLapHoaDon;
            //oldData.MaHoaDon = input.MaHoaDon;
            //oldData.IdChiNhanh = input.IdChiNhanh;
            //oldData.HachToanKinhDoanh = input.HachToanKinhDoanh;
            //oldData.IdLoaiChungTu = input.IdLoaiChungTu;
            //oldData.NoiDungThu = input.NoiDungThu;
            //oldData.TongTienThu = input.TongTienThu;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _quyHoaDonRepository.UpdateAsync(oldData);

            var result = ObjectMapper.Map<QuyHoaDonDto>(input);
            return result;
        }
        public async Task<CreateOrEditQuyHoaDonDto> CreateQuyHoaDon([FromBody] JObject data)
        {
            List<QuyHoaDon_ChiTiet> lstCTQuy = new();
            QuyHoaDon objHD = ObjectMapper.Map<QuyHoaDon>(data["soquy"].ToObject<QuyHoaDon>());
            List<QuyHoaDon_ChiTiet> dataChiTietHD = ObjectMapper.Map<List<QuyHoaDon_ChiTiet>>(data["soquyChiTiet"].ToObject<List<QuyHoaDon_ChiTiet>>());

            objHD.Id = Guid.NewGuid();
            objHD.TenantId = AbpSession.TenantId ?? 1;
            objHD.CreatorUserId = AbpSession.UserId;
            objHD.CreationTime = DateTime.Now;

            if (string.IsNullOrEmpty(objHD.MaHoaDon))
            {
                var maChungTu = await _repoQuyHD.FnGetMaPhieuThuChi(AbpSession.TenantId ?? 1, objHD.IdChiNhanh ?? null,
                    objHD.IdLoaiChungTu, objHD.NgayLapHoaDon);
                objHD.MaHoaDon = maChungTu;
            }
            foreach (var item in dataChiTietHD)
            {
                QuyHoaDon_ChiTiet ctNew = ObjectMapper.Map<QuyHoaDon_ChiTiet>(item);
                ctNew.Id = Guid.NewGuid();
                ctNew.IdQuyHoaDon = objHD.Id;
                ctNew.TenantId = AbpSession.TenantId ?? 1;
                ctNew.CreatorUserId = AbpSession.UserId;
                ctNew.CreationTime = DateTime.Now;
                lstCTQuy.Add(ctNew);
            }
            await _quyHoaDonRepository.InsertAsync(objHD);
            await _quyHoaDonChiTiet.InsertRangeAsync(lstCTQuy);

            var result = ObjectMapper.Map<CreateOrEditQuyHoaDonDto>(objHD);
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
        public async Task<List<QuyHoaDonViewItemDto>> GetNhatKyThanhToan_ofHoaDon(Guid idHoadonLienQuan)
        {
            var data = await _repoQuyHD.GetNhatKyThanhToan_ofHoaDon(idHoadonLienQuan);
            return data;
        }
    }
}
