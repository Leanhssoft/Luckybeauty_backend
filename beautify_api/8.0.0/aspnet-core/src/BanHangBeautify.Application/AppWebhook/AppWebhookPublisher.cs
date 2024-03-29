using Abp.Authorization;
using Abp.Webhooks;
using Azure.Core;
using BanHangBeautify.AppWebhook.Dto;
using BanHangBeautify.Consts;
using BanHangBeautify.KhachHang.KhachHang;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.Zalo.DangKy_ThanhVien;
using BanHangBeautify.Zalo.DangKyThanhVien;
using BanHangBeautify.Zalo.KetNoi_XacThuc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BanHangBeautify.AppWebhook
{
    [AbpAuthorize]
    public class AppWebhookPublisher : SPAAppServiceBase, IAppWebhookPublisher
    {
        private readonly IWebhookPublisher _webHookPublisher;
        private readonly IZalo_KhachHangThanhVienAppService _zaloKhachHangThanhVien;
        private readonly IKhachHangAppService _khachHangAppService;
        private readonly IZaloAuthorization _zaloAuthen;
        public AppWebhookPublisher(IWebhookPublisher webHookPublisher, IZalo_KhachHangThanhVienAppService zaloKhachHangThanhVien,
            IKhachHangAppService khachHangAppService, IZaloAuthorization zaloAuthen)
        {
            _webHookPublisher = webHookPublisher;
            _zaloKhachHangThanhVien = zaloKhachHangThanhVien;
            _khachHangAppService = khachHangAppService;
            _zaloAuthen = zaloAuthen;
        }
        /// <summary>
        /// zaloUserId: user của user (quantam/khong quan tam oa)
        /// </summary>
        /// <param name="zaloUserId"></param>
        /// <returns></returns>
        public async Task<Guid?> AddUpdate_ZaloKhachHangThanhVien(string zaloUserId)
        {
            var zaloToken = await _zaloAuthen.Innit_orGetToken();
            var userInfor = await _zaloAuthen.GetInforUser_ofOA(zaloToken.AccessToken, zaloUserId);
            if (userInfor != null)
            {
                Zalo_KhachHangThanhVienDto newUser = new()
                {
                    DisplayName = userInfor.DisplayName,
                    UserIdByApp = userInfor.UserIdByApp,
                    UserIsFollower = userInfor.UserIsFollower,
                    Avatar = userInfor.Avatar,
                    ZOAUserId = zaloUserId
                };

                var idKhachHangThanhVien = _zaloKhachHangThanhVien.GetId_fromZOAUserId(zaloUserId);
                if (idKhachHangThanhVien != null)
                {
                    await _zaloKhachHangThanhVien.UpdateThanhVienZOA(newUser);
                }
                else
                {
                    var dataNew = await _zaloKhachHangThanhVien.DangKyThanhVienZOA(newUser);
                    idKhachHangThanhVien = dataNew.Id;
                }
                return idKhachHangThanhVien;
            }
            return null;
        }
        public async Task AddNewCustomer_ShareInfor(Guid idKhachHangThanhVien, ZaloUserShareInforDto inforShare)
        {
            CreateOrEditKhachHangDto newCus = new()
            {
                Id = Guid.NewGuid(),
                TenKhachHang = inforShare.name,
                SoDienThoai = inforShare.phone,
                DiaChi = inforShare.address,
                TrangThai = 1,
                IdLoaiKhach = 1,
                IdKhachHangZOA = idKhachHangThanhVien
            };
            await _khachHangAppService.CreateOrEdit(newCus);
        }

        public async Task UserRecieveMessage(int tenantId)
        {
            await _webHookPublisher.PublishAsync(ConstAppWebHookNames.ZOA_UserRecieveMessage, tenantId);
        }
    }
}
