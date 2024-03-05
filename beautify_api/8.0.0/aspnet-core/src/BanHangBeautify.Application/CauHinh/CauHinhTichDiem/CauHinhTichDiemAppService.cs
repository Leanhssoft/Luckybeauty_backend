using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.CauHinh.CauHinhTichDiem.Dto;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.NhatKyHoatDong;
using BanHangBeautify.NhatKyHoatDong.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.CauHinh.CauHinhTichDiem
{
    [AbpAuthorize(PermissionNames.Pages_CauHinhTichDiem)]
    public class CauHinhTichDiemAppService : SPAAppServiceBase
    {
        private readonly IRepository<HT_CauHinh_TichDiem, Guid> _repository;
        INhatKyThaoTacAppService _audilogService;
        public CauHinhTichDiemAppService(IRepository<HT_CauHinh_TichDiem, Guid> repository,INhatKyThaoTacAppService audilogService)
        {
            _repository = repository;
            _audilogService = audilogService;
        }
        [AbpAuthorize(PermissionNames.Pages_CauHinhTichDiem_Create, PermissionNames.Pages_CauHinhTichDiem_Edit)]

        public async Task<CauHinhTichDiemDto> CreateOrEdit(CreateOrEditCauHinhTichDiemDto input)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist != null)
            {
                return await Create(input);
            }
            return await Update(input, checkExist);
        }
        [NonAction]
        public async Task<CauHinhTichDiemDto> Create(CreateOrEditCauHinhTichDiemDto input)
        {
            CauHinhTichDiemDto result = new CauHinhTichDiemDto();
            HT_CauHinh_TichDiem data = new HT_CauHinh_TichDiem();
            data = ObjectMapper.Map<HT_CauHinh_TichDiem>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _repository.InsertAsync(data);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
            nhatKyThaoTacDto.ChucNang = "Cấu hình tích điêm";
            nhatKyThaoTacDto.NoiDung = "Tạo mới cấu hình";
            nhatKyThaoTacDto.NoiDungChiTiet = "Tạo mới cấu hình";
            result = ObjectMapper.Map<CauHinhTichDiemDto>(input);
            return result;
        }
        [NonAction]
        public async Task<CauHinhTichDiemDto> Update(CreateOrEditCauHinhTichDiemDto input, HT_CauHinh_TichDiem oldData)
        {
            CauHinhTichDiemDto result = new CauHinhTichDiemDto();
            oldData.ChoPhepThanhToanBangDiem = input.ChoPhepThanhToanBangDiem;
            oldData.TienThanhToan = input.TienThanhToan;
            oldData.DiemThanhToan = input.DiemThanhToan;
            oldData.KhongTichDiemHDGiamGia = input.KhongTichDiemHDGiamGia;
            oldData.KhongTichDiemSPGiamGia = input.KhongTichDiemSPGiamGia;
            oldData.SoLanMua = input.SoLanMua;
            oldData.TatCaKhachHang = input.TatCaKhachHang;
            oldData.IdCauHinh = input.IdCauHinh;
            oldData.TichDiemHoaDonGiamGia = input.TichDiemHoaDonGiamGia;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(oldData);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Cấu hình tích điểm";
            nhatKyThaoTacDto.NoiDung = "Cập nhật cấu hình tích điểm";
            nhatKyThaoTacDto.NoiDungChiTiet = "Cập nhật cấu hình tích điểm";
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            result = ObjectMapper.Map<CauHinhTichDiemDto>(oldData);
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_CauHinhTichDiem_Delete)]
        public async Task<CauHinhTichDiemDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeletionTime = DateTime.Now;
                data.DeleterUserId = AbpSession.UserId;
                await _repository.UpdateAsync(data);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
                nhatKyThaoTacDto.ChucNang = "Cấu hình tích điểm";
                nhatKyThaoTacDto.NoiDung = "Xóa cấu hình tích điểm";
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa cấu hình tích điểm";
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                return ObjectMapper.Map<CauHinhTichDiemDto>(data);
            }
            return new CauHinhTichDiemDto();
        }
        public async Task<CreateOrEditCauHinhTichDiemDto> GetForEdit(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditCauHinhTichDiemDto>(data);
            }
            return new CreateOrEditCauHinhTichDiemDto();
        }
        public async Task<PagedResultDto<CauHinhTichDiemDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<CauHinhTichDiemDto> result = new PagedResultDto<CauHinhTichDiemDto>();
            var lstData = await _repository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<CauHinhTichDiemDto>>(lstData);
            return result;
        }
    }
}
