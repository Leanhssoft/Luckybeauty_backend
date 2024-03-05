using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Consts;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.NhomHangHoa.Dto;
using BanHangBeautify.NhatKyHoatDong;
using BanHangBeautify.NhatKyHoatDong.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace BanHangBeautify.HangHoa.NhomHangHoa
{
    [AbpAuthorize]
    public class NhomHangHoaAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_NhomHangHoa, Guid> _dmNhomHangHoa;
        private readonly IRepository<DM_HangHoa, Guid> _dmHangHoa;
        INhatKyThaoTacAppService _audilogService;
        public NhomHangHoaAppService(IRepository<DM_NhomHangHoa, Guid> dmNhomHangHoa, 
            IRepository<DM_HangHoa, Guid> dmHangHoa,
            INhatKyThaoTacAppService audilogService)
        {
            _dmNhomHangHoa = dmNhomHangHoa;
            _dmHangHoa = dmHangHoa;
            _audilogService = audilogService;
        }
        public async Task<NhomHangHoaDto> GetNhomHangHoa_byID(Guid id)
        {
            var data = await _dmNhomHangHoa.GetAsync(id);
            var result = ObjectMapper.Map<NhomHangHoaDto>(data);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<NhomHangHoaDto>> GetNhomDichVu()
        {
            PagedResultDto<NhomHangHoaDto> result = new();
            var lst = _dmNhomHangHoa.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false).OrderByDescending(x => x.TenNhomHang);
            result.Items = ObjectMapper.Map<List<NhomHangHoaDto>>(lst);
            return result;
        }

        public async Task<PagedResultDto<NhomHangHoaDto>> GetTreeNhomHangHoa()
        {
            PagedResultDto<NhomHangHoaDto> result = new();
            var data = _dmNhomHangHoa.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && !x.IsDeleted)
                        .Select(x =>
                        new NhomHangHoaDto
                        {
                            Id = x.Id,
                            IdParent = x.IdParent,
                            MaNhomHang = x.MaNhomHang,
                            TenNhomHang = x.TenNhomHang,
                            LaNhomHangHoa = x.LaNhomHangHoa,
                            MoTa = x.MoTa,
                            Color = x.Color,
                            ThuTuHienThi = x.ThuTuHienThi ?? 10,
                            IsDeleted = x.IsDeleted
                        }).ToList();
            var lst = data.Where(x => x.IdParent == null)
                .Select(o => new NhomHangHoaDto
                {
                    Id = o.Id,
                    IdParent = o.IdParent,
                    MaNhomHang = o.MaNhomHang,
                    TenNhomHang = o.TenNhomHang,
                    LaNhomHangHoa = o.LaNhomHangHoa,
                    MoTa = o.MoTa,
                    Color = o.Color,
                    ThuTuHienThi = o.ThuTuHienThi,
                    children = GetChildren(data, o.Id)
                }).OrderBy(x => x.ThuTuHienThi);

            result.Items = ObjectMapper.Map<List<NhomHangHoaDto>>(lst);
            return result;
        }

        public List<NhomHangHoaDto> GetChildren(List<NhomHangHoaDto> data, Guid? idParent)
        {
            return data.Where(o => o.IdParent.Equals(idParent))
                .Select(o =>
                         new NhomHangHoaDto
                         {
                             Id = o.Id,
                             IdParent = o.IdParent,
                             MaNhomHang = o.MaNhomHang,
                             TenNhomHang = o.TenNhomHang,
                             LaNhomHangHoa = o.LaNhomHangHoa,
                             Color = o.Color,
                             MoTa = o.MoTa,
                             children = GetChildren(data, o.Id)
                         }).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [AbpAuthorize(PermissionNames.Pages_DM_NhomHangHoa_Create)]
        public async Task<NhomHangHoaDto> CreateNhomHangHoa(NhomHangHoaDto dto)
        {
            if (dto == null) { return new NhomHangHoaDto(); };
            DM_NhomHangHoa objNew = ObjectMapper.Map<DM_NhomHangHoa>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.CreationTime = DateTime.Now;
            await _dmNhomHangHoa.InsertAsync(objNew);
            var result = ObjectMapper.Map<NhomHangHoaDto>(objNew);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
            nhatKyThaoTacDto.ChucNang = "Nhóm hàng hóa";
            nhatKyThaoTacDto.NoiDung = "Thêm mới nhóm hàng hóa";
            nhatKyThaoTacDto.NoiDungChiTiet = "Thêm mới loại hàng hóa: " + objNew.TenNhomHang;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_DM_NhomHangHoa_Edit)]
        public async Task<string> UpdateNhomHangHoa(NhomHangHoaDto dto)
        {
            try
            {
                if (dto == null) { return "Data null"; };
                DM_NhomHangHoa objUp = await _dmNhomHangHoa.FirstOrDefaultAsync(dto.Id);
                if (objUp == null)
                {
                    return "object null";
                }
                objUp.IdParent = dto.IdParent;
                objUp.MaNhomHang = dto.MaNhomHang;
                objUp.TenNhomHang = dto.TenNhomHang;
                objUp.TenNhomHang_KhongDau = dto.TenNhomHang_KhongDau;
                objUp.LaNhomHangHoa = dto.LaNhomHangHoa;
                objUp.Color = dto.Color;
                objUp.MoTa = dto.MoTa;
                objUp.ThuTuHienThi = dto.ThuTuHienThi;
                objUp.LastModifierUserId = AbpSession.UserId;
                objUp.LastModificationTime = DateTime.Now;

                // update hanghoa thuocnhom
                if (objUp.LaNhomHangHoa != dto.LaNhomHangHoa)
                {
                    _dmHangHoa.GetAllList(x => x.IdNhomHangHoa == dto.Id).ForEach(x => x.IdLoaiHangHoa = (dto.LaNhomHangHoa ?? false) ? 1 : 2);
                }
                await _dmNhomHangHoa.UpdateAsync(objUp);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
                nhatKyThaoTacDto.ChucNang = "Nhóm hàng hóa";
                nhatKyThaoTacDto.NoiDung = "Cập nhật nhóm hàng hóa";
                nhatKyThaoTacDto.NoiDungChiTiet = "Cập nhật loại hàng hóa: " + objUp.TenNhomHang;
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Concat(ex.InnerException + ex.Message);
            }
        }
        [AbpAuthorize(PermissionNames.Pages_DM_NhomHangHoa_Delete)]
        public async Task<string> XoaNhomHangHoa(Guid id)
        {
            try
            {
                DM_NhomHangHoa objUp = await _dmNhomHangHoa.FirstOrDefaultAsync(id);
                if (objUp == null)
                {
                    return "object null";
                }
                objUp.IsDeleted = true;
                objUp.DeletionTime = DateTime.Now;
                objUp.DeleterUserId = AbpSession.UserId;
                await _dmNhomHangHoa.UpdateAsync(objUp);
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Delete;
                nhatKyThaoTacDto.ChucNang = "Nhóm hàng hóa";
                nhatKyThaoTacDto.NoiDung = "Xóa nhóm hàng hóa";
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa loại hàng hóa: " + objUp.TenNhomHang;
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Concat(ex.InnerException + ex.Message);
            }
        }
    }
}
