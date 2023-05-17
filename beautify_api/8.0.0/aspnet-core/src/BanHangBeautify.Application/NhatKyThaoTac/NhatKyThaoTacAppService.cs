using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using BanHangBeautify.NhatKyHoatDong.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhatKyHoatDong
{
    [AbpAuthorize(PermissionNames.Pages_NhatKyThaoTac)]
    public class NhatKyThaoTacAppService : SPAAppServiceBase, INhatKyThaoTacAppService
    {
        private readonly IRepository<HT_NhatKyThaoTac, Guid> _repository;
        private readonly IRepository<Authorization.Users.User, long> _userRepository;
        public NhatKyThaoTacAppService(
            IRepository<HT_NhatKyThaoTac, Guid> repository,
            IRepository<User,long> userRepository
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
            data.CreatorUserId = AbpSession.UserId;
            data.CreationTime = DateTime.Now;
            data.IsDeleted = false;
            await _repository.InsertAsync(data);
            result = ObjectMapper.Map<NhatKyThaoTacDto>(input);
            return result;
        }
        [HttpPost]
        public async Task<NhatKyThaoTacDto> Delete(Guid id)
        {
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            data.IsDeleted = true;
            data.DeletionTime= DateTime.Now;
            await _repository.UpdateAsync(data);
            return ObjectMapper.Map<NhatKyThaoTacDto>(data);
        }

        public async Task<PagedResultDto<NhatKyThaoTacItemDto>> GetAll(PagedRequestDto input)
        {
            PagedResultDto<NhatKyThaoTacItemDto> result = new PagedResultDto<NhatKyThaoTacItemDto>();
            input.Keyword= string.IsNullOrEmpty(input.Keyword)?"":input.Keyword;
            input.SkipCount = input.SkipCount > 0 ? input.SkipCount * input.MaxResultCount : 0;
            var data = await _repository.GetAll().Where(x => x.IsDeleted == false && x.TenantId == (AbpSession.TenantId ?? 0)).ToListAsync();
            result.TotalCount = data.Count;
            data = data.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.Items = ObjectMapper.Map<List<NhatKyThaoTacItemDto>>(data);
            foreach (var item in result.Items)
            {
                var nhanSuId = await _userRepository.FirstOrDefaultAsync(x=>x.Id==(AbpSession.UserId??1));
                item.TenNguoiThaoTac = nhanSuId.FullName;
            }
            return result;
        }

        public async Task<NhatKyThaoTacItemDto> GetDetail(Guid id)
        {
            NhatKyThaoTacItemDto result = new NhatKyThaoTacItemDto();
            var data = await _repository.FirstOrDefaultAsync(x=>x.Id==id);
            if (data!=null)
            {
                result.LoaNhatKy = data.LoaiNhatKy;
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
