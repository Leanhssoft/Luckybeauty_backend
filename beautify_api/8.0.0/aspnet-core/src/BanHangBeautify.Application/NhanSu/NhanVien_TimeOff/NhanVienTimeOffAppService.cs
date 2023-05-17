using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.NhanVien_TimeOff.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BanHangBeautify.NhanSu.NhanVien_TimeOff
{
    [AbpAuthorize(PermissionNames.Pages_NhanSu_TimeOff)]
    public class NhanVienTimeOffAppService:SPAAppServiceBase
    {
        private readonly IRepository<NS_NhanVien_TimeOff,Guid> _nhanVienTimeOffRepository;
        public NhanVienTimeOffAppService(IRepository<NS_NhanVien_TimeOff, Guid> nhanVienTimeOffRepository)
        {
            _nhanVienTimeOffRepository = nhanVienTimeOffRepository;
        }
        public async Task<NhanVienTimeOffDto> CreateOrEdit(CreateOrEditNhanVienTimeOffDto input)
        {
            var check =await _nhanVienTimeOffRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (check == null)
            {
                return await Create(input);
            }
            return await Update(input, check);
        }
        [NonAction]
        public async Task<NhanVienTimeOffDto> Create(CreateOrEditNhanVienTimeOffDto input)
        {
            NhanVienTimeOffDto result = new NhanVienTimeOffDto();
            NS_NhanVien_TimeOff data = new NS_NhanVien_TimeOff();
            data = ObjectMapper.Map<NS_NhanVien_TimeOff>(input);
            var calculator = data.DenNgay.Subtract(data.TuNgay);
            var roundTimeOff = Math.Round(calculator.TotalDays, 1);
            data.TongNgayNghi = roundTimeOff;
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId ?? 0;
            data.IsDeleted= false;
            await _nhanVienTimeOffRepository.InsertAsync(data);
            result = ObjectMapper.Map<NhanVienTimeOffDto>(data);
            return result;
        }
        [NonAction]
        public async Task<NhanVienTimeOffDto> Update(CreateOrEditNhanVienTimeOffDto input,NS_NhanVien_TimeOff ollData)
        {
            var calculator = input.DenNgay.Subtract(input.TuNgay);
            var roundTimeOff = Math.Round(calculator.TotalDays, 1);
            ollData.TuNgay = input.TuNgay;
            ollData.DenNgay = input.DenNgay;
            ollData.LoaiNghi = input.LoaiNghi;
            ollData.TongNgayNghi = roundTimeOff;
            ollData.LastModificationTime= DateTime.Now;
            ollData.LastModifierUserId = AbpSession.UserId;
            await _nhanVienTimeOffRepository.UpdateAsync(ollData);
            var result = ObjectMapper.Map<NhanVienTimeOffDto>(ollData);
            return result;
        }
        [HttpPost]
        public async Task<NhanVienTimeOffDto> Delete(Guid id)
        {
            var check = await _nhanVienTimeOffRepository.FirstOrDefaultAsync(x => x.Id == id);
            check.IsDeleted = false;
            check.DeletionTime = DateTime.Now;
            _nhanVienTimeOffRepository.Update(check);
            return ObjectMapper.Map<NhanVienTimeOffDto>(check);
        }
        public async Task<CreateOrEditNhanVienTimeOffDto> GetForEdit(Guid id)
        {
            CreateOrEditNhanVienTimeOffDto result = new CreateOrEditNhanVienTimeOffDto();
            var check = await _nhanVienTimeOffRepository.FirstOrDefaultAsync(x=>x.Id== id);
            if (check != null)
            {
                result = ObjectMapper.Map<CreateOrEditNhanVienTimeOffDto>(check);
            }
            return result;
        }
        public async Task<PagedResultDto<NhanVienTimeOffDto>> GetAll(PagedRequestDto input)
        {
            if (string.IsNullOrEmpty(input.Keyword))
            {
                input.Keyword = "";
            }
            input.SkipCount = input.SkipCount==0|| input.SkipCount==1?0:(input.SkipCount - 1 ) * input.MaxResultCount;
            PagedResultDto<NhanVienTimeOffDto> result = new PagedResultDto<NhanVienTimeOffDto>();
            var data =await _nhanVienTimeOffRepository.GetAll().Include(x => x.NS_NhanVien)
                .Where(x =>
                        x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false && 
                        (x.NS_NhanVien.TenNhanVien!=null && x.NS_NhanVien.TenNhanVien.Contains(input.Keyword))
                    )
                .ToListAsync();
            result.TotalCount = data.Count;
            data = data.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var lstItem = new List<NhanVienTimeOffDto>();
            foreach (var item in data)
            {
                NhanVienTimeOffDto dto = new NhanVienTimeOffDto();
                dto = ObjectMapper.Map<NhanVienTimeOffDto>(item);
                dto.TenNhanVien = item.NS_NhanVien.TenNhanVien;
                lstItem.Add(dto);
            }
            result.Items = lstItem;
            return result;
        }
    }
}
