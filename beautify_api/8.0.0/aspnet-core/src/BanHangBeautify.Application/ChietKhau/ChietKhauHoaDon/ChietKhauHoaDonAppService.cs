using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Dto;
using BanHangBeautify.ChietKhau.ChietKhauHoaDon.Repository;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.ChietKhau.ChietKhauHoaDon
{
    [AbpAuthorize(PermissionNames.Pages_ChietKhauHoaDon)]
    public class ChietKhauHoaDonAppService : SPAAppServiceBase
    {
        private readonly IRepository<NS_ChietKhauHoaDon, Guid> _repository;
        private readonly IRepository<NS_ChietKhauHoaDon_ChiTiet, Guid> _chietKhauHoaDonCTService;
        private readonly IChietKhauHoaDonRepository _chietKhauHoaDonRepository;
        public ChietKhauHoaDonAppService(
            IRepository<NS_ChietKhauHoaDon, Guid> repository,
            IChietKhauHoaDonRepository chietKhauHoaDonRepository,
            IRepository<NS_ChietKhauHoaDon_ChiTiet, Guid> chietKhauHoaDonCTService)
        {
            _repository = repository;
            _chietKhauHoaDonRepository = chietKhauHoaDonRepository;
            _chietKhauHoaDonCTService = chietKhauHoaDonCTService;
        }
        [AbpAuthorize(PermissionNames.Pages_ChietKhauHoaDon_Create, PermissionNames.Pages_ChietKhauHoaDon_Edit)]
        public async Task<ExecuteResultDto> CreateOrEdit(CreateOrEditChietKhauHDDto input)
        {
            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist == null)
            {
                return await Create(input);
            }
            return await Update(input, checkExist);
        }
        [NonAction]
        public async Task<ExecuteResultDto> Create(CreateOrEditChietKhauHDDto input)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                NS_ChietKhauHoaDon data = new NS_ChietKhauHoaDon();
                data = ObjectMapper.Map<NS_ChietKhauHoaDon>(input);
                data.Id = Guid.NewGuid();
                data.ChungTuApDung = input.ChungTuApDung;
                data.CreationTime = DateTime.Now;
                data.GhiChu = input.GhiChu;
                data.CreatorUserId = AbpSession.UserId;
                data.TenantId = AbpSession.TenantId ?? 1;
                data.IsDeleted = false;
                await _repository.InsertAsync(data);
                if (input.IdNhanViens != null && input.IdNhanViens.Count > 0)
                {
                    foreach (var item in input.IdNhanViens.Distinct())
                    {
                        NS_ChietKhauHoaDon_ChiTiet ckhd_ct = new NS_ChietKhauHoaDon_ChiTiet();
                        ckhd_ct.Id = Guid.NewGuid();
                        ckhd_ct.IdNhanVien = item;
                        ckhd_ct.IdChietKhauHD = data.Id;
                        ckhd_ct.TenantId = AbpSession.TenantId ?? 1;
                        ckhd_ct.CreationTime = DateTime.Now;
                        ckhd_ct.CreatorUserId = AbpSession.UserId;
                        ckhd_ct.IsDeleted = false;
                        await _chietKhauHoaDonCTService.InsertAsync(ckhd_ct);
                    }

                }
                result.Message = "Thêm mới thành công!";
                result.Status = "success";
            }
            catch (Exception)
            {
                result.Message = "Thêm mới thất bại!";
                result.Status = "error";
            }

            return result;
        }
        [NonAction]
        public async Task<ExecuteResultDto> Update(CreateOrEditChietKhauHDDto input, NS_ChietKhauHoaDon oldData)
        {
            ExecuteResultDto result = new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi xảy ra vui lòng thử lại sau"
            };
            try
            {
                oldData.IdChiNhanh = input.IdChiNhanh;
                oldData.LoaiChietKhau = input.LoaiChietKhau;
                oldData.GiaTriChietKhau = input.GiaTriChietKhau;
                oldData.ChungTuApDung = input.ChungTuApDung;
                oldData.TrangThai = 0;
                oldData.GhiChu = input.GhiChu;
                oldData.LastModificationTime = DateTime.Now;
                oldData.LastModifierUserId = AbpSession.UserId;
                await _repository.UpdateAsync(oldData);
                var checkChietkhauHdCt = _chietKhauHoaDonCTService.GetAll().Where(x => x.IdChietKhauHD == oldData.Id && x.IsDeleted == false).ToList();
                if (checkChietkhauHdCt != null && checkChietkhauHdCt.Count > 0)
                {
                    if (input.IdNhanViens.Distinct().Count() > 0)
                    {
                        foreach (var item in input.IdNhanViens.Distinct())
                        {
                            if (!checkChietkhauHdCt.Select(x => x.IdNhanVien).ToList().Contains(item))
                            {
                                NS_ChietKhauHoaDon_ChiTiet ckhd_ct = new NS_ChietKhauHoaDon_ChiTiet();
                                ckhd_ct.Id = Guid.NewGuid();
                                ckhd_ct.IdNhanVien = item;
                                ckhd_ct.IdChietKhauHD = oldData.Id;
                                ckhd_ct.TenantId = AbpSession.TenantId ?? 1;
                                ckhd_ct.CreationTime = DateTime.Now;
                                ckhd_ct.CreatorUserId = AbpSession.UserId;
                                await _chietKhauHoaDonCTService.InsertAsync(ckhd_ct);
                            }
                            else
                            {
                                var ckhd_ct = checkChietkhauHdCt.Where(x => x.IdNhanVien == item).FirstOrDefault();
                                if (ckhd_ct.IsDeleted == true)
                                {
                                    ckhd_ct.IsDeleted = false;
                                    ckhd_ct.DeleterUserId = null;
                                    ckhd_ct.DeletionTime = null;
                                    await _chietKhauHoaDonCTService.UpdateAsync(ckhd_ct);
                                }
                                // nếu k còn trong input.IdNhanViens thì xóa
                                var itemRemove = _chietKhauHoaDonCTService.GetAll().Where(x => !input.IdNhanViens.Contains(x.IdNhanVien)).ToList();
                                if (itemRemove != null && itemRemove.Count > 0)
                                {
                                    _chietKhauHoaDonCTService.RemoveRange(itemRemove);
                                    //foreach (var remove in itemRemove)
                                    //{
                                    //    remove.IsDeleted = true;
                                    //    remove.DeleterUserId = AbpSession.UserId;
                                    //    remove.DeletionTime = DateTime.Now;
                                    //    await _chietKhauHoaDonCTService.UpdateAsync(remove);
                                    //}
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in checkChietkhauHdCt)
                        {
                            item.IsDeleted = true;
                            item.DeleterUserId = AbpSession.UserId;
                            item.DeletionTime = DateTime.Now;
                            await _chietKhauHoaDonCTService.UpdateAsync(item);
                        }
                    }

                }
                else
                {
                    if (input.IdNhanViens.Distinct().Count() > 0)
                    {
                        foreach (var item in input.IdNhanViens.Distinct())
                        {
                            NS_ChietKhauHoaDon_ChiTiet ckhd_ct = new NS_ChietKhauHoaDon_ChiTiet();
                            ckhd_ct.Id = Guid.NewGuid();
                            ckhd_ct.IdNhanVien = item;
                            ckhd_ct.IdChietKhauHD = oldData.Id;
                            ckhd_ct.TenantId = AbpSession.TenantId ?? 1;
                            ckhd_ct.CreationTime = DateTime.Now;
                            ckhd_ct.CreatorUserId = AbpSession.UserId;
                            await _chietKhauHoaDonCTService.InsertAsync(ckhd_ct);
                        }
                    }
                }
                result.Status = "success";
                result.Message = "Cập nhật thành công!";
            }
            catch (Exception)
            {
                result.Status = "error";
                result.Message = "Có lỗi xảy ra vui lòng thử lại sau";
            }

            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_ChietKhauHoaDon_Delete)]
        public async Task<ChietKhauHoaDonDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                await _repository.DeleteAsync(data);
                return ObjectMapper.Map<ChietKhauHoaDonDto>(data);
            }
            return new ChietKhauHoaDonDto();
        }
        public async Task<ExecuteResultDto> DeleteMany(List<Guid> ids)
        {
            ExecuteResultDto result = new ExecuteResultDto()
            {
                Status = "error",
                Message = "Có lỗi xảy ra vui lòng thử lại sau!"
            };
            {
                var finds = await _repository.GetAll().Where(x => ids.Contains(x.Id)).ToListAsync();
                if (finds != null && finds.Count > 0)
                {
                    _repository.RemoveRange(finds);
                    result.Status = "success";
                    result.Message = string.Format("Xóa {0} bản ghi thành công!", ids.Count);
                }
                return result;
            }
        }
        public async Task<CreateOrEditChietKhauHDDto> GetForEdit(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                var result = ObjectMapper.Map<CreateOrEditChietKhauHDDto>(data);
                var idNhaniens = _chietKhauHoaDonCTService.GetAll().Where(x => x.IdChietKhauHD == data.Id && x.IsDeleted == false).Select(x => x.IdNhanVien).ToList();
                result.IdNhanViens = idNhaniens;
                return result;

            }
            return new CreateOrEditChietKhauHDDto();
        }
        public async Task<PagedResultDto<ChietKhauHoaDonItemDto>> GetAll(PagedRequestDto input, Guid? idChiNhanh)
        {
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            return await _chietKhauHoaDonRepository.GetAll(input, AbpSession.TenantId ?? 1, idChiNhanh);
        }
        [HttpGet]
        public async Task<List<ChietKhauHoaDonItemDto>> GetHoaHongNV_theoLoaiChungTu(Guid idChiNhanh, Guid idNhanVien, string loaiChungTu = "1")
        {
            return await _chietKhauHoaDonRepository.GetHoaHongNV_theoHoaDon(idChiNhanh, idNhanVien, loaiChungTu);
        }
    }
}
