﻿using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.DataExporting.Excel.EpPlus;
using BanHangBeautify.Entities;
using BanHangBeautify.HangHoa.HangHoa.Repository;
using BanHangBeautify.HangHoa.NhomHangHoa.Dto;
using BanHangBeautify.SMS.Brandname.Repository;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.Storage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.Brandname
{
    public class BrandnameAppService : SPAAppServiceBase
    {
        private readonly IRepository<HT_SMSBrandname, Guid> _dmBrandname;
        private readonly IBrandnameRepository _repository;
        private readonly IExcelBase _excelBase;

        public BrandnameAppService(IRepository<HT_SMSBrandname, Guid> dmBrandname, IBrandnameRepository repository,
            IExcelBase excelBase)
        {
            _dmBrandname = dmBrandname;
            _repository = repository;
            _excelBase = excelBase;
        }

        [HttpGet]
        public async Task<bool> Brandname_CheckExistSDT(string phoneNumber, Guid? id = null)
        {
            if (id != null && id != Guid.Empty)
            {
                var lst = await _dmBrandname.GetAllListAsync(x => x.Id != id && x.SDTCuaHang == phoneNumber.Trim());
                if (lst.Count > 0)
                {
                    return true;
                }
            }
            else
            {
                var lst = await _dmBrandname.GetAllListAsync(x => x.SDTCuaHang == phoneNumber.Trim());
                if (lst.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        [HttpGet]
        public async Task<PageBrandnameDto> GetInforBrandnamebyID(Guid id)
        {
            var data = await _repository.GetInforBrandname_byId(id);
            return data;
        }
        [HttpPost]
        public async Task<PagedResultDto<PageBrandnameDto>> GetListBandname(PagedRequestDto param)
        {
            var data = await _repository.GetListBandname(param, AbpSession.TenantId ?? 1);
            return data;
        }
        [HttpPost]
        public BrandnameDto CreateBrandname(BrandnameDto dto)
        {
            if (dto == null) { return new BrandnameDto(); };
            HT_SMSBrandname objNew = ObjectMapper.Map<HT_SMSBrandname>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.CreationTime = DateTime.Now;
            _dmBrandname.InsertAsync(objNew);
            var result = ObjectMapper.Map<BrandnameDto>(objNew);
            return result;
        }
        [HttpPost]
        public async Task<string> UpdateBrandname(BrandnameDto dto)
        {
            try
            {
                if (dto == null) { return "Data null"; };
                HT_SMSBrandname objUp = await _dmBrandname.FirstOrDefaultAsync(dto.Id);
                if (objUp == null)
                {
                    return "object null";
                }
                objUp.Brandname = dto.Brandname;
                objUp.SDTCuaHang = dto.SDTCuaHang;
                objUp.NgayKichHoat = dto.NgayKichHoat;
                objUp.TrangThai = dto.TrangThai;
                objUp.LastModifierUserId = AbpSession.UserId;
                objUp.LastModificationTime = DateTime.Now;
                await _dmBrandname.UpdateAsync(objUp);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Concat(ex.InnerException + ex.Message);
            }
        }
        [HttpGet]
        public async Task<string> XoaBrandname(Guid id)
        {
            try
            {
                HT_SMSBrandname objUp = await _dmBrandname.FirstOrDefaultAsync(id);
                if (objUp == null)
                {
                    return "object null";
                }
                objUp.TrangThai = 0;
                objUp.IsDeleted = true;
                objUp.DeletionTime = DateTime.Now;
                objUp.DeleterUserId = AbpSession.UserId;
                await _dmBrandname.UpdateAsync(objUp);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Concat(ex.InnerException + ex.Message);
            }
        }
        [HttpPost]
        public async Task DeleteMultiple_Brandname(List<Guid> lstIdBrandname)
        {
            _dmBrandname.GetAllList(x => lstIdBrandname.Contains(x.Id)).ToList().ForEach(x =>
            {
                x.TrangThai = 0;
                x.IsDeleted = true;
                x.DeletionTime = DateTime.Now;
                x.DeleterUserId = AbpSession.UserId;
            });
        }
        [HttpPost]
        public async Task ActiveMultiple_Brandname(List<Guid> lstIdBrandname)
        {
            _dmBrandname.GetAllList(x => lstIdBrandname.Contains(x.Id)).ToList().ForEach(x =>
            {
                x.TrangThai = 1;
                x.IsDeleted = false;
                x.LastModificationTime = DateTime.Now;
                x.LastModifierUserId = AbpSession.UserId;
            });
        }

        [HttpPost]
        public async Task<FileDto> ExportToExcel_ListBrandname(PagedRequestDto input)
        {
            var data = await _repository.GetListBandname(input, AbpSession.TenantId ?? 1);
            var dataExcel = ObjectMapper.Map<List<PageBrandnameDto>>(data.Items);
            var dataNew = dataExcel.Select(x => new
            {
                x.Brandname,
                x.SDTCuaHang,
                x.NgayKichHoat,
                x.TongTienNap,
                x.DaSuDung,
                x.ConLai
            }).ToList();
            return _excelBase.WriteToExcel("DanhSachBrandname_", "Brandname_Export_Template.xlsx", dataNew, 5);
        }
    }
}