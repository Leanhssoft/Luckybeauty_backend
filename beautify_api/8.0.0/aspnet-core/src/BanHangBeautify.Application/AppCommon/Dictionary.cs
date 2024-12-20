﻿using System.Collections.Generic;

namespace BanHangBeautify.AppCommon
{
    public class Dictionary
    {
        public static class MauInTeamPlate
        {
            public const string HD = "HD";
            public const string GDV = "GDV";
            public const string TGT = "TGT";
            public const string SQPT = "SQPT";
            public const string SQPC = "SQPC";
        }

        public static Dictionary<string, string> DanhSachMauInA4 = new Dictionary<string, string>()
        {
            {MauInTeamPlate.HD.ToString(),"HoaDonBan.txt" },
            {MauInTeamPlate.SQPT.ToString(),"PhieuThu.txt" },
            {MauInTeamPlate.SQPC.ToString(),"PhieuChi.txt" },
        };
        public static Dictionary<string, string> DanhSachMauInK80 = new Dictionary<string, string>()
        {
            {MauInTeamPlate.HD.ToString(),"K80_HoaDonBan.txt" },
            {MauInTeamPlate.GDV.ToString(),"K80_GoiDichVu.txt" },
            {MauInTeamPlate.TGT.ToString(),"K80_TheGiaTri.txt" },
            {MauInTeamPlate.SQPT.ToString(),"K80_PhieuThu.txt" },
            {MauInTeamPlate.SQPC.ToString(),"K80_PhieuChi.txt" },
        };
    }
}
