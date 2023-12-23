using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using BanHangBeautify.AppCommon;
using BanHangBeautify.AppDanhMuc.MauIn.Dto;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.MauIn
{
    //[AbpAuthorize(PermissionNames.Pages_MauIn)]
    public class MauInAppService : SPAAppServiceBase
    {
        private readonly IRepository<DM_MauIn, Guid> _dmMauInRepository;
        private readonly IRepository<DM_LoaiChungTu> _dmLoaiChungTu;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        //public static readonly string App = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //public static readonly string Templates = Path.Combine(App, "Template");
        //private readonly IHostingEnvironment _env;
        public MauInAppService(IWebHostEnvironment hostEnvironment, IRepository<DM_MauIn, Guid> dmMauInRepository, IRepository<DM_LoaiChungTu> dmLoaiChungTu,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _dmMauInRepository = dmMauInRepository;
            _hostEnvironment = hostEnvironment;
            _dmLoaiChungTu = dmLoaiChungTu;
            _unitOfWorkManager = unitOfWorkManager;
        }
        [HttpPost]
        public async Task<CreateOrEditMauInDto> InsertMauIn(CreateOrEditMauInDto input)
        {
            DM_MauIn data = ObjectMapper.Map<DM_MauIn>(input);
            data.Id = Guid.NewGuid();
            data.CreationTime = DateTime.Now;
            data.CreatorUserId = AbpSession.UserId;
            data.IsDeleted = false;
            data.TrangThai = 1;
            await _dmMauInRepository.InsertAsync(data);
            return ObjectMapper.Map<CreateOrEditMauInDto>(data);
        }
        [HttpPost]
        public async Task<CreateOrEditMauInDto> UpdateMauIn(CreateOrEditMauInDto input)
        {
            var objUpdate = await _dmMauInRepository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (objUpdate != null)
            {
                objUpdate.TenMauIn = input.TenMauIn;
                objUpdate.NoiDungMauIn = input.NoiDungMauIn;
                objUpdate.LaMacDinh = input.LaMacDinh;
                objUpdate.IdChiNhanh = input.IdChiNhanh;
                objUpdate.LoaiChungTu = input.LoaiChungTu;
                objUpdate.LastModificationTime = DateTime.Now;
                objUpdate.LastModifierUserId = AbpSession.UserId;
                if (input.LaMacDinh)
                {
                    // update các mẫu còn lại (mặc định = false)
                    _dmMauInRepository.GetAll().Where(x => x.Id != input.Id && x.IdChiNhanh == input.IdChiNhanh && x.LaMacDinh).ToList().ForEach(x => x.LaMacDinh = false);
                }
                await _dmMauInRepository.UpdateAsync(objUpdate);
            }
            return ObjectMapper.Map<CreateOrEditMauInDto>(objUpdate);
        }
        [HttpGet]
        public async Task<CreateOrEditMauInDto> DeleteMauIn(Guid id)
        {
            var data = await _dmMauInRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeleterUserId = AbpSession.UserId;
                data.DeletionTime = DateTime.Now;
                data.LaMacDinh = false;
                data.TrangThai = 0;
                _dmMauInRepository.Update(data);
                return ObjectMapper.Map<CreateOrEditMauInDto>(data);
            }
            return new CreateOrEditMauInDto();
        }
        public async Task<CreateOrEditMauInDto> GetForEdit(Guid id)
        {
            var data = await _dmMauInRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                return ObjectMapper.Map<CreateOrEditMauInDto>(data);
            }
            return new CreateOrEditMauInDto();
        }
        /// <summary>
        /// get mẫu in theo chi nhánh (nếu idChiNhanh!=null), và theo loại chứng từ
        /// </summary>
        /// <param name="idChiNhanh"></param>
        /// <param name="idLoaiChungTu"></param>
        /// <returns></returns>
        public async Task<List<CreateOrEditMauInDto>> GetAllMauIn_byChiNhanh(Guid? idChiNhanh = null, int? idLoaiChungTu = 0)
        {
            var data = await _dmMauInRepository.GetAll().Where(x => x.TenantId == (AbpSession.TenantId ?? 1) && x.IsDeleted == false
            && (idChiNhanh == null || (x.IdChiNhanh == idChiNhanh))
            && (idLoaiChungTu == 0 || (x.LoaiChungTu == idLoaiChungTu))
            ).OrderByDescending(x => x.CreationTime).ToListAsync();
            return ObjectMapper.Map<List<CreateOrEditMauInDto>>(data);
        }
        /// <summary>
        /// read content from txt file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GetFileMauIn(string file = "HoaDonBan.txt")
        {
            var contents = string.Empty;
            var pathFile = Path.Combine(_hostEnvironment.WebRootPath, @"Template\MauIn\", file);

            if (File.Exists(pathFile))
            {
                contents = System.IO.File.ReadAllText(pathFile);
            }
            return contents;
        }
        /// <summary>
        /// read file .txt by loaiMauIn
        /// </summary>
        /// <param name="type"></param>
        /// <param name="idLoaiChungTu"></param>
        /// <returns></returns>
        public async Task<string> GetContentMauInMacDinh(int type = 1, int idLoaiChungTu = 1)
        {
            string contents = string.Empty, tenMauIn = string.Empty;
            DM_LoaiChungTu loaiChungTu = null;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant)) // tắt bộ lọc tenantId
            {
                loaiChungTu = await _dmLoaiChungTu.FirstOrDefaultAsync(x => x.Id == idLoaiChungTu);
            }

            if (loaiChungTu != null)
            {
                tenMauIn = loaiChungTu.MaLoaiChungTu;

                // find temp default from DB
                var lstMauIn_byLoaiChungTu = _dmMauInRepository.GetAll().Where(x => x.LoaiChungTu == idLoaiChungTu && x.LaMacDinh && !x.IsDeleted).ToList();
                if (lstMauIn_byLoaiChungTu != null && lstMauIn_byLoaiChungTu.Count > 0)
                {
                    contents = lstMauIn_byLoaiChungTu.FirstOrDefault().NoiDungMauIn;
                }
                else
                {
                    // mau k80
                    if (type == 1)
                    {
                        var data = Dictionary.DanhSachMauInK80.Where(x => x.Key == tenMauIn);
                        if (data != null && data.Count() > 0)
                        {
                            contents = GetFileMauIn(data.FirstOrDefault().Value);
                        }
                    }
                    else
                    {
                        // mau a4
                        var data = Dictionary.DanhSachMauInA4.Where(x => x.Key == tenMauIn);
                        if (data != null && data.Count() > 0)
                        {
                            contents = GetFileMauIn(data.FirstOrDefault().Value);
                        }
                    }
                }
            }
            return contents;
        }
    }
}
