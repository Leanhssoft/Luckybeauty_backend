﻿using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.KhuyenMai.KhuyenMai.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.KhuyenMai.KhuyenMai
{
    [AbpAuthorize(PermissionNames.Pages_KhuyenMai)]
    public class KhuyenMaiAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_KhuyenMai, Guid> _khuyenMaiRepository;
        private readonly IRepository<DM_KhuyenMai_ApDung, Guid> _khuyenMaiApDungService;
        private readonly IRepository<DM_KhuyenMai_ChiTiet, Guid> _khuyenMaiChiTIetService;
        public KhuyenMaiAppService(IRepository<DM_KhuyenMai, Guid> khuyenMaiRepository,
            IRepository<DM_KhuyenMai_ApDung, Guid> khuyenMaiApDungService,
            IRepository<DM_KhuyenMai_ChiTiet, Guid> khuyenMaiChiTIetService)
        {
            _khuyenMaiRepository = khuyenMaiRepository;
            _khuyenMaiApDungService = khuyenMaiApDungService;
            _khuyenMaiChiTIetService = khuyenMaiChiTIetService;
        }
        [AbpAuthorize(PermissionNames.Pages_KhuyenMai_Create, PermissionNames.Pages_KhuyenMai_Edit)]
        public async Task<KhuyenMaiDto> CreateOrEdit(CreateOrEditKhuyenMaiDto input)
        {
            var checkExist = await _khuyenMaiRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist != null)
            {
                return await Update(input, checkExist);
            }
            return await Create(input);
        }
        [NonAction]
        public async Task<KhuyenMaiDto> Create(CreateOrEditKhuyenMaiDto input)
        {
            KhuyenMaiDto result = new KhuyenMaiDto();
            DM_KhuyenMai data = new DM_KhuyenMai();
            data = ObjectMapper.Map<DM_KhuyenMai>(input);
            data.Id = Guid.NewGuid();
            data.ThangApDung = string.Join(";", input.ThangApDung);
            data.ThuApDung = string.Join(";", input.ThuApDung);
            data.NgayApDung = string.Join(";", input.NgayApDung);
            data.GioApDung = string.Join(";", input.GioApDung);
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _khuyenMaiRepository.InsertAsync(data);
            if (input.IdChiNhanhs.Count > 0 && input.TatCaChiNhanh==false)
            {
                foreach (var chiNhanh in input.IdChiNhanhs)
                {
                    if (input.IdNhanViens.Count > 0)
                    {
                        foreach (var nhanVien in input.IdNhanViens)
                        {
                            if (input.IdNhomKhachs.Count > 0)
                            {
                                foreach (var nhomKhach in input.IdNhomKhachs)
                                {
                                    DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                                    khuyenMaiApDung.Id = Guid.NewGuid();
                                    khuyenMaiApDung.IdChiNhanh = chiNhanh;
                                    khuyenMaiApDung.IdNhanVien = nhanVien;
                                    khuyenMaiApDung.IdNhomKhach = nhomKhach;
                                    khuyenMaiApDung.CreationTime = DateTime.Now;
                                    khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                                    khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                                    await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                                }
                            }
                            else
                            {
                                DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                                khuyenMaiApDung.Id = Guid.NewGuid();
                                khuyenMaiApDung.IdChiNhanh = chiNhanh;
                                khuyenMaiApDung.IdNhanVien = nhanVien;
                                khuyenMaiApDung.CreationTime = DateTime.Now;
                                khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                                khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                                await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                            }
                        }
                    }
                    else if (input.IdNhomKhachs.Count > 0)
                    {
                        foreach (var nhomKhach in input.IdNhomKhachs)
                        {
                            DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                            khuyenMaiApDung.Id = Guid.NewGuid();
                            khuyenMaiApDung.IdChiNhanh = chiNhanh;
                            khuyenMaiApDung.IdNhomKhach = nhomKhach;
                            khuyenMaiApDung.CreationTime = DateTime.Now;
                            khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                            khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                            await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                        }
                    }
                }
            }
            else if (input.IdNhanViens.Count > 0 && input.IdChiNhanhs == null && input.IdChiNhanhs.Count==0 && input.TatCaNhanVien==false)
            {

                foreach (var nhanVien in input.IdNhanViens)
                {
                    if (input.IdNhomKhachs.Count > 0)
                    {
                        foreach (var nhomKhach in input.IdNhomKhachs)
                        {
                            DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                            khuyenMaiApDung.Id = Guid.NewGuid();
                            khuyenMaiApDung.IdNhanVien = nhanVien;
                            khuyenMaiApDung.IdNhomKhach = nhomKhach;
                            khuyenMaiApDung.CreationTime = DateTime.Now;
                            khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                            khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                            await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                        }
                    }
                    else
                    {
                        DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                        khuyenMaiApDung.Id = Guid.NewGuid();
                        khuyenMaiApDung.IdNhanVien = nhanVien;
                        khuyenMaiApDung.CreationTime = DateTime.Now;
                        khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                        khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                        await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                    }
                }
            }
            else if (input.IdNhomKhachs.Count > 0 && input.IdNhanViens==null && input.IdNhomKhachs==null && input.IdNhanViens.Count==0 && input.IdNhomKhachs.Count==0 && input.TatCaKhachHang==false)
            {
                foreach (var nhomKhach in input.IdNhomKhachs)
                {
                    DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                    khuyenMaiApDung.Id = Guid.NewGuid();
                    khuyenMaiApDung.IdNhomKhach = nhomKhach;
                    khuyenMaiApDung.CreationTime = DateTime.Now;
                    khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                    khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                    await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                }
            }

            
            result = ObjectMapper.Map<KhuyenMaiDto>(input);
            return result;
        }
        [NonAction]
        public async Task<KhuyenMaiDto> Update(CreateOrEditKhuyenMaiDto input, DM_KhuyenMai oldData)
        {
            KhuyenMaiDto result = new KhuyenMaiDto();
            oldData.GhiChu = input.GhiChu;
            oldData.TatCaChiNhanh = input.TatCaChiNhanh;
            oldData.TatCaNhanVien = input.TatCaNhanVien;
            oldData.TatCaKhachHang = input.TatCaKhachHang;
            oldData.ThangApDung = string.Join(";", input.ThangApDung);
            oldData.ThuApDung = string.Join(";", input.ThuApDung);
            oldData.NgayApDung = string.Join(";", input.NgayApDung);
            oldData.GioApDung = string.Join(";", input.GioApDung);
            oldData.MaKhuyenMai = input.MaKhuyenMai;
            oldData.TenKhuyenMai = input.TenKhuyenMai;
            oldData.ThoiGianApDung = input.ThoiGianApDung;
            oldData.ThoiGianKetThuc = input.ThoiGianKetThuc;
            oldData.HinhThucKM = input.HinhThucKM;
            oldData.LoaiKhuyenMai = input.LoaiKhuyenMai;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _khuyenMaiRepository.UpdateAsync(oldData);
            result = ObjectMapper.Map<KhuyenMaiDto>(oldData);
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_KhuyenMai_Delete)]
        public async Task<KhuyenMaiDto> Delete(Guid id)
        {
            var data = await _khuyenMaiRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                data.IsDeleted = true;
                _khuyenMaiRepository.Update(data);
                return ObjectMapper.Map<KhuyenMaiDto>(data);
            }
            return new KhuyenMaiDto();
        }
        public async Task<CreateOrEditKhuyenMaiDto> GetForEdit(Guid id)
        {
            var data = await _khuyenMaiRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditKhuyenMaiDto>(data);
            }
            return new CreateOrEditKhuyenMaiDto();
        }
        public async Task<PagedResultDto<KhuyenMaiDto>> GetALl(PagedRequestDto input)
        {
            PagedResultDto<KhuyenMaiDto> result = new PagedResultDto<KhuyenMaiDto>();
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            var lstData = await _khuyenMaiRepository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 1)).OrderByDescending(x => x.CreationTime).ToListAsync();
            result.TotalCount = lstData.Count;
            var data = lstData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<KhuyenMaiDto>>(data);
            return result;
        }
    }
}
