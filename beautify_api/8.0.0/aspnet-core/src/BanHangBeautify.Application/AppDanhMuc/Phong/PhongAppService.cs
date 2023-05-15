using Abp.Domain.Repositories;
using BanHangBeautify.AppDanhMuc.Phong.Dto;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BanHangBeautify.AppDanhMuc.Phong
{
    public class PhongAppService:SPAAppServiceBase
    {
        private readonly IRepository<DM_Phong,Guid> _phongRepository;
        public PhongAppService(IRepository<DM_Phong, Guid> phongRepository)
        {
            _phongRepository = phongRepository;
        }
        public async Task<PhongDto> CreateOrEdit(CreateOrEditPhongDto input) {
            var checkExist = await _phongRepository.FirstOrDefaultAsync(x=> x.Id == input.Id);
            if (checkExist == null)
            {
                return await Create(input);
            }
            return await Update(input, checkExist);
        }
        [NonAction]
        public async Task<PhongDto> Create(CreateOrEditPhongDto input)
        {
            DM_Phong data = new DM_Phong();
            data = ObjectMapper.Map<DM_Phong>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.TenantId = AbpSession.TenantId??1;
            data.IsDeleted = false;
            await _phongRepository.InsertAsync(data);
            PhongDto result = new PhongDto();
            result = ObjectMapper.Map<PhongDto>(data);
            return result;
        }
        [NonAction]
        public async Task<PhongDto> Update(CreateOrEditPhongDto input,DM_Phong oldData) { 
            oldData.MaPhong = input.MaPhong;
            oldData.TenPhong = input.TenPhong;
            oldData.IdViTri = input.IdViTri;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            await _phongRepository.UpdateAsync(oldData);
            PhongDto result = new PhongDto();
            result = ObjectMapper.Map<PhongDto>(oldData);
            return result;
        }
        public async Task<PhongDto> Delete(Guid id)
        {
            var data = await _phongRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data!=null)
            {
                data.IsDeleted = true;
                data.DeleterUserId= AbpSession.UserId;
                data.DeletionTime= DateTime.Now;
                _phongRepository.Update(data);
            }
            var result = ObjectMapper.Map<PhongDto>(data);
            return result;
        }
        public async Task<CreateOrEditPhongDto> GetForEdit(Guid id)
        {
            var data = await _phongRepository.FirstOrDefaultAsync(x=>x.Id== id);
            if (data!=null)
            {
                return ObjectMapper.Map<CreateOrEditPhongDto>(data);
            }
            return new CreateOrEditPhongDto();
        }
        public async Task<PagedResultDto<PhongDto>> GetAll() { get; set; }
    }
}
