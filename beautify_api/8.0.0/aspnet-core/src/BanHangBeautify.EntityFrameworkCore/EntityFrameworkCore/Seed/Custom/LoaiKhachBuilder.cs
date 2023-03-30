using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    Id= 1,
                    IsDeleted= false,
                    MaLoai = "KH",
                    TenLoai = "Khách hàng",
                    TrangThai = 1,
                    TenantId = 1
                });
            }

            var ncc = _context.DM_LoaiKhach.Where(x => x.Id == 2);
            if (kh == null)
            {
                lstLoaiKhach.Add(new DM_LoaiKhach()
                {
                    Id = 2,
                    IsDeleted = false,
                    MaLoai = "NCC",
                    TenLoai = "Nhà cung cấp",
                    TrangThai = 1,
                    TenantId = 1
                },
                
            };
            var exists = _context.DM_LoaiKhach.Select(x => x.Id).ToList();
            if(exists.Count > 0 )
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
