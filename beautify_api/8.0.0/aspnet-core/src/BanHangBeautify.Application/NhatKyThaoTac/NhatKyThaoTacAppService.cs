using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Entities;
using BanHangBeautify.NhatKyHoatDong.Dto;
using BanHangBeautify.NhatKyThaoTac.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.NhatKyHoatDong
{
    public class NhatKyThaoTacAppService : SPAAppServiceBase, INhatKyThaoTacAppService
    {
        private readonly IRepository<HT_NhatKyThaoTac, Guid> _repository;
        private readonly IRepository<Authorization.Users.User, long> _userRepository;
        public NhatKyThaoTacAppService(
            IRepository<HT_NhatKyThaoTac, Guid> repository,
            IRepository<User, long> userRepository
        )
        {
            _repository = repository;
            _userRepository = userRepository;
        }
       
        public async Task<NhatKyThaoTacDto> CreateNhatKyHoatDong(CreateNhatKyThaoTacDto input)
        {
            HT_NhatKyThaoTac data = new HT_NhatKyThaoTac();
            NhatKyThaoTacDto result = new NhatKyThaoTacDto();
            data = ObjectMapper.Map<HT_NhatKyThaoTac>(input);
            data.Id = Guid.NewGuid();
            data.IdChiNhanh = input.IdChiNhanh;
            data.TenantId = AbpSession.TenantId??1;
            data.CreatorUserId = AbpSession.UserId;
            data.CreationTime = DateTime.Now;
            data.IsDeleted = false;
            await _repository.InsertAsync(data);
            result = ObjectMapper.Map<NhatKyThaoTacDto>(input);
            return result;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhatKyThaoTac_Delete)]
        public async Task<NhatKyThaoTacDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            data.IsDeleted = true;
            data.DeletionTime = DateTime.Now;
            await _repository.UpdateAsync(data);
            return ObjectMapper.Map<NhatKyThaoTacDto>(data);
        }
        [AbpAuthorize(PermissionNames.Pages_NhatKyThaoTac)]
        [HttpPost]
        public async Task<PagedResultDto<NhatKyThaoTacItemDto>> GetAll(PagedNhatKyRequestDto input)
        {
            PagedResultDto<NhatKyThaoTacItemDto> result = new PagedResultDto<NhatKyThaoTacItemDto>();
            input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            DateTime timeFrom = new DateTime(input.TimeFrom.Year,input.TimeFrom.Month,input.TimeFrom.Day,0,0,1);
            DateTime timeTo = new DateTime(input.TimeTo.Year, input.TimeTo.Month, input.TimeTo.Day, 23, 59, 59, 59);
            var data = await _repository.GetAllIncluding().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 0) && x.CreationTime>=timeFrom&&x.CreationTime<=timeTo).ToListAsync();
            data = data.Where(x=>x.NoiDung.Contains(input.Keyword)||x.ChucNang.Contains(input.Keyword)||x.NoiDungChiTiet.Contains(input.Keyword)).ToList();
            if(input.LoaiNhatKys!=null && input.LoaiNhatKys.Count > 0)
            {
                data = data.Where(x => input.LoaiNhatKys.Contains(x.LoaiNhatKy)).ToList();
            }
            result.TotalCount = data.Count;
            data = data.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<NhatKyThaoTacItemDto>>(data);
            foreach (var item in result.Items)
            {
                var nhanSuId = await _userRepository.FirstOrDefaultAsync(x => x.Id == (AbpSession.UserId ?? 1));
                item.TenNguoiThaoTac = nhanSuId.FullName;
            }
            return result;
        }
        [AbpAuthorize(PermissionNames.Pages_NhatKyThaoTac)]
        public async Task<NhatKyThaoTacItemDto> GetDetail(Guid id)
        {
            NhatKyThaoTacItemDto result = new NhatKyThaoTacItemDto();
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                result.LoaiNhatKy = data.LoaiNhatKy;
                result.NoiDung = data.NoiDung;
                result.NoiDungChiTiet = data.NoiDungChiTiet;
                result.ChucNang = data.ChucNang;
                var nhanSuId = await _userRepository.FirstOrDefaultAsync(x => x.Id == (AbpSession.UserId ?? 1));
                result.TenNguoiThaoTac = nhanSuId.FullName;
            }
            return result;
        }
    }
}
