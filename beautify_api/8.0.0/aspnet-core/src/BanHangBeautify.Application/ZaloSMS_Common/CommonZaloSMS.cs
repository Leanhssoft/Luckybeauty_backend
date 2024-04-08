using BanHangBeautify.AppDanhMuc.AppCuaHang;
using BanHangBeautify.AppDanhMuc.AppCuaHang.Dto;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.Zalo.ZaloTemplate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BanHangBeautify.ZaloSMS_Common
{
    public class CommonZaloSMS : SPAAppServiceBase, ICommonZaloSMS
    {
        public CommonZaloSMS()
        {
        }

        public string ReplaceContent_Withkey(PageKhachHangSMSDto cutomer, string key)
        {
            string txt = string.Empty;
            //Regex regex = new(@"^<.*>$");
            //if (regex.IsMatch(key))
            //{
            switch (key)
            {
                case "{TenKhachHang}":
                    txt = cutomer.TenKhachHang;
                    break;
                case "{SoDienThoai}":
                    txt = cutomer.SoDienThoai;
                    break;
                case "{XungHo}":
                    txt = cutomer.XungHo;
                    break;
                case "{NgaySinh}":
                    txt = cutomer.NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    break;
                case "{BookingDate}":
                    txt = cutomer.StartTime?.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture);
                    break;
                case "{ThoiGianHen}":
                    txt = cutomer.ThoiGianHen;
                    break;
                case "{TenDichVu}":
                    txt = cutomer.TenHangHoa;
                    break;
                case "{MaHoaDon}":
                    txt = cutomer.MaHoaDon;
                    break;
                case "{NgayLapHoaDon}":
                    txt = cutomer.NgayLapHoaDon?.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture);
                    break;
                case "{TongTienHang}":
                    {
                        double number = cutomer.TongThanhToan ?? 0;
                        txt = number.ToString("N0", new CultureInfo("vi-VN"));
                    }
                    break;  
                case "{DaThanhToan}":
                    {
                        double number = cutomer.DaThanhToan ?? 0;
                        txt = number.ToString("N0", new CultureInfo("vi-VN"));
                    }
                    break;
                case "{PTThanhToan}":
                    txt = cutomer.PTThanhToan;
                    break;
                case "{TenChiNhanh}":
                    txt = cutomer.TenChiNhanh;
                    break;
                case "{DienThoaiChiNhanh}":
                    txt = cutomer.SoDienThoaiChiNhanh;
                    break;
                case "{DiaChiChiNhanh}":
                    txt = cutomer.DiaChiChiNhanh;
                    break;
                case "{TenCuaHang}":
                    txt = cutomer?.TenCuaHang ?? "";
                    break;
                case "{DiaChiCuaHang}":
                    txt = cutomer?.DiaChiCuaHang ?? "";
                    break;
                case "{DienThoaiCuaHang}":
                    txt = cutomer?.DienThoaiCuaHang ?? "";
                    break;
            }
            //}
            return txt;
        }
        public string ReplaceContent(PageKhachHangSMSDto cutomer, string noiDungTin)
        {
            if (string.IsNullOrEmpty(noiDungTin)) return string.Empty;
            var ss = noiDungTin.Replace("{TenKhachHang}", cutomer.TenKhachHang);
            ss = ss.Replace("{NgaySinh}", cutomer.NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            ss = ss.Replace("{SoDienThoai}", cutomer.SoDienThoai);
            ss = ss.Replace("{XungHo}", cutomer.XungHo);
            ss = ss.Replace("{BookingDate}", cutomer.StartTime?.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture));
            ss = ss.Replace("{ThoiGianHen}", cutomer.ThoiGianHen);
            ss = ss.Replace("{TenDichVu}", cutomer.TenHangHoa);// dichvuhen
            ss = ss.Replace("{MaHoaDon}", cutomer.MaHoaDon);
            ss = ss.Replace("{NgayLapHoaDon}", cutomer.NgayLapHoaDon?.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture));
            ss = ss.Replace("{TongTienHang}", (cutomer.TongThanhToan ?? 0).ToString("N0", new CultureInfo("vi-VN")));
            ss = ss.Replace("{DaThanhToan}", (cutomer.DaThanhToan ?? 0).ToString("N0", new CultureInfo("vi-VN")));
            ss = ss.Replace("{PTThanhToan}", cutomer.PTThanhToan);
            ss = ss.Replace("{TenChiNhanh}", cutomer.TenChiNhanh);
            ss = ss.Replace("{DienThoaiChiNhanh}", cutomer.SoDienThoaiChiNhanh);
            ss = ss.Replace("{DiaChiChiNhanh}", cutomer.DiaChiChiNhanh);
            ss = ss.Replace("{TenCuaHang}", cutomer?.TenCuaHang ?? "");
            ss = ss.Replace("{DiaChiCuaHang}", cutomer?.DiaChiCuaHang ?? "");
            ss = ss.Replace("{DienThoaiCuaHang}", cutomer?.DienThoaiCuaHang ?? "");
            return ss;
        }
    }
}
