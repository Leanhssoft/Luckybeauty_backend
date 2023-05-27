using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.ChietKhau.ChietKhauDichVu.Dto;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauDichVu
{
    [AbpAuthorize(PermissionNames.Pages_ChietKhauDichVu)]
    public class ChietKhauDichVuAppService:SPAAppServiceBase
    {
        private readonly IRepository<NS_ChietKhauDichVu, Guid> _repository;
        private readonly IRepository<DM_HangHoa,Guid> _hangHoaRepository;
        public ChietKhauDichVuAppService(IRepository<NS_ChietKhauDichVu, Guid> repository, IRepository<DM_HangHoa, Guid> hangHoaRepository)
        {
            _repository = repository;
            _hangHoaRepository = hangHoaRepository;
        }
        public async Task<ChietKhauDichVuDto> CreateOrEdit(CreateOrEditChietKhauDichVuDto input)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist == null)
            {
                return await Create(input);
            }
            return await Update(input, checkExist);
        }
        [NonAction]
        public async Task<ChietKhauDichVuDto> Create(CreateOrEditChietKhauDichVuDto input)
        {
            ChietKhauDichVuDto result = new ChietKhauDichVuDto();
            NS_ChietKhauDichVu data = new NS_ChietKhauDichVu();
            data = ObjectMapper.Map<NS_ChietKhauDichVu>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _repository.InsertAsync(data);
            result = ObjectMapper.Map<ChietKhauDichVuDto>(input);
            return result;
        }
        [NonAction]
        public async Task<ChietKhauDichVuDto> Update(CreateOrEditChietKhauDichVuDto input, NS_ChietKhauDichVu oldData) { 
            ChietKhauDichVuDto result = new ChietKhauDichVuDto();
            oldData.IdDonViQuiDoi = input.IdDonViQuyDoi;
            oldData.IdNhanVien = input.IdNhanVien;
            oldData.GiaTri = input.GiaTri;
            oldData.LaPhanTram = input.LaPhanTram;
            oldData.LoaiChietKhau = input.LoaiChietKhau;
            oldData.TrangThai = 1;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(oldData);
            result = ObjectMapper.Map<ChietKhauDichVuDto>(oldData);
            return result;
        }
        [HttpPost]
        public async Task<ChietKhauDichVuDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeletionTime = DateTime.Now;
                data.DeleterUserId = AbpSession.UserId;
                await _repository.UpdateAsync(data);
                return ObjectMapper.Map<ChietKhauDichVuDto>(data);
            }
            return new ChietKhauDichVuDto();
        }
        public async Task<CreateOrEditChietKhauDichVuDto> GetForEdit(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditChietKhauDichVuDto>(data);
            }
            return new CreateOrEditChietKhauDichVuDto();
        }
        public async Task<PagedResultDto<ChietKhauDichVuDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 0 ? input.SkipCount * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<ChietKhauDichVuDto> result = new PagedResultDto<ChietKhauDichVuDto>();
            var lstData = await _repository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<ChietKhauDichVuDto>>(lstData);
            return result;
        }
        public async Task<PagedResultDto<ChietKhauDichVuItemDto>> GetAccordingByNhanVien(PagedRequestDto input,Guid idNhanVien)
        {
            input.SkipCount = input.SkipCount > 0 ? input.SkipCount * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<ChietKhauDichVuItemDto> result = new PagedResultDto<ChietKhauDichVuItemDto>();
            var lstData = await _repository.GetAll().Include(x=>x.DM_DonViQuiDoi).
                Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)&& x.IdNhanVien==idNhanVien).
                OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            List<ChietKhauDichVuItemDto> items = new List<ChietKhauDichVuItemDto>();
            foreach (var item in lstData)
            {
                ChietKhauDichVuItemDto rdo = new ChietKhauDichVuItemDto();
                var hangHoa = await _hangHoaRepository.GetAll().Include(x => x.DM_NhomHangHoa).Where(x => x.Id == item.DM_DonViQuiDoi.IdHangHoa).FirstOrDefaultAsync();
                rdo.Id = item.Id;
                rdo.TenDichVu = hangHoa != null ? hangHoa.TenHangHoa : "";
                rdo.TenNhomDichVu = hangHoa != null && hangHoa.DM_NhomHangHoa!=null ? hangHoa.DM_NhomHangHoa.TenNhomHang : "";
                switch (item.LoaiChietKhau)
                {
                    case 1:
                        rdo.HoaHongYeuCauThucHien = 0;
                        rdo.HoaHongThucHien = item.GiaTri;
                        rdo.HoaHongTuVan = 0;
                        break;
                    case 2:
                        rdo.HoaHongYeuCauThucHien = item.GiaTri;
                        rdo.HoaHongThucHien = 0;
                        rdo.HoaHongTuVan = 0;
                        break;
                    case 3:
                        rdo.HoaHongYeuCauThucHien = 0;
                        rdo.HoaHongThucHien = 0;
                        rdo.HoaHongTuVan = item.GiaTri;
                        break;
                    default:
                        rdo.HoaHongYeuCauThucHien = 0;
                        rdo.HoaHongThucHien = 0;
                        rdo.HoaHongTuVan = 0;
                        break;
                }
                rdo.GiaDichVu = item.DM_DonViQuiDoi.GiaBan??0;
                items.Add(rdo);
            }
            result.Items = items;
            return result;
        }
    }
}
