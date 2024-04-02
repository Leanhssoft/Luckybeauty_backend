using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.AppCommon;
using BanHangBeautify.AppDanhMuc.AppCuaHang.Dto;
using BanHangBeautify.Checkin.Dto;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BanHangBeautify.Zalo.ZaloTemplate
{
    public class Zalo_TemplateAppService : SPAAppServiceBase
    {
        private readonly IRepository<Zalo_Template, Guid> _zaloTemplate;
        private readonly IRepository<Zalo_Element, Guid> _zaloElement;
        private readonly IRepository<Zalo_TableDetail, Guid> _zaloTable;
        private readonly IRepository<Zalo_ButtonDetail, Guid> _zaloButton;
        private readonly IZalo_TemplateRepository _zaloTemplateRepo;

        public Zalo_TemplateAppService(IRepository<Zalo_Template, Guid> zaloTemplate, IZalo_TemplateRepository zaloTemplateRepo,
            IRepository<Zalo_ButtonDetail, Guid> zaloButton,
            IRepository<Zalo_Element, Guid> zaloElement,
            IRepository<Zalo_TableDetail, Guid> zaloTable)
        {
            _zaloTemplate = zaloTemplate;
            _zaloTemplateRepo = zaloTemplateRepo;
            _zaloButton = zaloButton;
            _zaloElement = zaloElement;
            _zaloTable = zaloTable;
        }

        [HttpGet]
        public List<Zalo_TemplateDto> InnitData_TempZalo()
        {
            var data = _zaloTemplateRepo.InnitData_TempZalo();
            return data;
        }

        public async Task<Zalo_TemplateDto> GetZaloTemplate_byId(Guid id)
        {
            var objFind = await _zaloTemplate.GetAsync(id);
            if (objFind != null)
            {
                var zaloTemp = ObjectMapper.Map<Zalo_TemplateDto>(objFind);
                var lstBtn = _zaloButton.GetAllList().Where(x => x.IdTemplate == id);

                var lstElm = _zaloElement.GetAllList().Where(x => x.IdTemplate == id);
                var lstElm_withTbl = ObjectMapper.Map<List<Zalo_ElementDto>>(lstElm).Select(x => new Zalo_ElementDto
                {
                    Id = x.Id,
                    IdTemplate = x.IdTemplate,
                    ElementType = x.ElementType,
                    Content = x.Content,
                    ThuTuSapXep = x.ThuTuSapXep,
                    tables = ObjectMapper.Map<List<Zalo_TableDetailDto>>(_zaloTable.GetAllList().Where(o => o.IdElement == x.Id)),
                }).OrderBy(x => x.ThuTuSapXep);

                zaloTemp.buttons = ObjectMapper.Map<List<Zalo_ButtonDetailDto>>(lstBtn);
                zaloTemp.elements = ObjectMapper.Map<List<Zalo_ElementDto>>(lstElm_withTbl);
                return zaloTemp;
            }
            return null;
        }
        public async Task<Zalo_TemplateDto> GetZaloTemplate_Default(byte idLoaiTin)
        {
            return await _zaloTemplateRepo.FindTempDefault_ByIdLoaiTin(idLoaiTin);
        }

        public async Task AddListElement(Guid idTemp, List<Zalo_ElementDto> lstElm)
        {
            if (lstElm != null)
            {
                foreach (var item in lstElm)
                {
                    Guid idElm = Guid.NewGuid();
                    Zalo_Element ele = ObjectMapper.Map<Zalo_Element>(item);
                    ele.Id = idElm;
                    ele.IdTemplate = idTemp;
                    ele.CreatorUserId = AbpSession.UserId ?? 1;
                    ele.CreationTime = DateTime.Now;
                    await _zaloElement.InsertAsync(ele);

                    if (item?.tables != null)
                    {
                        List<Zalo_TableDetail> lstTbl = new();
                        foreach (var tbl in item?.tables)
                        {
                            Zalo_TableDetail tblNew = ObjectMapper.Map<Zalo_TableDetail>(tbl);
                            tblNew.Id = Guid.NewGuid();
                            tblNew.IdElement = idElm;
                            tblNew.CreatorUserId = AbpSession.UserId ?? 1;
                            tblNew.CreationTime = DateTime.Now;
                            lstTbl.Add(tblNew);
                        }
                        await _zaloTable.InsertRangeAsync(lstTbl);
                    }
                }
            }
        }
        public async Task AddListButton(Guid idTemp, List<Zalo_ButtonDetailDto> lstButton)
        {
            if (lstButton != null)
            {
                List<Zalo_ButtonDetail> lstBtn = new();
                foreach (var tbl in lstButton)
                {
                    Zalo_ButtonDetail tblNew = ObjectMapper.Map<Zalo_ButtonDetail>(tbl);
                    tblNew.Id = Guid.NewGuid();
                    tblNew.IdTemplate = idTemp;
                    tblNew.CreatorUserId = AbpSession.UserId ?? 1;
                    tblNew.CreationTime = DateTime.Now;
                    lstBtn.Add(tblNew);
                }
                await _zaloButton.InsertRangeAsync(lstBtn);
            }
        }
        public async Task<bool> RemoveBtn_andElm(Guid idTemp)
        {
            var lstBtn = _zaloButton.GetAllList().Where(x => x.IdTemplate == idTemp);
            foreach (var item in lstBtn)
            {
                await _zaloButton.HardDeleteAsync(item);
            }

            var lstElm = _zaloElement.GetAllList().Where(x => x.IdTemplate == idTemp);
            foreach (var item in lstElm)
            {
                switch (item.ElementType)
                {
                    case ZaloElementType.TABLE:
                        {
                            var tblDetails = _zaloTable.GetAllList().Where(x => x.IdElement == item.Id);
                            foreach (var tbl in tblDetails)
                            {
                                await _zaloTable.HardDeleteAsync(tbl);
                            }
                            await _zaloElement.HardDeleteAsync(item);
                        }
                        break;
                    default:
                        await _zaloElement.HardDeleteAsync(item);
                        break;
                }
            }
            return true;
        }

        [HttpPost]
        public async Task<Zalo_TemplateDto> InsertMauTinZalo(Zalo_TemplateDto dto)
        {
            if (dto == null) { return new Zalo_TemplateDto(); };
            Guid idTemp = Guid.NewGuid();
            Zalo_Template objNew = ObjectMapper.Map<Zalo_Template>(dto);
            objNew.Id = idTemp;
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.CreationTime = DateTime.Now;
            await _zaloTemplate.InsertAsync(objNew);

            await AddListElement(idTemp, dto?.elements);
            await AddListButton(idTemp, dto?.buttons);

            var result = ObjectMapper.Map<Zalo_TemplateDto>(objNew);
            return result;
        }

        [HttpPost]
        public async Task<Zalo_TemplateDto> UpdateMauTinZalo(Zalo_TemplateDto dto)
        {
            if (dto == null) { return null; };
            Zalo_Template objUp = await _zaloTemplate.FirstOrDefaultAsync(dto.Id);
            if (objUp == null)
            {
                return null;
            }
            objUp.IdLoaiTin = dto.IdLoaiTin;
            objUp.TenMauTin = dto.TenMauTin;
            objUp.IsDefault = dto.IsDefault;
            objUp.TemplateType = dto.TemplateType;
            objUp.LastModifierUserId = AbpSession.UserId;
            objUp.LastModificationTime = DateTime.Now;
            await _zaloTemplate.UpdateAsync(objUp);

            // remove & add again
            await RemoveBtn_andElm(dto.Id);
            await AddListElement(dto.Id, dto?.elements);
            await AddListButton(dto.Id, dto?.buttons);

            var result = ObjectMapper.Map<Zalo_TemplateDto>(objUp);
            return result;
        }
    }
}
