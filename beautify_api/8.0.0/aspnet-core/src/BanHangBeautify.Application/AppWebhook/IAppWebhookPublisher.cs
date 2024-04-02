using BanHangBeautify.AppWebhook.Dto;
using BanHangBeautify.Zalo.DangKyThanhVien;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.AppWebhook
{
    public interface IAppWebhookPublisher
    {
        Task<Guid?> AddUpdate_ZaloKhachHangThanhVien(string zaloUserId);
        Task AddNewCustomer_ShareInfor(Guid idKhachHangThanhVien, ZaloUserShareInforDto inforShare);
        Task UserRecieveMessage(int tenantId);
    }
}
