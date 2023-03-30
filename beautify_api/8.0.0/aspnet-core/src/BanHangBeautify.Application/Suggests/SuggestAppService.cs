using Abp.Domain.Repositories;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.Suggests.Dto;
using BanHangBeautify.Users.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Suggests
{
    public class SuggestAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_NhanVien, Guid> _nhanVienRepository;//
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
        public SuggestAppService(
            IRepository<NS_NhanVien, Guid> nhanVienRepository,
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
            IRepository<DM_PhongBan, Guid> phongBanRepository
            )
        {
            _nhanVienRepository = nhanVienRepository;
            _khachHangRepository = khachHangRepository;
            _loaiHangHoaRepository= loaiHangHoaRepository;
            _chiNhanhRepository= chiNhanhRepository;
            _chucVuRepository= chucVuRepository;
            _nhomKhachRepository = nhomKhachRepository;
            _loaiKhachRepository= loaiKhachRepository;
            _nguonKhachRepository= nguonKhachRepository;
            _donViQuiDoiRepository = donViQuiDoiRepository;
            _hangHoaRepository = hangHoaRepository;
            _caLamViecRepository = caLamViecRepository;
            _phongBanRepository= phongBanRepository;
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
        public async Task<List<SuggestNhanSu>> SuggestNhanSus()
        {
            List<SuggestNhanSu> result = new List<SuggestNhanSu>();
            var lstNhanSu = await _nhanVienRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (lstNhanSu != null || lstNhanSu.Count > 0)
            {
                foreach (var item in lstNhanSu)
                {
                    SuggestNhanSu rdo = new SuggestNhanSu();
                    rdo.Id = item.Id;
                    rdo.TenNhanVien = item.TenNhanVien;
                    rdo.SoDienThoai = item.SoDienThoai;
                    result.Add(rdo);
                }
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
            var lst = await _loaiKhachRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestLoaiKhach rdo = new SuggestLoaiKhach();
                    rdo.Id = item.Id;
                    rdo.TenLoai = item.TenLoai;
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
        public async Task<List<SuggestHangHoa>> SuggestHangHoas()
        {
            List<SuggestHangHoa> result = new List<SuggestHangHoa>();
            var lst = await _hangHoaRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestHangHoa rdo = new SuggestHangHoa();
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
            var lst = await _loaiHangHoaRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestLoaiHangHoa rdo = new SuggestLoaiHangHoa();
                    rdo.Id = item.Id;
                    rdo.TenLoai = item.TenLoai;
                    result.Add(rdo);
                }
            }
            return result;

        }
        public async Task<List<SuggestDonViQuiDoi>> SuggestDonViQuiDois()
        {
            List<SuggestDonViQuiDoi> result = new List<SuggestDonViQuiDoi>();
            var lst = await _donViQuiDoiRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).ToListAsync();
            if (lst != null || lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    SuggestDonViQuiDoi rdo = new SuggestDonViQuiDoi();
                    rdo.Id = item.Id;
                    rdo.TenDonVi = item.TenDonViTinh;
                    result.Add(rdo);
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
    }
}
