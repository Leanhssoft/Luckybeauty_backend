using Abp.Domain.Repositories;
using Abp.Webhooks;
using BanHangBeautify.AppWebhook;
using BanHangBeautify.Entities;
using BanHangBeautify.Zalo.DangKyThanhVien;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BanHangBeautify.Zalo.DangKy_ThanhVien
{
    public class Zalo_KhachHangThanhVienAppService : SPAAppServiceBase, IZalo_KhachHangThanhVienAppService
    {
        private readonly IRepository<Zalo_KhachHangThanhVien, Guid> _zaloKhachHangThanhVien;
        private readonly IConfiguration _config;

        public Zalo_KhachHangThanhVienAppService(IRepository<Zalo_KhachHangThanhVien, Guid> zaloKhachHangThanhVien, IConfiguration config)
        {
            _zaloKhachHangThanhVien = zaloKhachHangThanhVien;
            _config = config;
        }
        [HttpPost]
        public async Task<Zalo_KhachHangThanhVienDto> DangKyThanhVienZOA(Zalo_KhachHangThanhVienDto dto)
        {
            if (dto == null) { return new Zalo_KhachHangThanhVienDto(); };
            Zalo_KhachHangThanhVien objNew = ObjectMapper.Map<Zalo_KhachHangThanhVien>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreationTime = DateTime.Now;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.IsDeleted = false;
            await _zaloKhachHangThanhVien.InsertAsync(objNew);
            var result = ObjectMapper.Map<Zalo_KhachHangThanhVienDto>(objNew);
            return result;
        }

        [HttpPost]
        public async Task<Zalo_KhachHangThanhVienDto> UpdateThanhVienZOA(Zalo_KhachHangThanhVienDto dto)
        {
            if (dto == null) { return new Zalo_KhachHangThanhVienDto(); };
            var objUpdate = _zaloKhachHangThanhVien.GetAllList().Where(x => x.ZOAUserId == dto.ZOAUserId).FirstOrDefault();
            if (objUpdate == null)
            {
                return new Zalo_KhachHangThanhVienDto();
            }
            Zalo_KhachHangThanhVien objNew = ObjectMapper.Map<Zalo_KhachHangThanhVien>(dto);
            objUpdate.TenDangKy = dto.TenDangKy;
            objUpdate.SoDienThoaiDK = dto.SoDienThoaiDK;
            objUpdate.DiaChi = dto.DiaChi;
            objUpdate.TenTinhThanh = dto.TenTinhThanh;
            objUpdate.TenQuanHuyen = dto.TenQuanHuyen;
            objUpdate.LastModificationTime = DateTime.Now;
            objUpdate.LastModifierUserId = AbpSession.UserId;
            objUpdate.IsDeleted = false;
            await _zaloKhachHangThanhVien.UpdateAsync(objUpdate);
            var result = ObjectMapper.Map<Zalo_KhachHangThanhVienDto>(objUpdate);
            return result;
        }

        public bool CheckExistZaloUserId(string zaloUserId)
        {
            try
            {
                var objFind = _zaloKhachHangThanhVien.GetAllList().Where(x => x.ZOAUserId == zaloUserId);
                if (objFind != null && objFind.Count() > 0) return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
