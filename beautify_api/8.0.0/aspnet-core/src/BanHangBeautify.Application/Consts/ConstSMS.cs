using System.Collections.Generic;

namespace BanHangBeautify.Consts
{
    public class ConstSMS
    {
        public class LoaiTin
        {
            public const byte TinThuong = 1;
            public const byte SinhNhat = 2;
            public const byte LichHen = 3;
            public const byte GiaoDich = 4;
            public const byte NhacLichHen = 31;
            public const byte XacNhanLichHen = 32;
            public const byte ThayDoiLichHen = 33;
            public const byte HuyLichHen = 30;
            public const byte KhuyenMai = 5;
        }
        public class HinhThucGuiTin
        {
            public const byte SMS = 1;
            public const byte Zalo = 2;
            public const byte Gmail = 3;
        }


        public class ESMS_TrangThaiTin
        {
            public const byte SUCCESS = 100;
            public const byte NOT_BALANCE = 103;
            public const byte BRANDNAME_NOTEXIST = 104;
            public const byte MESSAGE_NOT_VALID = 118;
            public const byte ERROR_UNDEFINED = 99;
        }

        public static Dictionary<string, string> EMS_ListTrangThaiGuiTin = new Dictionary<string, string>()
        {
              { ESMS_TrangThaiTin.SUCCESS.ToString(),"Thành công" },
              { ESMS_TrangThaiTin.NOT_BALANCE.ToString(),"Không đủ số dư" },
              { ESMS_TrangThaiTin.BRANDNAME_NOTEXIST.ToString(),"Brandname không tồn tại" },
              { ESMS_TrangThaiTin.MESSAGE_NOT_VALID.ToString(),"Tin nhắn không hợp lệ" },
              { ESMS_TrangThaiTin.ERROR_UNDEFINED.ToString(),"Lỗi không xác định" },
        };
    }
}
