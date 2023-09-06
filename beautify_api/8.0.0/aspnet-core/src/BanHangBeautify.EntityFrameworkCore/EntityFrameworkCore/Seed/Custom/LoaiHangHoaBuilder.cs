using BanHangBeautify.Data.Entities;
using System.Collections.Generic;
using System.Linq;

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
            // check exists
            List<DM_LoaiHangHoa> lstLoaiHangHoa = new();
            var hh = _context.DM_LoaiHangHoa.Where(x => x.Id == 1);
            if (hh == null)
            {
                lstLoaiHangHoa.Add(new DM_LoaiHangHoa()
                {
                    Id = 1,
                    IsDeleted = false,
                    MaLoaiHangHoa = "HH",
                    TenLoaiHangHoa = "Hàng Hóa",
                    TrangThai = 1
                });
            }
            var dv = _context.DM_LoaiHangHoa.Where(x => x.Id == 2);
            if (dv == null)
            {
                lstLoaiHangHoa.Add(new DM_LoaiHangHoa()
                {
                    Id = 2,
                    IsDeleted = false,
                    MaLoaiHangHoa = "DV",
                    TenLoaiHangHoa = "Dịch Vụ",
                    TrangThai = 1
                });
            }
            var cb = _context.DM_LoaiHangHoa.Where(x => x.Id == 3);
            if (cb == null)
            {
                lstLoaiHangHoa.Add(new DM_LoaiHangHoa()
                {
                    Id = 3,
                    IsDeleted = false,
                    MaLoaiHangHoa = "CB",
                    TenLoaiHangHoa = "Combo",
                    TrangThai = 1
                });
            };
            _context.DM_LoaiHangHoa.AddRange(lstLoaiHangHoa);
            _context.SaveChanges();
        }
    }
}