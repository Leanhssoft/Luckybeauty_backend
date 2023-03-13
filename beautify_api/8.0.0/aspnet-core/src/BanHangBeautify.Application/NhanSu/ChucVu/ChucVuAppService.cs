using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.ChucVu.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.ChucVu
{
    public class ChucVuAppService:SPAAppServiceBase
    {
        private readonly IRepository<NS_ChucVu,Guid> _repository;
        public ChucVuAppService(IRepository<NS_ChucVu, Guid> repository)
        {
            _repository= repository;
        }
        public async Task<bool> CreateOrEdit(CreateOrEditChucVuDto dto)
        {
            bool result = false;
            try
            {
                var find = await _repository.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (find == null)
                {
                    await Create(dto);
                    result= true;
                }
                else
                {
                    await Edit(dto,find);
                    result= true;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
            
            
        }
        [NonAction]
        public async Task Create(CreateOrEditChucVuDto dto)
        {
            NS_ChucVu chucVu = new NS_ChucVu();
            chucVu.Id = Guid.NewGuid();
            chucVu.MaChucVu = dto.MaChucVu;
            chucVu.TenChucVu = dto.TenChucVu;
            chucVu.TrangThai = 0;
            chucVu.MoTa = dto.MoTa;
            chucVu.TenantId = AbpSession.TenantId??1;
            chucVu.CreatorUserId = AbpSession.UserId;
            chucVu.NgayTao = DateTime.Now;
            await _repository.InsertAsync(chucVu);
        }
        [NonAction]
        public async Task Edit(CreateOrEditChucVuDto dto,NS_ChucVu chucVu)
        {
            chucVu.MaChucVu = dto.MaChucVu;
            chucVu.TenChucVu = dto.TenChucVu;
            chucVu.TrangThai = dto.TrangThai;
            chucVu.MoTa = dto.MoTa;
            chucVu.NgaySua = DateTime.Now;
            chucVu.LastModifierUserId = AbpSession.UserId;
            chucVu.LastModificationTime = DateTime.Now;
            await _repository.InsertAsync(chucVu);
        }
        public async Task<bool> Delete(Guid Id)
        {
            var find =await _repository.FirstOrDefaultAsync(x => x.Id == Id);
            if (find != null)
            {
                find.IsDeleted = true;
                find.DeleterUserId = AbpSession.UserId;
                find.DeletionTime = DateTime.Now;
                find.TrangThai = 1;
                return true;
            }
            return false;
        }
        public async Task<ListResultDto<NS_ChucVu>> GetAll()
        {
            ListResultDto<NS_ChucVu> result= new ListResultDto<NS_ChucVu>();
            try
            {
                var lstChucVu = await _repository.GetAllListAsync();
                lstChucVu = lstChucVu.Where(x => x.IsDeleted == false).ToList();
                //var chucVus = ObjectMapper.Map<ChucVuDto>(lstChucVu);
                result.Items = lstChucVu;
            }
            catch (Exception)
            {
result.Items = null;
            }
           
            return result;
        }
    }
}
