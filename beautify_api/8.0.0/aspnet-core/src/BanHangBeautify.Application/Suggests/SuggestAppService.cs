using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using BanHangBeautify.Configuration.Common.Consts;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.Suggests.Dto;
using BanHangBeautify.Suggests.Repository;
using BanHangBeautify.Users.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.Suggests
{
    [Authorize]
    public class SuggestAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_NhanVien, Guid> _nhanVienRepository;//
        private readonly IRepository<DichVu_NhanVien, Guid> _dichVuNhanVienRespository;
        private readonly IRepository<DM_KhachHang, Guid> _khachHangRepository;//
        private readonly IRepository<DM_LoaiHangHoa, int> _loaiHangHoaRepository;//
        private readonly IRepository<NS_ChucVu, Guid> _chucVuRepository;//
        private readonly IRepository<DM_NhomKhachHang, Guid> _nhomKhachRepository;//
        private readonly IRepository<DM_LoaiKhach, int> _loaiKhachRepository;//
        private readonly IRepository<DM_NguonKhach, Guid> _nguonKhachRepository;//
        private readonly IRepository<DM_DonViQuiDoi, Guid> _donViQuiDoiRepository;//
        private readonly IRepository<DM_HangHoa, Guid> _hangHoaRepository;//
        private readonly IRepository<DM_ChiNhanh, Guid> _chiNhanhRepository;//
        private readonly IRepository<NS_CaLamViec, Guid> _caLamViecRepository;//
        private readonly IRepository<DM_PhongBan, Guid> _phongBanRepository;
        private readonly IRepository<DM_NhomHangHoa, Guid> _nhomHangHoaRepository;
        private readonly IRepository<DM_LoaiChungTu,int> _loaiChungTuRepository;
        private readonly IRepository<DM_NganHang,Guid> _nganHangRepository;
        private readonly ISuggestRepository _suggestRepository;
        public SuggestAppService(
            IRepository<NS_NhanVien, Guid> nhanVienRepository,
            IRepository<DichVu_NhanVien, Guid> dichVuNhanVienRespository,
            IRepository<DM_KhachHang, Guid> khachHangRepository,
            IRepository<DM_LoaiHangHoa, int> loaiHangHoaRepository,
            IRepository<NS_ChucVu, Guid> chucVuRepository,
            IRepository<DM_NhomKhachHang, Guid> nhomKhachRepository,
            IRepository<DM_LoaiKhach, int> loaiKhachRepository,
            IRepository<DM_NguonKhach, Guid> nguonKhachRepository,
            IRepository<DM_DonViQuiDoi, Guid> donViQuiDoiRepository,
            IRepository<DM_HangHoa, Guid> hangHoaRepository,
            IRepository<DM_ChiNhanh, Guid> chiNhanhRepository,
            IRepository<NS_CaLamViec, Guid> caLamViecRepository,
            IRepository<DM_PhongBan, Guid> phongBanRepository,
            IRepository<DM_NhomHangHoa,Guid> nhomHangHoaRepository,
            IRepository<DM_LoaiChungTu, int> loaiChungTuRepository,
            IRepository<DM_NganHang,Guid> nganHangRespository,
            ISuggestRepository suggestRepository
            )
        {
            _nhanVienRepository = nhanVienRepository;
            _dichVuNhanVienRespository = dichVuNhanVienRespository;
            _khachHangRepository = khachHangRepository;
            _loaiHangHoaRepository = loaiHangHoaRepository;
            _chiNhanhRepository = chiNhanhRepository;
            _chucVuRepository = chucVuRepository;
            _nhomKhachRepository = nhomKhachRepository;
            _loaiKhachRepository = loaiKhachRepository;
            _nguonKhachRepository = nguonKhachRepository;
            _donViQuiDoiRepository = donViQuiDoiRepository;
            _hangHoaRepository = hangHoaRepository;
            _caLamViecRepository = caLamViecRepository;
            _phongBanRepository = phongBanRepository;
            _nhomHangHoaRepository = nhomHangHoaRepository;
            _suggestRepository = suggestRepository;
            _nganHangRepository = nganHangRespository;
        }
        public async Task<List<SuggestChucVu>> SuggestChucVus()
        {
            List<SuggestChucVu> result = new List<SuggestChucVu>();
            var lstChucVu = _chucVuRepository.GetAll();
            foreach (var item in lstChucVu)
            {
                SuggestChucVu rdo = new SuggestChucVu();
                rdo.TenChucVu = item.TenChucVu;
                rdo.IdChucVu = item.Id;
                result.Add(rdo);
            }
            return result;
        }
        public async Task<List<SuggestNhanSu>> SuggestNhanSus(Guid idChiNhanh)
        {
            List<SuggestNhanSu> result = new List<SuggestNhanSu>();
            //var lstNhanSu = await _nhanVienRepository.GetAll().Include(x => x.NS_ChucVu).Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            //if (lstNhanSu != null || lstNhanSu.Count > 0)
            //{
            //    foreach (var item in lstNhanSu)
            //    {
            //        SuggestNhanSu rdo = new SuggestNhanSu();
            //        rdo.Id = item.Id;
            //        rdo.TenNhanVien = item.TenNhanVien;
            //        rdo.SoDienThoai = item.SoDienThoai;
            //        rdo.Avatar = item.Avatar;
            //        var chucVu = await _chucVuRepository.FirstOrDefaultAsync(x => x.Id == item.IdChucVu);
            //        rdo.ChucVu = chucVu != null ? chucVu.TenChucVu : "";
            //        result.Add(rdo);
            //    }
            //}
            result = await _suggestRepository.SuggestNhanSu(AbpSession.TenantId ?? 1, idChiNhanh);
            return result;

        }
        [HttpPost]
        public async Task<List<SuggestEmpolyeeExecuteServiceDto>> SuggestNhanVienThucHienDichVu(Guid idChiNhanh, Guid? idNhanVien)
        {
            List<SuggestEmpolyeeExecuteServiceDto> result = new List<SuggestEmpolyeeExecuteServiceDto>();
            var lstNhanSu = await _suggestRepository.SuggestNhanVienThucHienDichVu(AbpSession.TenantId ?? 1, idChiNhanh, idNhanVien);
            foreach (var item in lstNhanSu)
            {
                var nhanVienDichVu = await _dichVuNhanVienRespository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false && x.IdNhanVien == item.Id).ToListAsync();
                if (nhanVienDichVu == null || nhanVienDichVu.Count == 0)
                {
                    continue;
                }
                result.Add(item);
            }
            return result;
        }
        [HttpPost]
        public async Task<List<SuggestEmpolyeeExecuteServiceDto>> SuggestNhanVienByIdDichVu(Guid idChiNhanh, Guid idDichVu)
        {
            List<SuggestEmpolyeeExecuteServiceDto> result = new List<SuggestEmpolyeeExecuteServiceDto>();
            var lstNhanSu = await _suggestRepository.SuggestNhanVienByIdDichVu(AbpSession.TenantId ?? 1, idChiNhanh, idDichVu);
            foreach (var item in lstNhanSu)
            {
                var nhanVienDichVu = await _dichVuNhanVienRespository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false && x.IdNhanVien == item.Id).ToListAsync();
                if (nhanVienDichVu == null || nhanVienDichVu.Count == 0)
                {
                    continue;
                }
                result.Add(item);
            }
            return result;
        }

        public async Task<List<SuggestKhachHang>> SuggestKhachHangs()
        {
            List<SuggestKhachHang> result = new List<SuggestKhachHang>();
            var lst = await _khachHangRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestKhachHang rdo = new SuggestKhachHang();
                    rdo.Id = item.Id;
                    rdo.TenKhachHang = item.TenKhachHang;
                    rdo.SoDienThoai = item.SoDienThoai;
                    result.Add(rdo);
                }
            }
            return result;

        }

        public async Task<List<SuggestLoaiKhach>> SuggestLoaiKhachHangs()
        {
            List<SuggestLoaiKhach> result = new List<SuggestLoaiKhach>();
            var lst = await _loaiKhachRepository.GetAll().Where(x => x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestLoaiKhach rdo = new SuggestLoaiKhach();
                    rdo.Id = item.Id;
                    rdo.TenLoai = item.TenLoaiKhachHang;
                    result.Add(rdo);
                }
            }
            return result;

        }

        public async Task<List<SuggestNhomKhach>> SuggestNhomKhachHangs()
        {
            List<SuggestNhomKhach> result = new List<SuggestNhomKhach>();
            var lst = await _nhomKhachRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestNhomKhach rdo = new SuggestNhomKhach();
                    rdo.Id = item.Id;
                    rdo.TenNhomKhach = item.TenNhomKhach;
                    result.Add(rdo);
                }
            }
            return result;

        }

        public async Task<List<SuggestNguonKhach>> SuggestNguonKhachHangs()
        {
            List<SuggestNguonKhach> result = new List<SuggestNguonKhach>();
            var lst = await _nguonKhachRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestNguonKhach rdo = new SuggestNguonKhach();
                    rdo.Id = item.Id;
                    rdo.TenNguonKhach = item.TenNguon;
                    result.Add(rdo);
                }
            }
            return result;

        }
        public async Task<List<SuggestHangHoaDto>> SuggestHangHoas()
        {
            List<SuggestHangHoaDto> result = new List<SuggestHangHoaDto>();
            var lst = await _hangHoaRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestHangHoaDto rdo = new SuggestHangHoaDto();
                    rdo.Id = item.Id;
                    rdo.TenHangHoa = item.TenHangHoa;
                    result.Add(rdo);
                }
            }
            return result;

        }
        public async Task<List<SuggestLoaiHangHoa>> SuggestLoaiHangHoas()
        {
            List<SuggestLoaiHangHoa> result = new List<SuggestLoaiHangHoa>();
            var lst = await _loaiHangHoaRepository.GetAll().Where(x => x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestLoaiHangHoa rdo = new SuggestLoaiHangHoa();
                    rdo.Id = item.Id;
                    rdo.TenLoai = item.TenLoaiHangHoa;
                    result.Add(rdo);
                }
            }
            return result;

        }
        public async Task<List<SuggestNhomHangHoa>> SuggestNhomHangHoas()
        {
            List<SuggestNhomHangHoa> result = new List<SuggestNhomHangHoa>();
            var lst = await _nhomHangHoaRepository.GetAll().Where(x => x.IsDeleted == false && x.TenantId==(AbpSession.TenantId??1) && x.LaNhomHangHoa ==false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestNhomHangHoa rdo = new SuggestNhomHangHoa();
                    rdo.IdNhomHang = item.Id;
                    rdo.TenNhomHang = item.TenNhomHang;
                    result.Add(rdo);
                }
            }
            return result;

        }
        public async Task<List<SuggestDonViQuiDoi>> SuggestDonViQuiDois()
        {
            List<SuggestDonViQuiDoi> result = new List<SuggestDonViQuiDoi>();
            var lst = await _donViQuiDoiRepository.GetAll().Include(x => x.DM_HangHoa).Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestDonViQuiDoi rdo = new SuggestDonViQuiDoi();
                    rdo.Id = item.Id;
                    rdo.TenDonVi = item.DM_HangHoa.TenHangHoa;
                    result.Add(rdo);
                }
            }
            return result;

        }

        public async Task<List<SuggestDichVuDto>> SuggestDichVu(Guid? idNhanVien)
        {
            List<SuggestDichVuDto> result = new List<SuggestDichVuDto>();
            if (idNhanVien.HasValue==false)
            {
                var lst = await _donViQuiDoiRepository
                .GetAll()
                .Include(x => x.DM_HangHoa)
                .Where(
                    x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false &&
                    x.DM_HangHoa.IdLoaiHangHoa != LoaiHangHoaConst.HangHoa
                ).ToListAsync();
                if (lst != null || lst.Count > 0)
                {
                    foreach (var item in lst)
                    {
                        SuggestDichVuDto rdo = new SuggestDichVuDto();
                        rdo.Id = item.Id;
                        rdo.TenDichVu = item.DM_HangHoa.TenHangHoa;
                        rdo.DonGia = decimal.Parse(item.GiaBan.ToString() ?? "0");
                        rdo.ThoiGianThucHien = item.DM_HangHoa.SoPhutThucHien.HasValue ? item.DM_HangHoa.SoPhutThucHien.Value.ToString() + " phút" : "0 phút";
                        result.Add(rdo);
                    }
                }
            }
            else
            {
                var dichVus = await _dichVuNhanVienRespository.GetAllListAsync(x => x.IdNhanVien == idNhanVien.Value);
                var idDichVus = dichVus.Select(x => x.IdDonViQuyDoi).ToList();
                var lst = await _donViQuiDoiRepository
                .GetAll()
                .Include(x => x.DM_HangHoa)
                .Where(
                    x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false &&
                    x.DM_HangHoa.IdLoaiHangHoa != LoaiHangHoaConst.HangHoa && idDichVus.Contains(x.Id)
                ).ToListAsync();
                if (lst != null || lst.Count > 0)
                {
                    foreach (var item in lst)
                    {
                        SuggestDichVuDto rdo = new SuggestDichVuDto();
                        rdo.Id = item.Id;
                        rdo.TenDichVu = item.DM_HangHoa.TenHangHoa;
                        rdo.DonGia = decimal.Parse(item.GiaBan.ToString() ?? "0");
                        rdo.ThoiGianThucHien = item.DM_HangHoa.SoPhutThucHien.HasValue ? item.DM_HangHoa.SoPhutThucHien.Value.ToString() + " phút" : "0 phút";
                        result.Add(rdo);
                    }
                }
            }
            
            return result;

        }

        public async Task<List<SuggestChiNhanh>> SuggestChiNhanhs()
        {
            List<SuggestChiNhanh> result = new List<SuggestChiNhanh>();
            var lst = await _chiNhanhRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestChiNhanh rdo = new SuggestChiNhanh();
                    rdo.Id = item.Id;
                    rdo.TenChiNhanh = item.TenChiNhanh;
                    result.Add(rdo);
                }
            }
            return result;

        }
        public async Task<List<SuggestCaLamViec>> SuggestCaLamViecs()
        {
            List<SuggestCaLamViec> result = new List<SuggestCaLamViec>();
            var lst = await _caLamViecRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestCaLamViec rdo = new SuggestCaLamViec();
                    rdo.Id = item.Id;
                    rdo.TenCa = item.TenCa;
                    result.Add(rdo);
                }
            }
            return result;

        }
        public async Task<List<SuggestPhongBan>> SuggestPhongBans()
        {
            List<SuggestPhongBan> result = new List<SuggestPhongBan>();
            var lst = await _phongBanRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestPhongBan rdo = new SuggestPhongBan();
                    rdo.Id = item.Id;
                    rdo.TenPhongBan = item.TenPhongBan;
                    result.Add(rdo);
                }
            }
            return result;

        }
        [HttpPost]
        public async Task<List<SuggestLoaiChungTu>> SuggestLoaiChungTus()
        {
            List<SuggestLoaiChungTu> result = new List<SuggestLoaiChungTu>();
            var listLoaiChungTu = await _suggestRepository.SuggestLoaiChungTu();
            if (listLoaiChungTu != null && listLoaiChungTu.Count() > 0)
            {
                foreach (var item in listLoaiChungTu)
                {
                    SuggestLoaiChungTu rdo = new SuggestLoaiChungTu();
                    rdo.Id = item.Id;
                    rdo.TenLoaiChungTu = item.TenLoaiChungTu;
                    result.Add(rdo);
                }
            }

            return result;
        }

        [HttpPost]
        public async Task<List<SuggestNganHangDto>> SuggestNganHang()
        {
            List<SuggestNganHangDto> result = new List<SuggestNganHangDto>();
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var listNganHang = await _nganHangRepository.GetAll().ToListAsync();
                if (listNganHang != null && listNganHang.Count() > 0)
                {
                    foreach (var item in listNganHang)
                    {
                        SuggestNganHangDto rdo = new SuggestNganHangDto();
                        rdo.Id = item.Id; rdo.MaNganHang = item.MaNganHang;
                        rdo.TenNganHang = item.TenNganHang;
                        rdo.BIN = item.BIN;
                        rdo.TenRutGon = item.TenRutGon;
                        rdo.Logo = item.Logo;
                        result.Add(rdo);
                    }
                }
            }
                

            return result;
        }
    }
}
