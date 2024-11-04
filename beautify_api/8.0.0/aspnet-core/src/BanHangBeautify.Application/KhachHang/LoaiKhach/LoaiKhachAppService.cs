using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.LoaiKhach.Dto;
using BanHangBeautify.NhatKyHoatDong;
using BanHangBeautify.NhatKyHoatDong.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.LoaiKhach
{
    [AbpAuthorize(PermissionNames.Pages_LoaiKhach)]
    public class LoaiKhachAppService : SPAAppServiceBase, ILoaiKhachAppService
    {
        private readonly IRepository<DM_LoaiKhach, int> _repository;
        INhatKyThaoTacAppService _audilogService;
        public LoaiKhachAppService(IRepository<DM_LoaiKhach, int> repository, INhatKyThaoTacAppService audilogService)
        {
            _repository = repository;
            _audilogService = audilogService;
        }
        [AbpAuthorize(PermissionNames.Pages_LoaiKhach_Create)]
        public async Task<LoaiKhachDto> CreateLoaiKhach(CreateOrEditLoaiKhachDto dto)
        {
            LoaiKhachDto result = new LoaiKhachDto();
            var loaiKhach = ObjectMapper.Map<DM_LoaiKhach>(dto);
            loaiKhach.Id = _repository.Count() + 1;
            loaiKhach.CreationTime = DateTime.Now;
            loaiKhach.CreatorUserId = AbpSession.UserId;
            loaiKhach.TenantId = AbpSession.TenantId;
            loaiKhach.IsDeleted = false;
            await _repository.InsertAsync(loaiKhach);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
            nhatKyThaoTacDto.ChucNang = "Loại khách";
            nhatKyThaoTacDto.NoiDung = "Thêm mới loại khách hàng";
            nhatKyThaoTacDto.NoiDungChiTiet = "Thêm mới loại khách hàng: " + loaiKhach.TenLoaiKhachHang;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            result = ObjectMapper.Map<LoaiKhachDto>(loaiKhach);
            return result;
        }
        [AbpAuthorize(PermissionNames.Pages_LoaiKhach_Edit)]
        public async Task<LoaiKhachDto> EditLoaiKhach(CreateOrEditLoaiKhachDto dto)
        {
            LoaiKhachDto result = new LoaiKhachDto();
            var loaiKhach = ObjectMapper.Map<DM_LoaiKhach>(dto);
            loaiKhach.LastModificationTime = DateTime.Now;
            loaiKhach.LastModifierUserId = AbpSession.UserId;
            await _repository.UpdateAsync(loaiKhach);
            result = ObjectMapper.Map<LoaiKhachDto>(loaiKhach);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Loại khách";
            nhatKyThaoTacDto.NoiDung = "Cập nhật loại khách hàng";
            nhatKyThaoTacDto.NoiDungChiTiet = "Cập nhật loại khách hàng: " + loaiKhach.TenLoaiKhachHang;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_LoaiKhach_Delete)]
        public async Task<LoaiKhachDto> Delete(int id)
        {
            LoaiKhachDto result = new LoaiKhachDto();
            var delete = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (delete != null)
            {
                delete.IsDeleted = true;
                delete.DeletionTime = DateTime.Now;
                delete.DeleterUserId = AbpSession.UserId;
                delete.TrangThai = 1;
                _repository.Update(delete);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
                nhatKyThaoTacDto.ChucNang = "Loại khách";
                nhatKyThaoTacDto.NoiDung = "Xóa loại khách hàng";
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa loại khách hàng: " + delete.TenLoaiKhachHang;
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                result = ObjectMapper.Map<LoaiKhachDto>(delete);
            }
            return result;
        }
        public async Task<DM_LoaiKhach> GetLoaiKhachDetail(int Id)
        {
            var loaiKhach = await _repository.GetAsync(Id);
            return loaiKhach;
        }

        public async Task<PagedResultDto<DM_LoaiKhach>> GetAll(PagedLoaiKhachResultRequestDto input)
        {
            PagedResultDto<DM_LoaiKhach> ListResultDto = new PagedResultDto<DM_LoaiKhach>();
            var lstData = await _repository.GetAll().Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationTime).ToListAsync();
            ListResultDto.TotalCount = lstData.Count;
            if (!string.IsNullOrEmpty(input.Keyword))
            {
                lstData = lstData.Where(x => x.TenLoaiKhachHang.Contains(input.Keyword) || x.MaLoaiKhachHang.Contains(input.Keyword)).ToList();
            }
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            ListResultDto.Items = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return ListResultDto;
        }
        public async Task<string> GetMaDoiTuong_fromMaxNumber(double maxNumberCode, byte idLoaiDoiTuong)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == idLoaiDoiTuong);
            if (data != null)
            {
                string maDoiTuong = data.MaLoaiKhachHang;
                if (maxNumberCode < 10)
                {
                    return string.Concat(maDoiTuong, "00", maxNumberCode);
                }
                else
                {
                    if (maxNumberCode < 100)
                    {
                        return string.Concat(maDoiTuong, "0", maxNumberCode);
                    }
                }
                return string.Concat(maDoiTuong, maxNumberCode);
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
