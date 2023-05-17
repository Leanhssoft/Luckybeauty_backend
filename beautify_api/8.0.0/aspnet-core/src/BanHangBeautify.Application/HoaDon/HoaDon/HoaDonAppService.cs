using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Common.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BanHangBeautify.HoaDon.HoaDon.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BanHangBeautify.HangHoa.HangHoa.Repository;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BanHangBeautify.HoaDon.HoaDon
{
    [AbpAuthorize(PermissionNames.Pages_HoaDon)]
    public class HoaDonAppService : SPAAppServiceBase
    {
        private readonly IRepository<BH_HoaDon, Guid> _hoaDonRepository;
        private readonly IRepository<BH_HoaDon_ChiTiet, Guid> _hoaDonChiTietRepository;
        private readonly IRepository<BH_HoaDon_Anh, Guid> _hoaDonAnhRepository;
        private readonly IRepository<DM_LoaiChungTu, int> _loaiChungTuRepository;
        private readonly IHoaDonRepository _repoHoaDon;
        public HoaDonAppService(
            IRepository<BH_HoaDon, Guid> hoaDonRepository, 
            IRepository<DM_LoaiChungTu, int> loaiChungTuRepository, 
            IRepository<BH_HoaDon_ChiTiet, Guid> hoaDonChiTietRepository, 
            IRepository<BH_HoaDon_Anh, Guid> hoaDonAnhRepository,
            IHoaDonRepository repoHoaDon
        )
        {
            _hoaDonRepository = hoaDonRepository;
            _loaiChungTuRepository = loaiChungTuRepository;
            _hoaDonChiTietRepository = hoaDonChiTietRepository;
            _hoaDonAnhRepository = hoaDonAnhRepository;
            _repoHoaDon = repoHoaDon;
        }
        public async Task<CreateHoaDonDto> CreateHoaDon(CreateHoaDonDto dto)
        {
            List<BH_HoaDon_ChiTiet> lstCTHoaDon = new();
            BH_HoaDon objHD = ObjectMapper.Map<BH_HoaDon>(dto);

            objHD.Id = Guid.NewGuid();
            objHD.TenantId = AbpSession.TenantId ?? 1;
            objHD.CreatorUserId = AbpSession.UserId;
            objHD.CreationTime = DateTime.Now;

            if (string.IsNullOrEmpty(objHD.MaHoaDon))
            {
                var maChungTu = await _repoHoaDon.FnGetMaHoaDon(AbpSession.TenantId ?? 1, dto.IdChiNhanh, dto.IdLoaiChungTu, dto.NgayLapHoaDon);
                objHD.MaHoaDon = maChungTu;
            }
            foreach (var item in dto.BH_HoaDon_ChiTiet)
            {
                BH_HoaDon_ChiTiet ctNew = ObjectMapper.Map<BH_HoaDon_ChiTiet>(item);
                ctNew.Id = Guid.NewGuid();
                ctNew.IdHoaDon = objHD.Id;
                ctNew.TenantId = AbpSession.TenantId ?? 1;
                ctNew.CreatorUserId = AbpSession.UserId;
                ctNew.CreationTime = DateTime.Now;
                lstCTHoaDon.Add(ctNew);
                // toddo NVThucHien
            }
            await _hoaDonRepository.InsertAsync(objHD);
            await _hoaDonChiTietRepository.InsertRangeAsync(lstCTHoaDon);

            objHD.BH_HoaDon_ChiTiet = lstCTHoaDon;
            var result = ObjectMapper.Map<CreateHoaDonDto>(objHD);
            return result;
        }

        public async Task<CreateHoaDonDto> CreateHoaDon2([FromBody] JObject data)
        {
            List<BH_HoaDon_ChiTiet> lstCTHoaDon = new();
            BH_HoaDon objHD = ObjectMapper.Map<BH_HoaDon>(data["hoadon"].ToObject<BH_HoaDon>());
            List<BH_HoaDon_ChiTiet> dataChiTietHD = ObjectMapper.Map<List<BH_HoaDon_ChiTiet>>(data["hoadonChiTiet"].ToObject<List<BH_HoaDon_ChiTiet>>());

            objHD.Id = Guid.NewGuid();
            objHD.TenantId = AbpSession.TenantId ?? 1;
            objHD.CreatorUserId = AbpSession.UserId;
            objHD.CreationTime = DateTime.Now;

            if (string.IsNullOrEmpty(objHD.MaHoaDon))
            {
                var maChungTu = await _repoHoaDon.FnGetMaHoaDon(AbpSession.TenantId ?? 1, objHD.IdChiNhanh??null,
                    objHD.IdLoaiChungTu, objHD.NgayLapHoaDon);
                objHD.MaHoaDon = maChungTu;
            }
            foreach (var item in dataChiTietHD)
            {
                BH_HoaDon_ChiTiet ctNew = ObjectMapper.Map<BH_HoaDon_ChiTiet>(item);
                ctNew.Id = Guid.NewGuid();
                ctNew.IdHoaDon = objHD.Id;
                ctNew.TenantId = AbpSession.TenantId ?? 1;
                ctNew.CreatorUserId = AbpSession.UserId;
                ctNew.CreationTime = DateTime.Now;
                lstCTHoaDon.Add(ctNew);
                // toddo NVThucHien
            }
            await _hoaDonRepository.InsertAsync(objHD);
            await _hoaDonChiTietRepository.InsertRangeAsync(lstCTHoaDon);

            objHD.BH_HoaDon_ChiTiet = lstCTHoaDon;
            var result = ObjectMapper.Map<CreateHoaDonDto>(objHD);
            return result;
            }
        public async Task<string> UpdateHoaDon([FromBody] JObject data)
        {
            List<BH_HoaDon_ChiTiet> lstCTHoaDon = new();
            BH_HoaDon objUp = ObjectMapper.Map<BH_HoaDon>(data["hoadon"].ToObject<BH_HoaDon>());
            List<BH_HoaDon_ChiTiet> dataChiTietHD = ObjectMapper.Map<List<BH_HoaDon_ChiTiet>>(data["hoadonChiTiet"].ToObject<List<BH_HoaDon_ChiTiet>>());
            if (data["hoadon"] == null)
            {
                return "object null";
            }
            objUp = await _hoaDonRepository.FirstOrDefaultAsync(objUp.Id);

            if (objUp == null)
            {
                return "object null";
        }

            if (string.IsNullOrEmpty(objUp.MaHoaDon))
        {
                objUp.MaHoaDon = await _repoHoaDon.GetMaHoaDon(AbpSession.TenantId ?? 1, objUp.IdChiNhanh, objUp.IdLoaiChungTu, objUp.NgayLapHoaDon);
            }

            //objUp.IdKhachHang = dto.IdKhachHang;
            //objUp.IdNhanVien = dto.IdNhanVien;
            //objUp.IdHoaDon = dto.IdHoaDon;
            //objUp.NgayLapHoaDon = dto.NgayLapHoaDon;
            //objUp.MaHoaDon = dto.MaHoaDon;
            //objUp.NgayApDung = dto.NgayApDung;
            //objUp.IdHoaDon = dto.IdHoaDon;
            //objUp.IdHoaDon = dto.IdHoaDon;
            //objUp.IdHoaDon = dto.IdHoaDon;
            //objUp.GhiChuHD = dto.GhiChuHD;
            objUp.LastModifierUserId = AbpSession.UserId;
            objUp.LastModificationTime = DateTime.Now;

            foreach (var item in dataChiTietHD)
            {
                BH_HoaDon_ChiTiet ctUpdate = await _hoaDonChiTietRepository.FirstOrDefaultAsync(item.Id);
                BH_HoaDon_ChiTiet ctUp = ObjectMapper.Map<BH_HoaDon_ChiTiet>(ctUpdate);
                ctUp.CreatorUserId = AbpSession.UserId;
                ctUp.CreationTime = DateTime.Now;
                await _hoaDonChiTietRepository.UpdateAsync(ctUp);
                // toddo NVThucHien
            }
            await _hoaDonRepository.UpdateAsync(objUp);
            return string.Empty;
        }
        [HttpPost]
        public async Task DeleteHoaDon(Guid id)
        {
            var hoaDon = _hoaDonRepository.FirstOrDefault(x => x.Id == id);
            if (hoaDon != null)
            {
                var hoaDonCTs = await _hoaDonChiTietRepository.GetAll().Where(x => x.IsDeleted == false && x.IdHoaDon == hoaDon.Id).ToListAsync();
                if (hoaDonCTs != null || hoaDonCTs.Count > 0)
                {
                    foreach (var item in hoaDonCTs)
                    {
                        item.IsDeleted = true;
                        item.DeleterUserId = AbpSession.UserId;
                        item.DeletionTime = DateTime.Now;
                        await _hoaDonChiTietRepository.UpdateAsync(item);
                    }
                }
                
                var hoaDonAnh = await _hoaDonAnhRepository.GetAll().Where(x => x.IdHoaDon == hoaDon.IdHoaDon && x.IsDeleted == false).ToListAsync();
                if (hoaDonAnh != null || hoaDonAnh.Count > 0)
                {
                    foreach (var item in hoaDonAnh)
                    {
                        item.IsDeleted = true;
                        item.DeleterUserId = AbpSession.UserId;
                        item.DeletionTime = DateTime.Now;
                        await _hoaDonAnhRepository.UpdateAsync(item); 
                    }
                }
                hoaDon.IsDeleted = true;
                hoaDon.DeleterUserId = AbpSession.UserId;
                hoaDon.DeletionTime = DateTime.Now;
                await _hoaDonRepository.UpdateAsync(hoaDon);
            }
        }
        public async Task GetHoaDon(Guid id) { }
        public async Task GetllHoaDon() { }
    }
}
