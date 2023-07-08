using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.HoaDon.HoaDonAnh.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.HoaDon.HoaDonAnh
{
    [AbpAuthorize(PermissionNames.Pages_HoaDon_Anh)]
    public class HoaDonAnhAppService: SPAAppServiceBase
    {
        private readonly IRepository<BH_HoaDon_Anh,Guid> _hoaDonAnhRepository;
        public HoaDonAnhAppService(IRepository<BH_HoaDon_Anh, Guid> hoaDonAnhRepository)
        {
            _hoaDonAnhRepository = hoaDonAnhRepository;
        }
        [AbpAuthorize(PermissionNames.Pages_HoaDon_Anh_Create,PermissionNames.Pages_HoaDon_Anh_Update)]
        public async Task<HoaDonAnhDto> CreateOrEdit(CreateOrEditHoaDonAnhDto input)
        {
            var checkExist = await _hoaDonAnhRepository.FirstOrDefaultAsync(x=>x.Id== input.Id);
            if (checkExist != null)
            {
                return await Update(input, checkExist);
            }
            return await Create(input);
        }
        [NonAction]
        public async Task<HoaDonAnhDto> Create(CreateOrEditHoaDonAnhDto input)
        {
            HoaDonAnhDto result = new HoaDonAnhDto();
            BH_HoaDon_Anh hd = new BH_HoaDon_Anh();
            hd.Id = Guid.NewGuid();
            hd.IdHoaDon = input.IdHoaDon;
            hd.URLAnh = input.URLAnh;
            hd.CreatorUserId = AbpSession.UserId;
            hd.CreationTime = DateTime.Now;
            hd.IsDeleted = false;
            hd.TenantId = AbpSession.TenantId ?? 1;
            _hoaDonAnhRepository.Insert(hd);
            result.IdHoaDon = hd.IdHoaDon;
            result.URLAnh = hd.URLAnh;
            result.Id = hd.Id;
            return result;
        }
        [NonAction]
        public async Task<HoaDonAnhDto> Update(CreateOrEditHoaDonAnhDto input,BH_HoaDon_Anh oldData)
        {
            HoaDonAnhDto result = new HoaDonAnhDto();
            oldData.URLAnh = input.URLAnh;
            oldData.IdHoaDon = input.IdHoaDon;
            oldData.LastModificationTime = DateTime.Now;
            oldData.LastModifierUserId = AbpSession.UserId;
            _hoaDonAnhRepository.Update(oldData);
            result.IdHoaDon = oldData.IdHoaDon;
            result.URLAnh = oldData.URLAnh;
            result.Id = oldData.Id;
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_HoaDon_Anh_Delete)]
        public async Task<HoaDonAnhDto> Delete(Guid id)
        {
            HoaDonAnhDto result = new HoaDonAnhDto();
            var check =await _hoaDonAnhRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (check!=null)
            {
                check.IsDeleted = true;
                check.DeleterUserId = AbpSession.UserId;
                check.DeletionTime = DateTime.Now;
                _hoaDonAnhRepository.Update(check);
                result.Id = check.Id;
                result.URLAnh= check.URLAnh;
                result.IdHoaDon= check.IdHoaDon;
            }
            return result;
        }
        public async Task<BH_HoaDon_Anh> GetDetail(Guid id)
        {
            BH_HoaDon_Anh result = new BH_HoaDon_Anh();
            result = _hoaDonAnhRepository.FirstOrDefault(x => x.Id == id);
            return result;
        }
        public async Task<List<BH_HoaDon_Anh>> GetAll()
        {
            return await _hoaDonAnhRepository.GetAll().Where(x=>x.IsDeleted==false&& x.TenantId==(AbpSession.TenantId??0)).ToListAsync();
        }
    }
}
