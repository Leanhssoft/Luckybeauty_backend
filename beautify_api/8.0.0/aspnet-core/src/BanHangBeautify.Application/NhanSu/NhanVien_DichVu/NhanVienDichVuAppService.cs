using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using BanHangBeautify.Authorization;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.NhanVien_DichVu.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu
{
    [AbpAuthorize(PermissionNames.Pages_NhanVien_DichVu)]
    public class NhanVienDichVuAppService : AsyncCrudAppService<DichVu_NhanVien, DichVuNhanVienDto, Guid, PagedDichVuNhanVienResultRequestDto, CreateOrUpdateDichVuNhanVienDto, DichVuNhanVienDto>
    {
        IRepository<DichVu_NhanVien, Guid> _repository;
        public NhanVienDichVuAppService(IRepository<DichVu_NhanVien, Guid> repository) : base(repository)
        {
            _repository = repository;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanVien_DichVu_Create)]
        public override Task<DichVuNhanVienDto> CreateAsync(CreateOrUpdateDichVuNhanVienDto input)
        {
            return base.CreateAsync(input);
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanVien_DichVu_Delete)]
        public override Task DeleteAsync(EntityDto<Guid> input)
        {
            return base.DeleteAsync(input);
        }
        [HttpPost]
        public async override Task<PagedResultDto<DichVuNhanVienDto>> GetAllAsync(PagedDichVuNhanVienResultRequestDto input)
        {
            PagedResultDto<DichVuNhanVienDto> result = new PagedResultDto<DichVuNhanVienDto>();
            input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
            result = await base.GetAllAsync(input);
            return result;
        }
        [HttpPost]
        public override Task<DichVuNhanVienDto> GetAsync(EntityDto<Guid> input)
        {
            return base.GetAsync(input);
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Pages_NhanVien_DichVu_Edit)]
        public override Task<DichVuNhanVienDto> UpdateAsync(DichVuNhanVienDto input)
        {
            return base.UpdateAsync(input);
        }
        [HttpGet]
        protected override Task<DichVu_NhanVien> GetEntityByIdAsync(Guid id)
        {
            return base.GetEntityByIdAsync(id);
        }
    }
}
