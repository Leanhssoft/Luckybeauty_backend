using Abp.Domain.Repositories;
using BanHangBeautify.Entities;
using BanHangBeautify.Zalo.DangKyThanhVien;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.DangKy_ThanhVien
{
    public class Zalo_KhachHangThanhVienAppService: SPAAppServiceBase
    {
        private readonly IRepository<Zalo_KhachHangThanhVien, Guid> _zaloKhachHangThanhVien;
        public Zalo_KhachHangThanhVienAppService(IRepository<Zalo_KhachHangThanhVien, Guid> zaloKhachHangThanhVien)
        {
            _zaloKhachHangThanhVien = zaloKhachHangThanhVien;
        }


        [HttpPost]
        public Zalo_KhachHangThanhVienDto DangKyThanhVienZOA(Zalo_KhachHangThanhVienDto dto)
        {
            if (dto == null) { return new Zalo_KhachHangThanhVienDto(); };
            Zalo_KhachHangThanhVien objNew = ObjectMapper.Map<Zalo_KhachHangThanhVien>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreationTime = DateTime.Now;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.IsDeleted = false;
            _zaloKhachHangThanhVien.InsertAsync(objNew);
            var result = ObjectMapper.Map<Zalo_KhachHangThanhVienDto>(objNew);
            return result;
        }
    }
}
