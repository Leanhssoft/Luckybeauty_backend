using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Consts;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.DonViQuiDoi.Dto;
using BanHangBeautify.NhatKyHoatDong;
using BanHangBeautify.NhatKyHoatDong.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.DonViQuiDoi
{
    [AbpAuthorize(PermissionNames.Pages_DonViQuiDoi)]
    public class DonViQuiDoiAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_DonViQuiDoi, Guid> _repository;
        private readonly IRepository<DM_HangHoa, Guid> _hangHoaRepository;
        INhatKyThaoTacAppService _audilogService;
        public DonViQuiDoiAppService(
            IRepository<DM_DonViQuiDoi,
            Guid> repository,
            IRepository<DM_HangHoa, Guid> hangHoaRepository,
            INhatKyThaoTacAppService audilogService
        )
        {
            _repository = repository;
            _hangHoaRepository = hangHoaRepository;
            _audilogService = audilogService;
        }
        [AbpAuthorize(PermissionNames.Pages_DonViQuiDoi_Create, PermissionNames.Pages_DonViQuiDoi_Edit)]
        public async Task<DonViQuiDoiDto> CreateOrEdit(CreateOrEditDonViQuiDoiDto dto)
        {
            var findHangHoa = await _repository.FirstOrDefaultAsync(h => h.Id == dto.Id);
            if (findHangHoa == null)
            {
                return await Create(dto);
            }
            else
            {
                return await Edit(dto, findHangHoa);
            }
        }
        [NonAction]
        public async Task<DonViQuiDoiDto> Create(CreateOrEditDonViQuiDoiDto dto)
        {
            DM_DonViQuiDoi donViQuiDoi = new DM_DonViQuiDoi();
            donViQuiDoi.Id = Guid.NewGuid();
            donViQuiDoi.IdHangHoa = dto.IdHangHoa;
            var hangHoa = _hangHoaRepository.FirstOrDefault(h => h.Id == dto.IdHangHoa);
            donViQuiDoi.MaHangHoa = hangHoa == null ? dto.MaHangHoa : hangHoa.TenHangHoa;
            donViQuiDoi.GiaBan = dto.GiaBan;
            donViQuiDoi.TenDonViTinh = dto.TenDonViTinh;
            donViQuiDoi.LaDonViTinhChuan = dto.LaDonViTinhChuan;
            donViQuiDoi.TyLeChuyenDoi = dto.TyLeChuyenDoi;
            donViQuiDoi.TenantId = AbpSession.TenantId ?? 1;
            donViQuiDoi.CreatorUserId = AbpSession.UserId;
            donViQuiDoi.CreationTime = DateTime.Now;
            var result = ObjectMapper.Map<DonViQuiDoiDto>(donViQuiDoi);
            await _repository.InsertAsync(donViQuiDoi);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
            nhatKyThaoTacDto.ChucNang = "Đơn vị quy đổi";
            nhatKyThaoTacDto.NoiDung = "Thêm mới đơn vị quy đổi: " + donViQuiDoi.MaHangHoa;
            nhatKyThaoTacDto.NoiDungChiTiet = string.Format("<div>" +
                "<p>Giá: {0}</p>" +
                "<p>Đơn vị tính: {1}</p>" +
                "<p>Tỉ lệ chuyển đổi: {2}</p>" +
                "</div>", donViQuiDoi.GiaBan, donViQuiDoi.TenDonViTinh, donViQuiDoi.TyLeChuyenDoi);
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        [NonAction]
        public async Task<DonViQuiDoiDto> Edit(CreateOrEditDonViQuiDoiDto dto, DM_DonViQuiDoi donViQuiDoi)
        {
            donViQuiDoi.IdHangHoa = dto.IdHangHoa;
            donViQuiDoi.MaHangHoa = dto.MaHangHoa;
            donViQuiDoi.GiaBan = dto.GiaBan;
            donViQuiDoi.TenDonViTinh = dto.TenDonViTinh;
            donViQuiDoi.LaDonViTinhChuan = dto.LaDonViTinhChuan;
            donViQuiDoi.TyLeChuyenDoi = dto.TyLeChuyenDoi;
            donViQuiDoi.LastModificationTime = DateTime.Now;
            donViQuiDoi.LastModifierUserId = AbpSession.UserId;
            var result = ObjectMapper.Map<DonViQuiDoiDto>(donViQuiDoi);
            await _repository.UpdateAsync(donViQuiDoi);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Đơn vị quy đổi";
            nhatKyThaoTacDto.NoiDung = "Cập nhật đơn vị quy đổi: " + donViQuiDoi.MaHangHoa;
            nhatKyThaoTacDto.NoiDungChiTiet = string.Format("<div>" +
                "<p>Giá: {0}</p>" +
                "<p>Đơn vị tính: {1}</p>" +
                "<p>Tỉ lệ chuyển đổi: {2}</p>" +
                "</div>", donViQuiDoi.GiaBan, donViQuiDoi.TenDonViTinh, donViQuiDoi.TyLeChuyenDoi);
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        public async Task<DM_DonViQuiDoi> GetDetail(Guid id)
        {
            return await _repository.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<PagedResultDto<DM_DonViQuiDoi>> GetAll(DonViQuiDoiPagedRequestResultDto input)
        {
            var lstDonViQuiDoi = await _repository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                lstDonViQuiDoi = lstDonViQuiDoi.
                    Where(
                        x => x.TenDonViTinh.Contains(input.Keyword) || x.MaHangHoa.Contains(input.Keyword) ||
                        x.GiaBan.ToString().Contains(input.Keyword)
                    ).OrderByDescending(x => x.CreationTime).ToList();
            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            PagedResultDto<DM_DonViQuiDoi> result = new PagedResultDto<DM_DonViQuiDoi>();
            result.TotalCount = lstDonViQuiDoi.Count;
            var getDonViQuiDoi = lstDonViQuiDoi.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = getDonViQuiDoi;
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_DonViQuiDoi_Delete)]
        public async Task<DonViQuiDoiDto> Delete(Guid id)
        {
            DonViQuiDoiDto result = new DonViQuiDoiDto();
            var donViQuiDoi = await _repository.FirstOrDefaultAsync(h => h.Id == id);
            if (donViQuiDoi != null)
            {
                donViQuiDoi.IsDeleted = true;
                donViQuiDoi.DeletionTime = DateTime.Now;
                donViQuiDoi.DeleterUserId = AbpSession.UserId;
                _repository.Update(donViQuiDoi);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
                nhatKyThaoTacDto.ChucNang = "Đơn vị quy đổi";
                nhatKyThaoTacDto.NoiDung = "Xóa đơn vị quy đổi: " + donViQuiDoi.MaHangHoa;
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa đơn vị quy đổi: " + donViQuiDoi.MaHangHoa;
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                result = ObjectMapper.Map<DonViQuiDoiDto>(donViQuiDoi);
            }
            return result;
        }
    }
}
