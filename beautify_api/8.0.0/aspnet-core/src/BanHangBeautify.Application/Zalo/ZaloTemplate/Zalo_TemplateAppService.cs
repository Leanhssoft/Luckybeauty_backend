using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.AppCommon;
using BanHangBeautify.AppDanhMuc.AppCuaHang.Dto;
using BanHangBeautify.Checkin.Dto;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.ZaloSMS_Common;
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
            IRepository<Zalo_TableDetail, Guid> zaloTable
            )
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

        public Zalo_TemplateDto GetZaloTemplate_byId(Guid id)
        {
            return _zaloTemplateRepo.GetZaloTemplate_byId(id);
        }
        public async Task<List<Zalo_TemplateDto>> GetAllZaloTemplate_fromDB()
        {
            var data = _zaloTemplate.GetAllList().Select(x => new Zalo_TemplateDto
            {
                Id = x.Id,
                TenMauTin = x.TenMauTin,
                IsDefault = x.IsDefault,
                IdLoaiTin = x.IdLoaiTin,
                TemplateType = x.TemplateType,
                Language = x.Language,
                IsSystem = false,
                elements = _zaloElement.GetAllList().Where(o => o.IdTemplate == x.Id)
                .Select(o => new Zalo_ElementDto
                {
                    Id = o.Id,
                    IdTemplate = o.IdTemplate,
                    ElementType = o.ElementType,
                    ThuTuSapXep = o.ThuTuSapXep,
                    IsImage = o.IsImage,
                    Content = o.Content,
                    tables = _zaloTable.GetAllList().Where(y => y.IdElement == o.Id).Select(y => new Zalo_TableDetailDto
                    {
                        Id = y.Id,
                        IdElement = y.IdElement,
                        Key = y.Key,
                        Value = y.Value,
                        ThuTuSapXep = y.ThuTuSapXep,
                    }).OrderBy(x => x.ThuTuSapXep).ToList()
                }).OrderBy(x => x.ThuTuSapXep).ToList(),
                buttons = _zaloButton.GetAllList().Where(z => z.IdTemplate == x.Id).Select(z => new Zalo_ButtonDetailDto
                {
                    Id = z.Id,
                    IdTemplate = z.IdTemplate,
                    Type = z.Type,
                    Title = z.Title,
                    Payload = z.Payload,
                    ImageIcon = z.ImageIcon,
                    ThuTuSapXep = z.ThuTuSapXep,
                }).OrderBy(x => x.ThuTuSapXep).ToList()
            }).ToList();
            return data;
        }

        [HttpGet]
        public List<ZaloTemPlate_GroupLoaiTinDto> GetAllMauTinZalo_groupLoaiTin()
        {
            var data = _zaloTemplate.GetAllList().Select(x => new Zalo_TemplateDto
            {
                Id = x.Id,
                TenMauTin = x.TenMauTin,
                IsDefault = x.IsDefault,
                IdLoaiTin = x.IdLoaiTin,
                TemplateType = x.TemplateType,
                Language = x.Language,
                IsSystem = false,
                elements = _zaloElement.GetAllList().Where(o => o.IdTemplate == x.Id)
                .Select(o => new Zalo_ElementDto
                {
                    Id = o.Id,
                    IdTemplate = o.IdTemplate,
                    ElementType = o.ElementType,
                    ThuTuSapXep = o.ThuTuSapXep,
                    IsImage = o.IsImage,
                    Content = o.Content
                }).OrderBy(x => x.ThuTuSapXep).ToList()
            }).ToList();

            var dtGr = data.GroupBy(x => new { x.IdLoaiTinZalo, x.TenLoaiTinZalo }).Select(x => new ZaloTemPlate_GroupLoaiTinDto
            {
                IdLoaiTinZalo = x.Key.IdLoaiTinZalo,
                TenLoaiTinZalo = x.Key.TenLoaiTinZalo,
                LstDetail = x.ToList()
            }).OrderBy(x => x.IdLoaiTinZalo).ToList();

            return dtGr;
        }
        public async Task<Zalo_TemplateDto> GetZaloTemplate_Default(byte idLoaiTin)
        {
            return await _zaloTemplateRepo.FindTempDefault_ByIdLoaiTin(idLoaiTin);
        }
        private async Task AddListElement(Guid idTemp, List<Zalo_ElementDto> lstElm)
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
        private async Task AddListButton(Guid idTemp, List<Zalo_ButtonDetailDto> lstButton)
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
        [HttpGet]
        /// <summary>
        /// xóa hẳn khỏi database
        /// </summary>
        /// <param name="idTemp"></param>
        /// <returns></returns>
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

        [HttpGet]
        /// <summary>
        /// xóa mềm: chỉ update trạng thái
        /// </summary>
        /// <param name="idTemp"></param>
        /// <returns></returns>
        public async Task<bool> XoaMauTinZalo(Guid idTemp)
        {
            Zalo_Template objUp = await _zaloTemplate.FirstOrDefaultAsync(idTemp);
            if (objUp != null)
            {
                objUp.IsDeleted = true;
                objUp.DeletionTime = DateTime.Now;
                objUp.DeleterUserId = AbpSession.UserId;

                _zaloButton.GetAllList().Where(x => x.IdTemplate == idTemp).ToList().ForEach(x =>
                { x.IsDeleted = true; x.DeletionTime = DateTime.Now; x.DeleterUserId = AbpSession.UserId; });

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
                                    await _zaloTable.DeleteAsync(tbl.Id);
                                }
                                await _zaloElement.DeleteAsync(item.Id);
                            }
                            break;
                        default:
                            await _zaloElement.DeleteAsync(item.Id);
                            break;
                    }
                }
                await _zaloTemplate.UpdateAsync(objUp);
            }
            return true;
        }
    }
}
