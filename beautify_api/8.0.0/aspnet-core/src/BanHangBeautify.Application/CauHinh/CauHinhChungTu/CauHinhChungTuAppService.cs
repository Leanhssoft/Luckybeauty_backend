using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.CauHinh.CauHinhChungTu.Dto;
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

namespace BanHangBeautify.CauHinh.CauHinhChungTu
{
    [AbpAuthorize(PermissionNames.Pages_CauHinhChungTu)]
    public class CauHinhChungTuAppService : SPAAppServiceBase
    {
        private readonly IRepository<HT_CauHinh_ChungTu, Guid> _repository;
        private readonly IRepository<DM_LoaiChungTu, int> _loaiCHungTu;
        INhatKyThaoTacAppService _audilogService;
        public CauHinhChungTuAppService(
            IRepository<HT_CauHinh_ChungTu, 
            Guid> repository, 
            IRepository<DM_LoaiChungTu, int> loaiCHungTu,
            INhatKyThaoTacAppService audilogService
        )
        {
            _repository = repository;
            _loaiCHungTu = loaiCHungTu;
            _audilogService = audilogService;
        }
        [AbpAuthorize(PermissionNames.Pages_CauHinhChungTu_Create, PermissionNames.Pages_CauHinhChungTu_Edit)]
        public async Task<CauHinhChungTuDto> CreateOrEdit(CreateOrEditCauHinhChungTuDto input)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist != null)
            {
                return await Create(input);
            }
            return await Update(input, checkExist);
        }
        [NonAction]
        public async Task<CauHinhChungTuDto> Create(CreateOrEditCauHinhChungTuDto input)
        {
            CauHinhChungTuDto result = new CauHinhChungTuDto();
            HT_CauHinh_ChungTu data = new HT_CauHinh_ChungTu();
            data = ObjectMapper.Map<HT_CauHinh_ChungTu>(input);
            data.Id = Guid.NewGuid();
            var chungTu = await _loaiCHungTu.FirstOrDefaultAsync(x => x.Id == input.IdLoaiChungTu);
            data.MaLoaiChungTu = string.IsNullOrEmpty(data.MaLoaiChungTu) ? chungTu.MaLoaiChungTu : data.MaLoaiChungTu;
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _repository.InsertAsync(data);
            result = ObjectMapper.Map<CauHinhChungTuDto>(input);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
            nhatKyThaoTacDto.ChucNang = "Cấu hình chứng từ";
            nhatKyThaoTacDto.NoiDung = "Tạo mới cấu hình";
            nhatKyThaoTacDto.NoiDungChiTiet = "Tạo mới cấu hình " + data.MaLoaiChungTu;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        [NonAction]
        public async Task<CauHinhChungTuDto> Update(CreateOrEditCauHinhChungTuDto input, HT_CauHinh_ChungTu oldData)
        {
            CauHinhChungTuDto result = new CauHinhChungTuDto();
            oldData.IdLoaiChungTu = input.IdLoaiChungTu;
            var chungTu = await _loaiCHungTu.FirstOrDefaultAsync(x => x.Id == input.IdLoaiChungTu);
            input.MaLoaiChungTu = string.IsNullOrEmpty(input.MaLoaiChungTu) ? chungTu.MaLoaiChungTu : input.MaLoaiChungTu;
            oldData.SuDungMaChiNhanh = input.SuDungMaChiNhanh;
            oldData.KiTuNganCach1 = input.KiTuNganCach1;
            oldData.KiTuNganCach2 = input.KiTuNganCach2;
            oldData.KiTuNganCach3 = input.KiTuNganCach3;
            oldData.IdChiNhanh = input.IdChiNhanh;
            oldData.DoDaiSTT = input.DoDaiSTT;
            oldData.NgayThangNam = input.NgayThangNam;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(oldData);
            result = ObjectMapper.Map<CauHinhChungTuDto>(oldData);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Cấu hình chứng từ";
            nhatKyThaoTacDto.NoiDung = "Cập nhật cấu hình chứng từ";
            nhatKyThaoTacDto.NoiDungChiTiet = "Cập nhật cấu hình chứng từ " + oldData.MaLoaiChungTu;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_CauHinhChungTu_Delete)]
        public async Task<CauHinhChungTuDto> Delete(Guid id)
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
                nhatKyThaoTacDto.ChucNang = "Cấu hình chứng từ";
                nhatKyThaoTacDto.NoiDung = "Xóa cấu hình chứng từ";
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa cấu hình chứng từ " + data.MaLoaiChungTu;
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                return ObjectMapper.Map<CauHinhChungTuDto>(data);
            }
            return new CauHinhChungTuDto();
        }
        public async Task<CreateOrEditCauHinhChungTuDto> GetForEdit(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditCauHinhChungTuDto>(data);
            }
            return new CreateOrEditCauHinhChungTuDto();
        }
        public async Task<PagedResultDto<CauHinhChungTuDto>> GetAll(PagedRequestDto input)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            PagedResultDto<CauHinhChungTuDto> result = new PagedResultDto<CauHinhChungTuDto>();
            var lstData = await _repository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            lstData = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<CauHinhChungTuDto>>(lstData);
            return result;
        }
    }
}
