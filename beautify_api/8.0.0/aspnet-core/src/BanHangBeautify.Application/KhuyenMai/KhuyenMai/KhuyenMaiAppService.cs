using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using BanHangBeautify.Authorization;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.KhuyenMai.KhuyenMai.Dto;
using BanHangBeautify.NhatKyHoatDong;
using BanHangBeautify.NhatKyHoatDong.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BanHangBeautify.KhuyenMai.KhuyenMai
{
    [AbpAuthorize(PermissionNames.Pages_KhuyenMai)]
    public class KhuyenMaiAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_KhuyenMai, Guid> _khuyenMaiRepository;
        private readonly IRepository<DM_KhuyenMai_ApDung, Guid> _khuyenMaiApDungService;
        private readonly IRepository<DM_KhuyenMai_ChiTiet, Guid> _khuyenMaiChiTietService;
        private readonly IRepository<DM_DonViQuiDoi, Guid> _donViQuiDoiService;
        private readonly IRepository<DM_NhomHangHoa, Guid> _nhomHangHoaService;
        INhatKyThaoTacAppService _audilogService;
        public KhuyenMaiAppService(IRepository<DM_KhuyenMai, Guid> khuyenMaiRepository,
            IRepository<DM_KhuyenMai_ApDung, Guid> khuyenMaiApDungService,
            IRepository<DM_KhuyenMai_ChiTiet, Guid> khuyenMaiChiTietService,
            IRepository<DM_DonViQuiDoi, Guid> donViQuiDoiService,
            IRepository<DM_NhomHangHoa, Guid> nhomHangHoaService,
            INhatKyThaoTacAppService audilogService)
        {
            _khuyenMaiRepository = khuyenMaiRepository;
            _khuyenMaiApDungService = khuyenMaiApDungService;
            _khuyenMaiChiTietService = khuyenMaiChiTietService;
            _donViQuiDoiService = donViQuiDoiService;
            _nhomHangHoaService = nhomHangHoaService;
            _audilogService = audilogService;
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
            if (string.IsNullOrEmpty(input.MaKhuyenMai))
            {
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    var countKhuyenMai = _khuyenMaiRepository.Count();
                    if (countKhuyenMai.ToString().Length >= 3)
                    {
                        data.MaKhuyenMai = "KM" + (countKhuyenMai + 1).ToString();
                    }
                    else if (countKhuyenMai.ToString().Length == 2)
                    {
                        data.MaKhuyenMai = "KM0" + (countKhuyenMai + 1).ToString();
                    }
                    else
                    {
                        data.MaKhuyenMai = "KM00" + (countKhuyenMai + 1).ToString();
                    }
                }

            }
            if (input.ThangApDung != null && input.ThangApDung.Count > 0)
            {
                data.ThangApDung = string.Join(";", input.ThangApDung);
            }
            if (input.ThuApDung != null && input.ThuApDung.Count > 0)
            {
                data.ThuApDung = string.Join(";", input.ThuApDung);
            }
            if (input.NgayApDung != null && input.NgayApDung.Count > 0)
            {
                data.NgayApDung = string.Join(";", input.NgayApDung);
            }
            if (input.GioApDung != null && input.GioApDung.Count > 0)
            {
                data.GioApDung = string.Join(";", input.GioApDung);
            }
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 1;
            data.IsDeleted = false;
            await _khuyenMaiRepository.InsertAsync(data);
            if (input.TatCaChiNhanh == false && input.IdChiNhanhs.Count > 0)
            {
                foreach (var chiNhanh in input.IdChiNhanhs)
                {
                    if (input.IdNhanViens != null && input.IdNhanViens.Count > 0)
                    {
                        foreach (var nhanVien in input.IdNhanViens)
                        {
                            if (input.IdNhomKhachs != null && input.IdNhomKhachs.Count > 0)
                            {
                                foreach (var nhomKhach in input.IdNhomKhachs)
                                {
                                    DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                                    khuyenMaiApDung.Id = Guid.NewGuid();
                                    khuyenMaiApDung.IdKhuyenMai = data.Id;
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
                                khuyenMaiApDung.IdKhuyenMai = data.Id;
                                khuyenMaiApDung.IdChiNhanh = chiNhanh;
                                khuyenMaiApDung.IdNhanVien = nhanVien;
                                khuyenMaiApDung.CreationTime = DateTime.Now;
                                khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                                khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                                await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                            }
                        }
                    }
                    else if (input.IdNhomKhachs != null && input.IdNhomKhachs.Count > 0)
                    {
                        foreach (var nhomKhach in input.IdNhomKhachs)
                        {
                            DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                            khuyenMaiApDung.Id = Guid.NewGuid();
                            khuyenMaiApDung.IdKhuyenMai = data.Id;
                            khuyenMaiApDung.IdChiNhanh = chiNhanh;
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
                        khuyenMaiApDung.IdKhuyenMai = data.Id;
                        khuyenMaiApDung.IdChiNhanh = chiNhanh;
                        khuyenMaiApDung.CreationTime = DateTime.Now;
                        khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                        khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                        await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                    }
                }
            }
            else if (input.IdNhanViens != null && input.IdNhanViens.Count > 0 && (input.IdChiNhanhs != null || input.IdChiNhanhs.Count == 0) && input.TatCaNhanVien == false)
            {

                foreach (var nhanVien in input.IdNhanViens)
                {
                    if (input.IdNhomKhachs != null && input.IdNhomKhachs.Count > 0)
                    {
                        foreach (var nhomKhach in input.IdNhomKhachs)
                        {
                            DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                            khuyenMaiApDung.Id = Guid.NewGuid();
                            khuyenMaiApDung.IdKhuyenMai = data.Id;
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
                        khuyenMaiApDung.IdKhuyenMai = data.Id;
                        khuyenMaiApDung.IdNhanVien = nhanVien;
                        khuyenMaiApDung.CreationTime = DateTime.Now;
                        khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                        khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                        await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                    }
                }
            }
            else if (input.IdNhomKhachs != null && input.IdNhomKhachs.Count > 0 && input.IdNhanViens == null && input.TatCaKhachHang == false)
            {
                foreach (var nhomKhach in input.IdNhomKhachs)
                {
                    DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                    khuyenMaiApDung.Id = Guid.NewGuid();
                    khuyenMaiApDung.IdKhuyenMai = data.Id;
                    khuyenMaiApDung.IdNhomKhach = nhomKhach;
                    khuyenMaiApDung.CreationTime = DateTime.Now;
                    khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                    khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                    await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                }
            }

            if (input.KhuyenMaiChiTiets != null && input.KhuyenMaiChiTiets.Count > 0)
            {
                foreach (var item in input.KhuyenMaiChiTiets)
                {
                    var khuyenMaiCT = ObjectMapper.Map<DM_KhuyenMai_ChiTiet>(item);
                    khuyenMaiCT.Id = Guid.NewGuid();
                    khuyenMaiCT.CreationTime = DateTime.Now;
                    khuyenMaiCT.CreatorUserId = AbpSession.UserId;
                    khuyenMaiCT.TenantId = AbpSession.TenantId ?? 1;
                    khuyenMaiCT.IdKhuyenMai = data.Id;
                    await _khuyenMaiChiTietService.InsertAsync(khuyenMaiCT);
                }
            }
            result = ObjectMapper.Map<KhuyenMaiDto>(input);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
            nhatKyThaoTacDto.ChucNang = "Khuyến mại";
            nhatKyThaoTacDto.NoiDung = "Thêm mới chương trình khuyến mại";
            nhatKyThaoTacDto.NoiDungChiTiet = "Thêm mới chương trình khuyến mại: " + data.TenKhuyenMai;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
            return result;
        }
        [NonAction]
        public async Task<KhuyenMaiDto> Update(CreateOrEditKhuyenMaiDto input, DM_KhuyenMai oldData)
        {
            var khuyenMaiApDungs = _khuyenMaiApDungService.GetAll().Where(x => x.IdKhuyenMai == oldData.Id).ToList();
            if (khuyenMaiApDungs != null && khuyenMaiApDungs.Count > 0)
            {
                foreach (var item in khuyenMaiApDungs)
                {
                    await _khuyenMaiApDungService.HardDeleteAsync(item);
                }
            }
            var khuyenMaiCTs = _khuyenMaiChiTietService.GetAll().Where(x => x.IdKhuyenMai == oldData.Id).ToList();
            if (khuyenMaiCTs != null && khuyenMaiCTs.Count > 0)
            {
                foreach (var item in khuyenMaiCTs)
                {
                    await _khuyenMaiChiTietService.HardDeleteAsync(item);
                }
            }
            KhuyenMaiDto result = new KhuyenMaiDto();
            oldData.GhiChu = input.GhiChu;
            oldData.TatCaChiNhanh = input.TatCaChiNhanh;
            oldData.TatCaNhanVien = input.TatCaNhanVien;
            oldData.TatCaKhachHang = input.TatCaKhachHang;
            if (input.ThangApDung != null && input.ThangApDung.Count > 0)
            {
                oldData.ThangApDung = string.Join(";", input.ThangApDung);
            }
            if (input.ThuApDung != null && input.ThuApDung.Count > 0)
            {
                oldData.ThuApDung = string.Join(";", input.ThuApDung);
            }
            if (input.NgayApDung != null && input.NgayApDung.Count > 0)
            {
                oldData.NgayApDung = string.Join(";", input.NgayApDung);
            }
            if (input.GioApDung != null && input.GioApDung.Count > 0)
            {
                oldData.GioApDung = string.Join(";", input.GioApDung);
            }
            oldData.MaKhuyenMai = input.MaKhuyenMai;
            oldData.TenKhuyenMai = input.TenKhuyenMai;
            oldData.ThoiGianApDung = input.ThoiGianApDung;
            oldData.ThoiGianKetThuc = input.ThoiGianKetThuc;
            oldData.HinhThucKM = input.HinhThucKM;
            oldData.LoaiKhuyenMai = input.LoaiKhuyenMai;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _khuyenMaiRepository.UpdateAsync(oldData);
            if (input.TatCaChiNhanh == false && input.IdChiNhanhs.Count > 0)
            {
                foreach (var chiNhanh in input.IdChiNhanhs)
                {
                    if (input.IdNhanViens != null && input.IdNhanViens.Count > 0)
                    {
                        foreach (var nhanVien in input.IdNhanViens)
                        {
                            if (input.IdNhomKhachs != null && input.IdNhomKhachs.Count > 0)
                            {
                                foreach (var nhomKhach in input.IdNhomKhachs)
                                {
                                    DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                                    khuyenMaiApDung.Id = Guid.NewGuid();
                                    khuyenMaiApDung.IdKhuyenMai = oldData.Id;
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
                                khuyenMaiApDung.IdKhuyenMai = oldData.Id;
                                khuyenMaiApDung.IdChiNhanh = chiNhanh;
                                khuyenMaiApDung.IdNhanVien = nhanVien;
                                khuyenMaiApDung.CreationTime = DateTime.Now;
                                khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                                khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                                await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                            }
                        }
                    }
                    else if (input.IdNhomKhachs != null && input.IdNhomKhachs.Count > 0)
                    {
                        foreach (var nhomKhach in input.IdNhomKhachs)
                        {
                            DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                            khuyenMaiApDung.Id = Guid.NewGuid();
                            khuyenMaiApDung.IdKhuyenMai = oldData.Id;
                            khuyenMaiApDung.IdChiNhanh = chiNhanh;
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
                        khuyenMaiApDung.IdKhuyenMai = oldData.Id;
                        khuyenMaiApDung.IdChiNhanh = chiNhanh;
                        khuyenMaiApDung.CreationTime = DateTime.Now;
                        khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                        khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                        await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                    }
                }
            }
            else if (input.IdNhanViens != null && input.IdNhanViens.Count > 0 && (input.IdChiNhanhs != null || input.IdChiNhanhs.Count == 0) && input.TatCaNhanVien == false)
            {

                foreach (var nhanVien in input.IdNhanViens)
                {
                    if (input.IdNhomKhachs != null && input.IdNhomKhachs.Count > 0)
                    {
                        foreach (var nhomKhach in input.IdNhomKhachs)
                        {
                            DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                            khuyenMaiApDung.Id = Guid.NewGuid();
                            khuyenMaiApDung.IdKhuyenMai = oldData.Id;
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
                        khuyenMaiApDung.IdKhuyenMai = oldData.Id;
                        khuyenMaiApDung.IdNhanVien = nhanVien;
                        khuyenMaiApDung.CreationTime = DateTime.Now;
                        khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                        khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                        await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                    }
                }
            }
            else if (input.IdNhomKhachs != null && input.IdNhomKhachs.Count > 0 && input.IdNhanViens == null && input.TatCaKhachHang == false)
            {
                foreach (var nhomKhach in input.IdNhomKhachs)
                {
                    DM_KhuyenMai_ApDung khuyenMaiApDung = new DM_KhuyenMai_ApDung();
                    khuyenMaiApDung.Id = Guid.NewGuid();
                    khuyenMaiApDung.IdKhuyenMai = oldData.Id;
                    khuyenMaiApDung.IdNhomKhach = nhomKhach;
                    khuyenMaiApDung.CreationTime = DateTime.Now;
                    khuyenMaiApDung.CreatorUserId = AbpSession.UserId;
                    khuyenMaiApDung.TenantId = AbpSession.TenantId ?? 1;
                    await _khuyenMaiApDungService.InsertAsync(khuyenMaiApDung);
                }
            }

            if (input.KhuyenMaiChiTiets != null && input.KhuyenMaiChiTiets.Count > 0)
            {
                foreach (var item in input.KhuyenMaiChiTiets)
                {
                    var khuyenMaiCT = ObjectMapper.Map<DM_KhuyenMai_ChiTiet>(item);
                    khuyenMaiCT.Id = Guid.NewGuid();
                    khuyenMaiCT.IdKhuyenMai = oldData.Id;
                    khuyenMaiCT.CreationTime = DateTime.Now;
                    khuyenMaiCT.CreatorUserId = AbpSession.UserId;
                    khuyenMaiCT.TenantId = AbpSession.TenantId ?? 1;
                    await _khuyenMaiChiTietService.InsertAsync(khuyenMaiCT);
                }
            }
            result = ObjectMapper.Map<KhuyenMaiDto>(oldData);
            var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
            nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Update;
            nhatKyThaoTacDto.ChucNang = "Khuyến mại";
            nhatKyThaoTacDto.NoiDung = "Cập nhật chương trình khuyến mại";
            nhatKyThaoTacDto.NoiDungChiTiet = "Cập nhật chương trình khuyến mại: " + oldData.TenKhuyenMai;
            await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
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
                var nhatKyThaoTacDto = new CreateNhatKyThaoTacDto();
                nhatKyThaoTacDto.LoaiNhatKy = LoaiThaoTacConst.Create;
                nhatKyThaoTacDto.ChucNang = "Khuyến mại";
                nhatKyThaoTacDto.NoiDung = "Xóa chương trình khuyến mại";
                nhatKyThaoTacDto.NoiDungChiTiet = "Xóa chương trình khuyến mại: " + data.TenKhuyenMai;
                await _audilogService.CreateNhatKyHoatDong(nhatKyThaoTacDto);
                return ObjectMapper.Map<KhuyenMaiDto>(data);
            }
            return new KhuyenMaiDto();
        }
        public async Task<CreateOrEditKhuyenMaiDto> GetForEdit(Guid id)
        {
            var data = await _khuyenMaiRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                var createOrEditKhuyenMai = ObjectMapper.Map<CreateOrEditKhuyenMaiDto>(data);
                var khuyenMaiChiTiet = _khuyenMaiChiTietService.GetAllList(x => x.IdKhuyenMai == id);
                if (khuyenMaiChiTiet != null && khuyenMaiChiTiet.Count > 0)
                {
                    var khuyenMaiCTMap = new List<KhuyenMaiChiTietMap>();
                    foreach (var item in khuyenMaiChiTiet)
                    {
                        var khuyenMaiCT = ObjectMapper.Map<KhuyenMaiChiTietMap>(item);
                        khuyenMaiCTMap.Add(khuyenMaiCT);
                    }
                    createOrEditKhuyenMai.KhuyenMaiChiTiets = khuyenMaiCTMap;
                }
                var khuyenMaiApDung = await _khuyenMaiApDungService.GetAllListAsync(x => x.IdKhuyenMai == id);
                if (khuyenMaiApDung != null && khuyenMaiApDung.Count > 0)
                {
                    var idChiNhanhs = khuyenMaiApDung.Where(x => x.IdChiNhanh != null).ToList().Select(x => x.IdChiNhanh).ToList();
                    var idNhanViens = khuyenMaiApDung.Where(x => x.IdNhanVien != null).ToList().Select(x => x.IdNhanVien).ToList();
                    var idNhomKhachs = khuyenMaiApDung.Where(x => x.IdNhomKhach != null).ToList().Select(x => x.IdNhomKhach).ToList();
                    createOrEditKhuyenMai.IdChiNhanhs = idChiNhanhs ?? new List<Guid?>();
                    createOrEditKhuyenMai.IdNhanViens = idNhanViens ?? new List<Guid?>();
                    createOrEditKhuyenMai.IdNhomKhachs = idNhomKhachs ?? new List<Guid?>();
                }
                if (!string.IsNullOrEmpty(data.NgayApDung))
                {
                    createOrEditKhuyenMai.NgayApDung = data.NgayApDung.Split(";").ToList();
                }
                if (!string.IsNullOrEmpty(data.ThangApDung))
                {
                    createOrEditKhuyenMai.ThangApDung = data.ThangApDung.Split(";").ToList();
                }
                if (!string.IsNullOrEmpty(data.ThuApDung))
                {
                    createOrEditKhuyenMai.ThuApDung = data.ThuApDung.Split(";").ToList();
                }
                if (!string.IsNullOrEmpty(data.GioApDung))
                {
                    createOrEditKhuyenMai.GioApDung = data.GioApDung.Split(";").ToList();
                }
                return createOrEditKhuyenMai;
            }
            return new CreateOrEditKhuyenMaiDto();
        }
        [HttpPost]
        public async Task<PagedResultDto<KhuyenMaiDto>> GetAll(PagedRequestDto input)
        {
            PagedResultDto<KhuyenMaiDto> result = new PagedResultDto<KhuyenMaiDto>();
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            var query = from km in _khuyenMaiRepository.GetAll()
                        join kmct in _khuyenMaiChiTietService.GetAll() on km.Id equals kmct.IdKhuyenMai
                        where km.TenantId == (AbpSession.TenantId ?? 1)
                            && km.IsDeleted == false
                            && (km.TenKhuyenMai.ToLower().Contains(input.Keyword.ToLower())
                                || km.MaKhuyenMai.ToLower().Contains(input.Keyword.ToLower())
                            )
                        orderby km.CreationTime descending
                        select new KhuyenMaiDto()
                        {
                            Id = km.Id,
                            GhiChu = km.GhiChu,
                            MaKhuyenMai = km.MaKhuyenMai,
                            TenKhuyenMai = km.TenKhuyenMai,
                            GioApDung = km.GioApDung,
                            NgayApDung = km.NgayApDung,
                            ThangApDunng = km.ThangApDung,
                            ThuApDung = km.ThuApDung,
                            ThoiGianApDung = km.ThoiGianApDung,
                            ThoiGianKetThuc = km.ThoiGianKetThuc,
                            HinhThucKM = km.HinhThucKM.ToString(),
                            LoaiKhuyenMai = km.LoaiKhuyenMai == 1 ? "Hóa đơn" : "Hàng hóa",
                            TrangThai = km.TrangThai
                        };
            result.TotalCount = query.Count();
            #region Sorting Data
            if (input.SortType == "desc" && input.SortBy == "hinhThucKM")
            {
                query.OrderByDescending(x => x.HinhThucKM).ToList();
            }
            else if (input.SortType == "asc" && input.SortBy == "hinhThucKM")
            {
                query.OrderByDescending(x => x.HinhThucKM).Reverse().ToList();
            }
            else if (input.SortType == "desc" && input.SortBy == "maKhuyenMai")
            {
                query.OrderByDescending(x => x.MaKhuyenMai).ToList();
            }
            else if (input.SortType == "asc" && input.SortBy == "maKhuyenMai")
            {
                query.OrderByDescending(x => x.HinhThucKM).Reverse().ToList();
            }
            else if (input.SortType == "desc" && input.SortBy == "tenKhuyenMai")
            {
                query.OrderByDescending(x => x.TenKhuyenMai).ToList();
            }
            else if (input.SortType == "asc" && input.SortBy == "tenKhuyenMai")
            {
                query.OrderByDescending(x => x.TenKhuyenMai).Reverse().ToList();
            }
            else if (input.SortType == "desc" && input.SortBy == "thoiGianApDung")
            {
                query.OrderByDescending(x => x.ThoiGianApDung).ToList();
            }
            else if (input.SortType == "asc" && input.SortBy == "thoiGianApDung")
            {
                query.OrderByDescending(x => x.ThoiGianApDung).Reverse().ToList();
            }
            else if (input.SortType == "desc" && input.SortBy == "thoiGianKetThuc")
            {
                query.OrderByDescending(x => x.ThoiGianKetThuc).ToList();
            }
            else if (input.SortType == "asc" && input.SortBy == "thoiGianKetThuc")
            {
                query.OrderByDescending(x => x.ThoiGianKetThuc).Reverse().ToList();
            }
            else if (input.SortType == "desc" && input.SortBy == "trangThai")
            {
                query.OrderByDescending(x => x.TrangThai).ToList();
            }
            else if (input.SortType == "asc" && input.SortBy == "trangThai")
            {
                query.OrderByDescending(x => x.TrangThai).Reverse().ToList();
            }
            #endregion
            var data = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            foreach (var item in data)
            {
                switch (item.HinhThucKM)
                {
                    case "11":
                        item.HinhThucKM = "Hóa đơn - Giảm giá hóa đơn";
                        break;
                    case "12":
                        item.HinhThucKM = "Hóa đơn - Tặng hàng";
                        break;
                    case "13":
                        item.HinhThucKM = "Hóa đơn - Giảm giá hàng";
                        break;
                    case "14":
                        item.HinhThucKM = "Hóa đơn - Tặng điểm";
                        break;
                    case "21":
                        item.HinhThucKM = "Hàng hóa - Mua hàng giảm giá hàng";
                        break;
                    case "22":
                        item.HinhThucKM = "Hàng hóa - Mua hàng tặng hàng";
                        break;
                    case "23":
                        item.HinhThucKM = "Hàng hóa - Mua hàng giảm giá theo số lượng mua";
                        break;
                    case "24":
                        item.HinhThucKM = "Hàng hóa - Mua hàng tặng điểm";
                        break;
                    default:
                        item.HinhThucKM = "";
                        break;
                }
                var listKhuyenMaiCT = _khuyenMaiChiTietService.GetAll().Where(x => x.IdKhuyenMai == item.Id).ToList();
                if (listKhuyenMaiCT != null && listKhuyenMaiCT.Count > 0)
                {
                    var khuyenMaiCTs = ObjectMapper.Map<List<KhuyenMaiChiTietMap>>(listKhuyenMaiCT);
                    foreach (var kmct in khuyenMaiCTs)
                    {
                        var hangTang = await _donViQuiDoiService.GetAllIncluding(x => x.DM_HangHoa).Where(x => x.Id == kmct.IdDonViQuiDoiTang).FirstOrDefaultAsync();
                        if (hangTang != null && hangTang.DM_HangHoa != null)
                        {
                            kmct.TenHangHoaTang = hangTang.DM_HangHoa.TenHangHoa;
                        }
                        var hangMua = await _donViQuiDoiService.GetAllIncluding(x => x.DM_HangHoa).Where(x => x.Id == kmct.IdDonViQuiDoiMua).FirstOrDefaultAsync();
                        if (hangMua != null && hangMua.DM_HangHoa != null)
                        {
                            kmct.TenHangHoaMua = hangMua.DM_HangHoa.TenHangHoa;
                        }
                        var nhomHangTang = await _nhomHangHoaService.FirstOrDefaultAsync(x => x.Id == kmct.IdNhomHangTang);
                        if (nhomHangTang != null)
                        {
                            kmct.TenHangHoaTang = nhomHangTang.TenNhomHang;
                        }
                        var nhomHangMua = await _nhomHangHoaService.FirstOrDefaultAsync(x => x.Id == kmct.IdNhomHangMua);
                        if (nhomHangMua != null)
                        {
                            kmct.TenNhomHangMua = nhomHangMua.TenNhomHang;
                        }
                    }
                    item.KhuyenMaiChiTiets = khuyenMaiCTs;
                }

            }
            data = data.Where(x => x.HinhThucKM.ToLower().Contains(input.Keyword.ToLower())).ToList();
            result.Items = ObjectMapper.Map<List<KhuyenMaiDto>>(data);
            return result;
        }
    }
}
