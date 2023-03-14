using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BanHangBeautify.Entities;
using BanHangBeautify.NhanSu.NhanVien_DichVu.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu
{
    public class NhanVienDichVuAppService : AsyncCrudAppService<DichVu_NhanVien, DichVuNhanVienDto, Guid, PagedDichVuNhanVienResultRequestDto, CreateOrUpdateDichVuNhanVienDto, DichVuNhanVienDto>
    {
        IRepository<DichVu_NhanVien, Guid> _repository;
        public NhanVienDichVuAppService(IRepository<DichVu_NhanVien, Guid> repository) : base(repository)
        {
            _repository = repository;
        }
        [HttpPost]
        public override Task<DichVuNhanVienDto> CreateAsync(CreateOrUpdateDichVuNhanVienDto input)
        {
            return base.CreateAsync(input);
        }
        [HttpDelete]
        public override Task DeleteAsync(EntityDto<Guid> input)
        {
            return base.DeleteAsync(input);
        }
        [HttpPost]
        public async override Task<PagedResultDto<DichVuNhanVienDto>> GetAllAsync(PagedDichVuNhanVienResultRequestDto input)
        {
            PagedResultDto<DichVuNhanVienDto> result = new PagedResultDto<DichVuNhanVienDto>();
            result = await base.GetAllAsync(input);
            return result;
        }
        [HttpPost]
        public override Task<DichVuNhanVienDto> GetAsync(EntityDto<Guid> input)
        {
            return base.GetAsync(input);
        }
        [HttpPost]
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
