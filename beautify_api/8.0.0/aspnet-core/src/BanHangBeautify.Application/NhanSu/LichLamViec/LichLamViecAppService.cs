using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Consts;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.LichLamViec.Dto;
using BanHangBeautify.NhanSu.LichLamViec.Repository;
using BanHangBeautify.NhatKyHoatDong;
using BanHangBeautify.NhatKyHoatDong.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.LichLamViec
{
    [AbpAuthorize(PermissionNames.Pages_NhanSu_LichLamViec)]
    public class LichLamViecAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_LichLamViec, Guid> _lichLamViecService;
        private readonly IRepository<NS_LichLamViec_Ca, Guid> _lichLamViecCaService;
        private readonly ILichLamViecRespository _lichLamViecRespository;
        private readonly IRepository<DM_NgayNghiLe, Guid> _dmNgayNghiLeService;
        private readonly INhatKyThaoTacAppService _audiLogService;
        private readonly IRepository<NS_NhanVien, Guid> _nhanVienRepository;
        public LichLamViecAppService(
            IRepository<NS_LichLamViec, Guid> lichLamViecService,
            ILichLamViecRespository lichLamViecRespository,
             IRepository<NS_LichLamViec_Ca, Guid> lichLamViecCaService,
             IRepository<DM_NgayNghiLe, Guid> dmNgayNghiLeService,
              IRepository<NS_NhanVien, Guid> nhanVienRepository,
             INhatKyThaoTacAppService audiLogService

         )
        {
            _lichLamViecService = lichLamViecService;
            _lichLamViecRespository = lichLamViecRespository;
            _lichLamViecCaService = lichLamViecCaService;
            _dmNgayNghiLeService = dmNgayNghiLeService;
            _nhanVienRepository = nhanVienRepository;
            _audiLogService = audiLogService;

        }
        [AbpAuthorize(PermissionNames.Pages_NhanSu_LichLamViec_Create, PermissionNames.Pages_NhanSu_Edit)]
        public async Task<LichLamViecDto> CreateOrEdit(CreateOrEditLichLamViecDto input)
        {
            var isExist = await _lichLamViecService.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (isExist == null)
            {
                return await Create(input);
            }
            return await Update(input, isExist);
        }
        [NonAction]
        public async Task<LichLamViecDto> Create(CreateOrEditLichLamViecDto input)
        {
            LichLamViecDto result = new LichLamViecDto();
            NS_LichLamViec data = new NS_LichLamViec();
            data = ObjectMapper.Map<NS_LichLamViec>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.NgayLamViecTrongTuan = string.Join(";", input.NgayLamViec);
            data.TrangThai = 0;
            result = ObjectMapper.Map<LichLamViecDto>(data);
            await _lichLamViecService.InsertAsync(data);
            var tongSoNgay = data.DenNgay.Value.Subtract(data.TuNgay).TotalDays;
            List<DateTime> danhSachNgayNghi = new List<DateTime>();
            var ngayNghi = _dmNgayNghiLeService.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).ToList();
            foreach (var item in ngayNghi)
            {
                for (int i = 1; i <= item.TongSoNgay; i++)
                {
                    var day = item.TuNgay.AddDays(i);
                    danhSachNgayNghi.Add(day);
                }
            }
            for (int i = 0; i < tongSoNgay + 1; i++)
            {
                var day = data.TuNgay.AddDays(i);
                if (danhSachNgayNghi.Contains(day))
                {
                    continue;
                }
                if (input.NgayLamViec.Contains(day.DayOfWeek.ToString()))
                {
                    NS_LichLamViec_Ca rdo = new NS_LichLamViec_Ca();
                    rdo.Id = Guid.NewGuid();
                    rdo.IdCaLamViec = input.IdCaLamViec;
                    rdo.IdLichLamViec = data.Id;
                    rdo.NgayLamViec = DateTime.Parse(day.ToString("yyyy-MM-dd"));
                    rdo.TenantId = AbpSession.TenantId ?? 1;
                    rdo.CreatorUserId = AbpSession.UserId;
                    rdo.IsDeleted = false;
                    await _lichLamViecCaService.InsertAsync(rdo);
                }
            }
            var nhanVien = _nhanVienRepository.FirstOrDefault(x => x.Id == data.IdNhanVien);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
            nhatKyThaoTacDto.ChucNang = "Lịch làm việc";
            nhatKyThaoTacDto.NoiDung = "Thêm mới lịch làm việc cho nhân viên: " + nhanVien.TenNhanVien + "(" + nhanVien.MaNhanVien + ")";
            nhatKyThaoTacDto.NoiDungChiTiet = "Thêm mới lịch làm việc cho nhân viên: " + nhanVien.TenNhanVien + "(" + nhanVien.MaNhanVien + ")";
            await _audiLogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        [NonAction]
        public async Task<LichLamViecDto> Update(CreateOrEditLichLamViecDto input, NS_LichLamViec oldData)
        {
            var lichLamViecCaAll = _lichLamViecCaService.GetAll().Where(x => x.IdLichLamViec == oldData.Id).ToList();
            if (lichLamViecCaAll != null && lichLamViecCaAll.Count > 0)
            {
                foreach (var item in lichLamViecCaAll)
                {
                    await _lichLamViecCaService.HardDeleteAsync(item);
                }
            }
            LichLamViecDto result = new LichLamViecDto();
            oldData.GiaTriLap = input.GiaTriLap;
            oldData.KieuLapLai = input.KieuLapLai;
            oldData.LapLai = input.LapLai;
            oldData.TuNgay = input.TuNgay;
            oldData.DenNgay = input.DenNgay;
            oldData.IdNhanVien = input.IdNhanVien;
            oldData.IdChiNhanh = input.IdChiNhanh;
            oldData.NgayLamViecTrongTuan = string.Join(";", input.NgayLamViec);
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _lichLamViecService.UpdateAsync(oldData);
            var tongSoNgay = oldData.DenNgay.Value.Subtract(oldData.TuNgay).TotalDays;
            List<DateTime> danhSachNgayNghi = new List<DateTime>();
            var ngayNghi = _dmNgayNghiLeService.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).ToList();
            foreach (var item in ngayNghi)
            {
                for (int i = 1; i <= item.TongSoNgay; i++)
                {
                    var day = item.TuNgay.AddDays(i);
                    danhSachNgayNghi.Add(day);
                }
            }
            for (int i = 0; i < tongSoNgay + 1; i++)
            {
                var day = oldData.TuNgay.AddDays(i);
                if (danhSachNgayNghi.Contains(day))
                {
                    continue;
                }
                if (input.NgayLamViec.Contains(day.DayOfWeek.ToString()))
                {
                    NS_LichLamViec_Ca rdo = new NS_LichLamViec_Ca();
                    rdo.Id = Guid.NewGuid();
                    rdo.IdCaLamViec = input.IdCaLamViec;
                    rdo.IdLichLamViec = oldData.Id;
                    rdo.NgayLamViec = DateTime.Parse(day.ToString("yyyy-MM-dd"));
                    rdo.TenantId = AbpSession.TenantId ?? 1;
                    rdo.CreatorUserId = AbpSession.UserId;
                    rdo.IsDeleted = false;
                    await _lichLamViecCaService.InsertAsync(rdo);
                }
            }
            result = ObjectMapper.Map<LichLamViecDto>(input);
            var nhanVien = _nhanVienRepository.FirstOrDefault(x => x.Id == input.IdNhanVien);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Lịch làm việc";
            nhatKyThaoTacDto.NoiDung = "Sửa lịch làm việc cho nhân viên: " + nhanVien.TenNhanVien + "(" + nhanVien.MaNhanVien + ")";
            nhatKyThaoTacDto.NoiDungChiTiet = "Sửa lịch làm việc cho nhân viên: " + nhanVien.TenNhanVien + "(" + nhanVien.MaNhanVien + ")";
            await _audiLogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;

        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanSu_Delete)]
        public async Task<ExecuteResultDto> Delete(Guid id)
        {
            var checkExist = await _lichLamViecService.FirstOrDefaultAsync(x => x.Id == id);
            if (checkExist != null)
            {

                var listLichLamViecCa = _lichLamViecCaService.GetAll().Where(x => x.IdLichLamViec == id).ToList();
                _lichLamViecCaService.RemoveRange(listLichLamViecCa);
                checkExist.IsDeleted = true;
                checkExist.DeleterUserId = AbpSession.UserId;
                checkExist.DeletionTime = DateTime.Now;
                await _lichLamViecService.DeleteAsync(checkExist);
                var nhanVien = _nhanVienRepository.FirstOrDefault(x => x.Id == checkExist.IdNhanVien);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
                nhatKyThaoTacDto.ChucNang = "Lịch làm việc";
                nhatKyThaoTacDto.NoiDung = "Xóa lịch làm việc nhân viên: " + nhanVien.TenNhanVien + "(" + nhanVien.MaNhanVien + ")";
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa lịch làm việc nhân viên: " + nhanVien.TenNhanVien + "(" + nhanVien.MaNhanVien + ")";
                await _audiLogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                return new ExecuteResultDto()
                {
                    Status = "success",
                    Message = "Xóa dữ liệu thành công!"
                };
            }
            return new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi xảy ra vu lòng thử lại sau!"
            };
        }
        public async Task<NS_LichLamViec> GetDetail(Guid id)
        {
            var checkExist = await _lichLamViecService.FirstOrDefaultAsync(x => x.Id == id);
            if (checkExist != null)
            {
                return checkExist;
            }
            return new NS_LichLamViec();
        }
        public async Task<CreateOrEditLichLamViecDto> GetForEdit(Guid id)
        {

            var checkExist = await _lichLamViecService.FirstOrDefaultAsync(x => x.Id == id);
            if (checkExist != null)
            {
                var createOrEdit = ObjectMapper.Map<CreateOrEditLichLamViecDto>(checkExist);
                var ngayLamViec = new List<string>();
                ngayLamViec.AddRange(checkExist.NgayLamViecTrongTuan.Split(';'));
                createOrEdit.NgayLamViec = ngayLamViec;
                var lichLamViecCa = await _lichLamViecCaService.FirstOrDefaultAsync(x => x.IdLichLamViec == checkExist.Id);
                if (lichLamViecCa != null)
                {
                    createOrEdit.IdCaLamViec = lichLamViecCa.IdCaLamViec;
                }
                return createOrEdit;
            }
            return new CreateOrEditLichLamViecDto();
        }
        public async Task<PagedResultDto<LichLamViecDto>> GetAll(PagedRequestDto input)
        {
            input.MaxResultCount = input.MaxResultCount;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            PagedResultDto<LichLamViecDto> result = new PagedResultDto<LichLamViecDto>();
            var data = await _lichLamViecService.GetAll()
                .Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false)
                .OrderByDescending(x => x.CreationTime)
                .ToListAsync();
            result.TotalCount = data.Count;
            data = data.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<LichLamViecDto>>(data);
            return result;
        }

        public async Task<PagedResultDto<LichLamViecNhanVien>> GetAllLichLamViecWeek(PagedRequestLichLamViecDto input)
        {
            DateTime now = DateTime.Now;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            DateTime date = input.DateFrom;
            string firstDayOfWeek = date.AddDays(DayOfWeek.Monday - date.DayOfWeek).ToString("yyyy-MM-dd");
            string lastDayOfWeek = date.AddDays(DayOfWeek.Sunday - date.DayOfWeek + 7).ToString("yyyy-MM-dd");
            input.DateFrom = DateTime.Parse(firstDayOfWeek);
            input.DateTo = DateTime.Parse(lastDayOfWeek);
            return await _lichLamViecRespository.GetAllLichLamViecWeek(input, AbpSession.TenantId ?? 1);
        }
    }
}
