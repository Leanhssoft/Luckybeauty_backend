using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using BanHangBeautify.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHangBeautify.KhachHang.AnhLieuTrinh
{
    public class AnhLieuTrinhAppService : SPAAppServiceBase
    {
        private readonly IRepository<KhachHang_Anh_LieuTrinh, Guid> _anhLieuTrinh;
        private readonly IRepository<KhachHang_Anh_LieuTrinh_ChiTiet, Guid> _anhLieuTrinhChiTiet;
        public AnhLieuTrinhAppService(IRepository<KhachHang_Anh_LieuTrinh, Guid> anhLieuTrinh, IRepository<KhachHang_Anh_LieuTrinh_ChiTiet, Guid> anhLieuTrinhChiTiet)
        {
            _anhLieuTrinh = anhLieuTrinh;
            _anhLieuTrinhChiTiet = anhLieuTrinhChiTiet;
        }

        public async Task<List<AnhLieuTrinh_ChiTietDto>> AddListImages(Guid albumId, List<AnhLieuTrinh_ChiTietDto> lst)
        {
            List<KhachHang_Anh_LieuTrinh_ChiTiet> lstImages = new();
            if (lst != null && lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    KhachHang_Anh_LieuTrinh_ChiTiet ctNew = ObjectMapper.Map<KhachHang_Anh_LieuTrinh_ChiTiet>(item);
                    ctNew.Id = Guid.NewGuid();
                    ctNew.AlbumId = albumId;
                    ctNew.CreatorUserId = AbpSession.UserId;
                    ctNew.CreationTime = DateTime.Now;
                    lstImages.Add(ctNew);
                }
                await _anhLieuTrinhChiTiet.InsertRangeAsync(lstImages);
            }
            return ObjectMapper.Map<List<AnhLieuTrinh_ChiTietDto>>(lstImages);
        }
        [HttpPost]
        public async Task<AnhLieuTrinhDto> Insert_AnhLieuTrinh(AnhLieuTrinhDto dto)
        {
            KhachHang_Anh_LieuTrinh obj = ObjectMapper.Map<KhachHang_Anh_LieuTrinh>(dto);
            obj.Id = Guid.NewGuid();
            obj.TenantId = AbpSession.TenantId ?? 1;
            obj.CreatorUserId = AbpSession.UserId;
            obj.CreationTime = DateTime.Now;
            await _anhLieuTrinh.InsertAsync(obj);

            var result = ObjectMapper.Map<AnhLieuTrinhDto>(obj);
            result.LstAnhLieuTrinh = await AddListImages(obj.Id, dto.LstAnhLieuTrinh);
            return result;
        }
        [HttpPost]
        public async Task<AnhLieuTrinhDto> Update_AnhLieuTrinh(AnhLieuTrinhDto objUp)
        {
            KhachHang_Anh_LieuTrinh objOld = await _anhLieuTrinh.FirstOrDefaultAsync(objUp.Id);
            if (objOld != null)
            {
                objOld.AlbumName = objUp.AlbumName;
                objOld.LastModificationTime = DateTime.Now;
                objOld.LastModificationTime = DateTime.Now;
                await _anhLieuTrinh.UpdateAsync(objOld);
            }

            var result = ObjectMapper.Map<AnhLieuTrinhDto>(objUp);
            result.LstAnhLieuTrinh = await AddListImages(objUp.Id, objUp.LstAnhLieuTrinh);
            return result;
        }

        public List<AnhLieuTrinh_ChiTietDto> GetAllImage_inAlbum(Guid albumId)
        {
            var lstImages = _anhLieuTrinhChiTiet.GetAll().Where(x => x.AlbumId == albumId);
            if (lstImages.Any())
            {
                return ObjectMapper.Map<List<AnhLieuTrinh_ChiTietDto>>(lstImages);
            }
            return null;
        }
        [HttpGet]
        public async Task<bool> RemoveAlbum_byId(Guid albumId)
        {
            var album = await _anhLieuTrinh.FirstOrDefaultAsync(albumId);
            if (album != null)
            {
                _anhLieuTrinh.HardDelete(album);
                var lstImages = _anhLieuTrinhChiTiet.GetAllList().Where(x => x.AlbumId == albumId);
                if (lstImages.Any())
                {
                    foreach (var item in lstImages)
                    {
                        _anhLieuTrinhChiTiet.HardDelete(item);
                    }
                }
                return true;
            }
            return false;
        }
        [HttpGet]
        public async Task<bool> Remove_AnhLieuTrinhChiTiet(Guid idChiTietAnh)
        {
            var image = await _anhLieuTrinhChiTiet.FirstOrDefaultAsync(idChiTietAnh);
            if (image != null)
            {
                _anhLieuTrinhChiTiet.HardDelete(image);
                return true;
            }
            return false;
        }
        public List<AnhLieuTrinhDto> GetAllAlbum_ofCustomer(Guid idKhachHang)
        {
            var lstAlbum = _anhLieuTrinh.GetAll().Where(x => x.IdKhachHang == idKhachHang).ToList();
            if (lstAlbum.Any())
            {
                var dtgr = lstAlbum.Select(x => new AnhLieuTrinhDto
                {
                    Id = x.Id,
                    AlbumName = x.AlbumName,
                    CreationTime = x.CreationTime,
                    TongSoAnh = GetAllImage_inAlbum(x.Id)?.ToList()?.Count ?? 0
                }).ToList();
                return ObjectMapper.Map<List<AnhLieuTrinhDto>>(dtgr);
            }
            return null;
        }

        public string GetInforImage_OfAnyAnhLieuTrinh()
        {
            var data = _anhLieuTrinhChiTiet.GetAll().Where(x => !string.IsNullOrEmpty(x.ImageUrl)).FirstOrDefault();
            if (data != null)
            {
                return data.ImageUrl;
            }
            return string.Empty;
        }
    }
}
