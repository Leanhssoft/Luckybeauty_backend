

using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.DonViQuiDoi.Dto;
using BanHangBeautify.HangHoa.HangHoa.Dto;
using BanHangBeautify.HangHoa.HangHoa.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static BanHangBeautify.Common.CommonClass;


namespace BanHangBeautify.HangHoa.HangHoa
{
    //[AbpAuthorize(PermissionNames.Pages_DM_LoaiHangHoa)]
    public class HangHoaAppService : SPAAppServiceBase, IHangHoaAppService
    {
        private readonly IRepository<DM_HangHoa, Guid> _dmHangHoa;
        private readonly IRepository<DM_DonViQuiDoi, Guid> _dmDonViQuiDoi;
        private readonly IHangHoaRepository _repository;
        public HangHoaAppService(IRepository<DM_HangHoa, Guid> repository,
            IHangHoaRepository productRepo,
            IRepository<DM_DonViQuiDoi, Guid> dvqd
            )
        {
            _dmHangHoa = repository;
            _dmDonViQuiDoi = dvqd;
            _repository = productRepo;
        }
        public async Task<CreateOrEditHangHoaDto> CreateOrEdit(CreateOrEditHangHoaDto dto)
        {
            var findHangHoa = await _dmHangHoa.FirstOrDefaultAsync(h => h.Id == dto.Id);
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
        public async Task<CreateOrEditHangHoaDto> Create(CreateOrEditHangHoaDto dto)
        {
            List<DM_DonViQuiDoi> lstDVT = new();
            DM_HangHoa hangHoa = ObjectMapper.Map<DM_HangHoa>(dto);
            Guid productId = Guid.NewGuid();
            hangHoa.Id = productId;
            hangHoa.TenantId = AbpSession.TenantId ?? 1;
            hangHoa.CreatorUserId = AbpSession.UserId;
            hangHoa.CreationTime = DateTime.Now;

            if (dto.DonViQuiDois != null && dto.DonViQuiDois.Count > 0)
            {
                MaxCodeDto objMax = await _repository.SpGetProductCode(dto.IdLoaiHangHoa, hangHoa.TenantId);
                var max = objMax.MaxVal;
                foreach (var item in dto.DonViQuiDois)
                {
                    DM_DonViQuiDoi dvt = ObjectMapper.Map<DM_DonViQuiDoi>(item);
                    dvt.Id = Guid.NewGuid();
                    dvt.TenantId = hangHoa.TenantId;
                    dvt.IdHangHoa = productId;
                    dvt.MaHangHoa = string.Concat(objMax.FirstStr, max);
                    lstDVT.Add(dvt);
                    max += 1;
                }
            }
            else
            {
                MaxCodeDto objMax = await _repository.SpGetProductCode(dto.IdLoaiHangHoa, hangHoa.TenantId);
                DM_DonViQuiDoi dvt = new()
                {
                    Id = Guid.NewGuid(),
                    IdHangHoa = productId,
                    TenantId = hangHoa.TenantId,
                    MaHangHoa = string.Concat(objMax.FirstStr, objMax.MaxVal),
                    TenDonViTinh = string.Empty,
                };
                lstDVT.Add(dvt);
            }
            await _dmHangHoa.InsertAsync(hangHoa);
            await _dmDonViQuiDoi.InsertRangeAsync(lstDVT);

            hangHoa.DonViQuiDois = lstDVT;
            var result = ObjectMapper.Map<CreateOrEditHangHoaDto>(hangHoa);
            return result;
        }

        [NonAction]
        public async Task<CreateOrEditHangHoaDto> Edit(CreateOrEditHangHoaDto dto, DM_HangHoa hangHoa)
        {
            #region compare dvt & update IsDeleted = true if not exists
            var dvt = _dmDonViQuiDoi.GetAllList(x => x.IdHangHoa == hangHoa.Id);
            var idOlds = dvt.Select(x => x.Id).ToList();
            var idNews = dto.DonViQuiDois.Select(x => x.Id);
            var idDeletes = (from idOld in idOlds
                             join idNew in idNews on idOld equals idNew
                             into tbl
                             from de in tbl.DefaultIfEmpty()
                             where de == Guid.Empty
                             select idOld).ToList();
            _dmDonViQuiDoi.GetAllList(x => idDeletes.Contains(x.Id)).ForEach(x => x.IsDeleted = true);
            #endregion

            hangHoa.IdLoaiHangHoa = dto.IdLoaiHangHoa;
            hangHoa.TenHangHoa = dto.TenHangHoa;
            hangHoa.TrangThai = dto.TrangThai;
            hangHoa.LastModificationTime = DateTime.Now;
            hangHoa.LastModifierUserId = AbpSession.UserId;
            await _dmHangHoa.UpdateAsync(hangHoa);

            foreach (var item in dto.DonViQuiDois)
            {
                DM_DonViQuiDoi objDVT = _dmDonViQuiDoi.FirstOrDefault(item.Id);
                if (objDVT != null)
                {
                    // update
                    objDVT.MaHangHoa = item.MaHangHoa;
                    objDVT.TenDonViTinh = item.TenDonViTinh;
                    objDVT.TyLeChuyenDoi = item.TyLeChuyenDoi;
                    objDVT.GiaBan = item.GiaBan;
                    await _dmDonViQuiDoi.UpdateAsync(objDVT);
                }
                else
                {
                    // insert
                    MaxCodeDto objMax = await _repository.SpGetProductCode(dto.IdLoaiHangHoa, hangHoa.TenantId);
                    DM_DonViQuiDoi dvtNew = ObjectMapper.Map<DM_DonViQuiDoi>(item);
                    dvtNew.MaHangHoa = string.Concat(objMax.FirstStr, objMax.MaxVal);
                    dvtNew.IdHangHoa = hangHoa.Id;
                    dvtNew.TenantId = hangHoa.TenantId;
                    dvtNew.LaDonViTinhChuan = item.LaDonViTinhChuan;
                    await _dmDonViQuiDoi.InsertAsync(dvtNew);
                    hangHoa.DonViQuiDois.Add(dvtNew);// used to return
                }
            }

            // only return dvt not delete (todo)
            var result = ObjectMapper.Map<CreateOrEditHangHoaDto>(hangHoa);
            return result;
        }
        public async Task<HangHoaDto> GetDetailProduct(Guid idDonViQuyDoi)
        {
            return await _repository.GetDetailProduct(idDonViQuyDoi, AbpSession.TenantId ?? 1);
        }
        public async Task<PagedResultDto<DM_HangHoa>> GetAll(HangHoaPagedResultRequestDto input)
        {
            PagedResultDto<DM_HangHoa> result = new PagedResultDto<DM_HangHoa>();
            var lstHangHoa = await _dmHangHoa.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstHangHoa.Count();
            if (!string.IsNullOrEmpty(input.TextSearch))
            {
                lstHangHoa = lstHangHoa.Where(x => x.TenHangHoa.Contains(input.TextSearch) || x.TenHangHoa.Contains(input.TextSearch)).ToList();
            }
            if (input.CurrentPage > 0)
            {
                input.CurrentPage *= 10;
            }
            result.Items = lstHangHoa.Skip(input.CurrentPage ?? 0).Take(input.CurrentPage ?? 10).ToList();
            return result;
        }
        [HttpPost]
        public async Task<PagedResultDto<HangHoaDto>> GetDMHangHoa(HangHoaPagedResultRequestDto input)
        {
            return await _repository.GetDMHangHoa(input, AbpSession.TenantId ?? 1);
        }
        [HttpPost]
        public async Task<CreateOrEditHangHoaDto> Delete(Guid id)
        {
            CreateOrEditHangHoaDto result = new();
            var findHangHoa = await _dmHangHoa.FirstOrDefaultAsync(h => h.Id == id);
            if (findHangHoa != null)
            {
                findHangHoa.IsDeleted = true;
                findHangHoa.TrangThai = 0;
                findHangHoa.DeletionTime = DateTime.Now;
                findHangHoa.DeleterUserId = AbpSession.UserId;
                _dmHangHoa.Update(findHangHoa);

                _dmDonViQuiDoi.GetAllList(x => x.IdHangHoa == id).ForEach(x => x.IsDeleted = true);

                result = ObjectMapper.Map<CreateOrEditHangHoaDto>(findHangHoa);
            }
            return result;
        }
    }
}
