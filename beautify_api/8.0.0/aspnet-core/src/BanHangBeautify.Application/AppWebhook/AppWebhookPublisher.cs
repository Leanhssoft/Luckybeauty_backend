using Abp.Authorization;
using Abp.Webhooks;
using BanHangBeautify.AppWebhook.Dto;
using BanHangBeautify.Consts;
using BanHangBeautify.KhachHang.KhachHang;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.Zalo.DangKy_ThanhVien;
using BanHangBeautify.Zalo.DangKyThanhVien;
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

        public AppWebhookPublisher(IWebhookPublisher webHookPublisher, IZalo_KhachHangThanhVienAppService zaloKhachHangThanhVien,
            IKhachHangAppService khachHangAppService)
        {
            _webHookPublisher = webHookPublisher;
            _zaloKhachHangThanhVien = zaloKhachHangThanhVien;
            _khachHangAppService = khachHangAppService;
        }
        /// <summary>
        /// userInfor: thông tin người dùng chia sẻ từ ZOA (chỉ gửi webhook đến tennant hiện tại)
        /// </summary>
        /// <param name="userInfor"></param>
        /// <param name="zaloUserId"></param>
        /// <returns></returns>
        public async Task UserSendMessage(ZOA_InforUserSubmit userInfor, string zaloUserId)
        {
            Zalo_KhachHangThanhVienDto newUser = new()
            {
                TenDangKy = userInfor.Name,
                SoDienThoaiDK = userInfor.Phone,
                DiaChi = userInfor.Address,
                TenTinhThanh = userInfor.City, // todo get IdQuanHuyen + IdTinhThanh 
                TenQuanHuyen = userInfor.District,
                ZOAUserId = zaloUserId
            };

            var dataUser = await _zaloKhachHangThanhVien.DangKyThanhVienZOA(newUser);
           
            var exists = await _khachHangAppService.CheckExistSoDienThoai(userInfor.Phone);
            if (exists)
            {
                List<Guid> arrIdCustomer = await _khachHangAppService.GetListCustomerId_byPhone(userInfor.Phone);
                if (arrIdCustomer != null && arrIdCustomer.Count > 0)
                {
                    await _khachHangAppService.Update_IdKhachHangZOA(arrIdCustomer[0], dataUser.Id);
                }
            }
            else
            {
                CreateOrEditKhachHangDto customer = new()
                {
                    IdKhachHangZOA = dataUser.Id,
                    TenKhachHang = userInfor.Name,
                    TenKhachHang_KhongDau = BanHangBeautify.AppCommon.ConvertHelper.ConvertToUnSign(userInfor.Name),
                    SoDienThoai = userInfor.Phone,
                    DiaChi = userInfor.Address,
                    GioiTinhNam = false,
                    TrangThai = 1,
                    IdLoaiKhach = LoaiKhachHang.KHACH_HANG,
                };
                await _khachHangAppService.CreateOrEdit(customer);
            }

            //await _webHookPublisher.PublishAsync(ConstAppWebHookNames.ZOA_UserSendMessage, userInfor, AbpSession.TenantId ?? 1);
        }

        public async Task UserRecieveMessage(int tenantId)
        {
            await _webHookPublisher.PublishAsync(ConstAppWebHookNames.ZOA_UserRecieveMessage, tenantId);
        }
    }
}
