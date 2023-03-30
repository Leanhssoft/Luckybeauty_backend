using BanHangBeautify.Data.Entities;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.EntityFrameworkCore.Seed.LoaiHangHoa
{
    public class LoaiHangHoaBuilder
    {
        private readonly SPADbContext _context;
        public LoaiHangHoaBuilder(SPADbContext context)
        {
            _context = context; 
        }
        public void Create()
        {
            CreateDMLoaiHangHoa();
        }
        private void CreateDMLoaiHangHoa()
        {
            List<DM_LoaiHangHoa> lstLoaiHangHoa = new List<DM_LoaiHangHoa>()
            {
                new DM_LoaiHangHoa()
                {
                    Id = 1,
                    IsDeleted= false,
                    MaLoai = "HH",
                    TenLoai = "Hàng Hóa",
                    TenantId = 1
                },
                new DM_LoaiHangHoa()
                {
                    Id = 2,
                    IsDeleted= false,
                    MaLoai = "DV",
                    TenLoai = "Dịch Vụ",
                    TenantId = 1
                },
                new DM_LoaiHangHoa()
                {
                    Id = 3,
                    IsDeleted= false,
                    MaLoai = "CB",
                    TenLoai = "Combo",
                    TenantId = 1
                },
            };
            var exists = _context.DM_LoaiHangHoa.Select(x => x.Id).ToList();
            if (exists.Count > 0)
            {
                foreach (var item in lstLoaiHangHoa)
                {
                    if (!exists.Contains(item.Id))
                    {
                        _context.DM_LoaiHangHoa.Add(item);
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
