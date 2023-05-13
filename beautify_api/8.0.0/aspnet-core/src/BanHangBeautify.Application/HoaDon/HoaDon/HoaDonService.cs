using Abp.Domain.Repositories;
using BanHangBeautify.Common.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.HoaDon.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDon
{
    public class HoaDonService : SPAAppServiceBase
    {
        private readonly IRepository<BH_HoaDon,Guid> _hoaDonRepository;
        private readonly IRepository<BH_HoaDon_ChiTiet,Guid> _hoaDonChiTietRepository;
        private readonly IRepository<BH_HoaDon_Anh, Guid> _hoaDonAnhRepository;
        private readonly IRepository<DM_LoaiChungTu,int> _loaiChungTuRepository;
        public HoaDonService(
            IRepository<BH_HoaDon, Guid> hoaDonRepository, 
            IRepository<DM_LoaiChungTu, int> loaiChungTuRepository, 
            IRepository<BH_HoaDon_ChiTiet, Guid> hoaDonChiTietRepository, 
            IRepository<BH_HoaDon_Anh, Guid> hoaDonAnhRepository
        )
        {
            _hoaDonRepository = hoaDonRepository;
            _loaiChungTuRepository = loaiChungTuRepository;
            _hoaDonChiTietRepository = hoaDonChiTietRepository;
            _hoaDonAnhRepository = hoaDonAnhRepository;
        }
        public async Task CreateHoaDon(CreateHoaDonDto input)
        {
            BH_HoaDon result= new BH_HoaDon();
            result = ObjectMapper.Map<BH_HoaDon>(input);
            if (string.IsNullOrEmpty(result.MaHoaDon))
            {
                var maLoaiChungTu = await _loaiChungTuRepository.FirstOrDefaultAsync(x => x.Id == result.IdLoaiChungTu);
                var countMaxHDChungTu = await _hoaDonRepository.GetAll().Where(x => x.MaHoaDon.Contains(maLoaiChungTu.MaLoaiChungTu)).ToListAsync();
                result.MaHoaDon = maLoaiChungTu.MaLoaiChungTu + "00" + (countMaxHDChungTu.Count+1).ToString();
            }
            if (result.IdLoaiChungTu==LoaiChungTuConst.GDV || result.IdLoaiChungTu!=LoaiChungTuConst.TGT)
            {
                result.NgayApDung = null;
                result.NgayApDung = null;
            }
            result.TrangThai = 1;
            await _hoaDonRepository.InsertAsync(result);
        }
        public async Task UpdateHoaDon()
        {

        }
        public async Task DeleteHoaDon(Guid id)
        {
            var hoaDon = _hoaDonRepository.FirstOrDefault(x => x.Id == id);
            if (hoaDon != null)
            {
                var hoaDonCTs = await _hoaDonChiTietRepository.GetAll().Where(x=>x.IsDeleted== false&& x.IdHoaDon== hoaDon.Id).ToListAsync();
                if (hoaDonCTs!=null || hoaDonCTs.Count>0)
                {
                    foreach (var item in hoaDonCTs)
                    {
                        item.IsDeleted = true;
                        item.DeleterUserId = AbpSession.UserId;
                        item.DeletionTime = DateTime.Now;
                        await _hoaDonChiTietRepository.UpdateAsync(item);
                    }
                }
                
                var hoaDonAnh = await _hoaDonAnhRepository.GetAll().Where(x=>x.IdHoaDon==hoaDon.IdHoaDon&& x.IsDeleted==false).ToListAsync();
                if (hoaDonAnh!=null || hoaDonAnh.Count>0)
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
