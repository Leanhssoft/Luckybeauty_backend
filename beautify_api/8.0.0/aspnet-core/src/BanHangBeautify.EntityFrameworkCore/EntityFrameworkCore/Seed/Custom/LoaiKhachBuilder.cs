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
            List<DM_LoaiKhach> lstLoaiKhach = new List<DM_LoaiKhach>()
            {
                new DM_LoaiKhach()
                {
                    Id = Guid.NewGuid(),
                    IsDeleted= false,
                    MaLoai = "KH",
                    TenLoai = "Khách hàng",
                    TenantId = 1
                },
                new DM_LoaiKhach()
                {
                    Id = Guid.NewGuid(),
                    IsDeleted= false,
                    MaLoai = "NCC",
                    TenLoai = "Nhà cung cấp",
                    TenantId = 1
                },
                
            };
            _context.DM_LoaiKhaches.AddRange(lstLoaiKhach);
            _context.SaveChanges();
        }
    }
}
