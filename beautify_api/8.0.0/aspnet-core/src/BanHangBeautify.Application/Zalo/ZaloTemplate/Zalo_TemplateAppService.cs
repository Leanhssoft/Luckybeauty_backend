using Abp.Domain.Repositories;
using BanHangBeautify.AppDanhMuc.AppCuaHang.Dto;
using BanHangBeautify.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.ZaloTemplate
{
    public class Zalo_TemplateAppService : SPAAppServiceBase
    {
        private readonly IRepository<Zalo_Template, Guid> _zaloTemplate;
        private readonly IZalo_TemplateRepository _zaloTemplateRepo;

        public Zalo_TemplateAppService(IRepository<Zalo_Template, Guid> zaloTemplate, IZalo_TemplateRepository zaloTemplateRepo)
        {
            _zaloTemplate = zaloTemplate;
            _zaloTemplateRepo = zaloTemplateRepo;
        }

        public async Task<Zalo_TemplateDto> GetZaloTemplate_byId(Guid id)
        {
            var objFind = await _zaloTemplate.GetAsync(id);
            if (objFind != null)
            {
                return ObjectMapper.Map<Zalo_TemplateDto>(objFind);
            }
            return null;
        }
        public async Task<Zalo_TemplateDto> GetZaloTemplate_Default(byte idLoaiTin)
        {
            return await _zaloTemplateRepo.FindTempDefault_ByIdLoaiTin(idLoaiTin);
        }
    }
}
