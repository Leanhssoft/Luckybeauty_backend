using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.NhomHangHoa.Dto;
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

        public NhomHangHoaAppService(IRepository<DM_NhomHangHoa, Guid> dmNhomHangHoa, IRepository<DM_HangHoa, Guid> dmHangHoa)
        {
            _dmNhomHangHoa = dmNhomHangHoa;
            _dmHangHoa = dmHangHoa;
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
                    children = GetChildren(data, o.Id)
                });

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
        public NhomHangHoaDto CreateNhomHangHoa(NhomHangHoaDto dto)
        {
            if (dto == null) { return new NhomHangHoaDto(); };
            DM_NhomHangHoa objNew = ObjectMapper.Map<DM_NhomHangHoa>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.CreationTime = DateTime.Now;
            _dmNhomHangHoa.InsertAsync(objNew);
            var result = ObjectMapper.Map<NhomHangHoaDto>(objNew);
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
                objUp.LastModifierUserId = AbpSession.UserId;
                objUp.LastModificationTime = DateTime.Now;

                // update hanghoa thuocnhom
                if (objUp.LaNhomHangHoa != dto.LaNhomHangHoa)
                {
                    _dmHangHoa.GetAllList(x => x.IdNhomHangHoa == dto.Id).ForEach(x => x.IdLoaiHangHoa = (dto.LaNhomHangHoa ?? false) ? 1 : 2);
                }
                await _dmNhomHangHoa.UpdateAsync(objUp);
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
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Concat(ex.InnerException + ex.Message);
            }
        }
    }
}
