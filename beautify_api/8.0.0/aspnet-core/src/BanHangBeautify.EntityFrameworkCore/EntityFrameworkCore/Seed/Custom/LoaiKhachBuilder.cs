using BanHangBeautify.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BanHangBeautify.EntityFrameworkCore.Seed.LoaiHangHoa
{
    public class LoaiKhachBuilder
    {
        private readonly SPADbContext _context;
        public LoaiKhachBuilder(SPADbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            CreateDMLoaiKhach();
        }
        private void CreateDMLoaiKhach()
        {
            List<DM_LoaiKhach> lstLoaiKhach = new();
            var kh = _context.DM_LoaiKhach.Where(x => x.Id == 1);
            if (kh == null)
            {
                lstLoaiKhach.Add(new DM_LoaiKhach()
                {
                    Id = 1,
                    IsDeleted = false,
                    MaLoaiKhachHang = "KH",
                    TenLoaiKhachHang = "Khách hàng",
                    TrangThai = 1
                });
            }

            var ncc = _context.DM_LoaiKhach.Where(x => x.Id == 2);
            if (kh == null)
            {
                lstLoaiKhach.Add(new DM_LoaiKhach()
                {
                    Id = 2,
                    IsDeleted = false,
                    MaLoaiKhachHang = "NCC",
                    TenLoaiKhachHang = "Nhà cung cấp",
                    TrangThai = 1
                });

            };
            var exists = _context.DM_LoaiKhach.Select(x => x.Id).ToList();
            if (exists.Count > 0)
            {
                foreach (var item in lstLoaiKhach)
                {
                    if (!exists.Contains(item.Id))
                    {
                        _context.DM_LoaiKhach.Add(item);
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
