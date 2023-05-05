using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.HangHoa.Repository;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.KhachHang.KhachHang.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.KhachHang
{
    //[AbpAuthorize(PermissionNames.Pages_KhachHang)]
    public class KhachHangAppService : SPAAppServiceBase
    {
        private IRepository<DM_KhachHang, Guid> _repository;
        private readonly IKhachHangRespository _customerRepo;

        public KhachHangAppService(IRepository<DM_KhachHang, Guid> repository,
              IKhachHangRespository customerRepo)
        {
            _repository = repository;
            _customerRepo = customerRepo;
        }
        public async Task<KhachHangDto> CreateKhachHang(CreateOrEditKhachHangDto dto)
        {
            KhachHangDto result = new KhachHangDto();
            var khachHang = ObjectMapper.Map<DM_KhachHang>(dto);
            khachHang.Id = Guid.NewGuid();
            khachHang.CreationTime = DateTime.Now;
            khachHang.CreatorUserId = AbpSession.UserId;
            khachHang.TenantId = AbpSession.TenantId ?? 1;
            khachHang.IsDeleted = false;
            await _repository.InsertAsync(khachHang);
            result = ObjectMapper.Map<KhachHangDto>(khachHang);
            return result;
        }
        public async Task<CreateOrEditKhachHangDto> GetKhachHang(Guid id)
        {
            var KhachHang = await _repository.GetAsync(id);
            var result = ObjectMapper.Map<CreateOrEditKhachHangDto>(KhachHang);
            return result;
        }
        public async Task<KhachHangDto> EditKhachHang(CreateOrEditKhachHangDto dto)
        {
            KhachHangDto result = new KhachHangDto();
            var khachHang = ObjectMapper.Map<DM_KhachHang>(dto);
            khachHang.LastModificationTime = DateTime.Now;
            khachHang.LastModifierUserId = AbpSession.UserId;
            khachHang.TenantId = AbpSession.TenantId ?? 1;
            await _repository.UpdateAsync(khachHang);
            result = ObjectMapper.Map<KhachHangDto>(khachHang);

            return result;
        }
        [HttpPost]
        public async Task<KhachHangDto> Delete(Guid id)
        {
            KhachHangDto result = new KhachHangDto();
            var delete = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (delete != null)
            {
                delete.IsDeleted = true;
                delete.DeletionTime = DateTime.Now;
                delete.DeleterUserId = AbpSession.UserId;
                delete.TrangThai = 0;
                _repository.Update(delete);
                result = ObjectMapper.Map<KhachHangDto>(delete);
            }
            return result;
        }
        public async Task<DM_KhachHang> GetKhachHangDetail(Guid Id)
        {
            var KhachHang = await _repository.GetAsync(Id);
            return KhachHang;
        }

        public async Task<PagedResultDto<DM_KhachHang>> GetAll(PagedKhachHangResultRequestDto input)
        {
            PagedResultDto<DM_KhachHang> ListResultDto = new PagedResultDto<DM_KhachHang>();
            var lstData = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            ListResultDto.TotalCount = lstData.Count;
            if (!string.IsNullOrEmpty(input.keyword))
            {
                lstData = lstData.Where(
                    x => (x.TenKhachHang!=null && x.TenKhachHang.Contains(input.keyword)) ||
                    (x.MaKhachHang!=null&& x.MaKhachHang.Contains(input.keyword)) ||
                    (x.MaSoThue != null && x.MaSoThue.Contains(input.keyword)) || 
                    (x.SoDienThoai != null && x.SoDienThoai.Contains(input.keyword)) ||
                    (x.DiaChi != null && x.DiaChi.Contains(input.keyword)) || 
                    (x.Email != null && x.Email.Contains(input.keyword))
                   ).ToList();
            }
            if (input.SkipCount > 0)
            {
                input.SkipCount = input.SkipCount * 10;
            }

            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            ListResultDto.Items = lstData;
            return ListResultDto;
        }

        public async Task<PagedResultDto<KhachHangView>> Search(PagedKhachHangResultRequestDto input)
        {
            PagedResultDto<KhachHangView> ListResultDto = new PagedResultDto<KhachHangView>();
            var lstData = await _repository.GetAll().Include(x=>x.NguonKhach).Include(x=>x.NhomKhach)
                .Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            ListResultDto.TotalCount = lstData.Count;
            if (!string.IsNullOrEmpty(input.keyword))
            {
                lstData = lstData.Where(
                    x => (x.TenKhachHang!=null && x.TenKhachHang.Contains(input.keyword)) ||
                    (x.MaKhachHang!=null&& x.MaKhachHang.Contains(input.keyword))||
                    (x.MaSoThue!=null&&x.MaSoThue.Contains(input.keyword)) || 
                    (x.SoDienThoai!=null&&x.SoDienThoai.Contains(input.keyword)) ||
                    (x.DiaChi!=null&&x.DiaChi.Contains(input.keyword)) || 
                    (x.Email != null && x.Email.Contains(input.keyword))
                   ).ToList();
            }
            if (input.SkipCount > 0)
            {
                input.SkipCount = (input.SkipCount-1 )<=0?0 : (input.SkipCount-1)* input.MaxResultCount;
            }

            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var items = lstData.Select(x=>new KhachHangView()
            {
                Id = x.Id,
                TenKhachHang = x.TenKhachHang,
                CuocHenGanNhat = x.CreationTime,
                GioiTinh = x.GioiTinhNam==null? "Khác": (x.GioiTinhNam==true?"Nam":"Nữ"),
                SoDienThoai = x.SoDienThoai,
                NhanVienPhuTrach = "",
                TenNguonKhach = x.NguonKhach==null?"":x.NguonKhach.TenNguon,
                TenNhomKhach = x.NhomKhach==null?"":x.NhomKhach.TenNhomKhach,
                TongChiTieu = 0,
                TongTichDiem = x.TongTichDiem
            }).ToList();
            ListResultDto.Items = items;
            return ListResultDto;
        }

        public async Task<List<KhachHangView>> JqAutoCustomer(PagedKhachHangResultRequestDto input)
        {
            try
            {
                return await _customerRepo.JqAutoCustomer(input, AbpSession.TenantId ?? 1);
            }
            catch (Exception)
            {
                return new List<KhachHangView>();
            }
        }
    }
}
