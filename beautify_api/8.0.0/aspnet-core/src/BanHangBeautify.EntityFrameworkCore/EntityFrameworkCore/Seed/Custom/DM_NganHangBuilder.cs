using BanHangBeautify.Data.Entities;
using BanHangBeautify.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.EntityFrameworkCore.Seed.Custom
{
    public class DM_NganHangBuilder
    {
        private readonly SPADbContext _context;

        public DM_NganHangBuilder(SPADbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            CreateDMNganHang();
        }
        private void CreateDMNganHang()
        {
            var exists = _context.DM_NganHang.Where(x => x.TenantId == null || x.TenantId == 1).Count() > 10;
            if (!exists)
            {
                List<DM_NganHang> lstBank = new();
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "ABB", TenNganHang = "Ngân hàng TMCP An Bình", TenRutGon = "ABBANK", BIN = "970425", Logo = "https://api.vietqr.io/img/ABB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "ACB", TenNganHang = "Ngân hàng TMCP Á Châu", TenRutGon = "ACB", BIN = "970416", Logo = "https://api.vietqr.io/img/ACB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "BAB", TenNganHang = "Ngân hàng TMCP Bắc Á", TenRutGon = "BacABank", BIN = "970409", Logo = "https://api.vietqr.io/img/BAB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "BIDV", TenNganHang = "Ngân hàng TMCP Đầu tư và Phát triển Việt Nam", TenRutGon = "BIDV", BIN = "970418", Logo = "https://api.vietqr.io/img/BIDV.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "BVB", TenNganHang = "Ngân hàng TMCP Bảo Việt", TenRutGon = "BaoVietBank", BIN = "970438", Logo = "https://api.vietqr.io/img/BVB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "CAKE", TenNganHang = "TMCP Việt Nam Thịnh Vượng - Ngân hàng số CAKE by VPBank", TenRutGon = "CAKE", BIN = "546034", Logo = "https://api.vietqr.io/img/CAKE.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "CBB", TenNganHang = "Ngân hàng Thương mại TNHH MTV Xây dựng Việt Nam", TenRutGon = "CBBank", BIN = "970444", Logo = "https://api.vietqr.io/img/CBB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "CIMB", TenNganHang = "Ngân hàng TNHH MTV CIMB Việt Nam", TenRutGon = "CIMB", BIN = "422589", Logo = "https://api.vietqr.io/img/CIMB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "CITIBANK", TenNganHang = "Ngân hàng Citibank, N.A. - Chi nhánh Hà Nội", TenRutGon = "Citibank", BIN = "533948", Logo = "https://api.vietqr.io/img/CITIBANK.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "COOPBANK", TenNganHang = "Ngân hàng Hợp tác xã Việt Nam", TenRutGon = "COOPBANK", BIN = "970446", Logo = "https://api.vietqr.io/img/COOPBANK.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "DBS", TenNganHang = "DBS Bank Ltd - Chi nhánh Thành phố Hồ Chí Minh", TenRutGon = "DBSBank", BIN = "796500", Logo = "https://api.vietqr.io/img/DBS.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "DOB", TenNganHang = "Ngân hàng TMCP Đông Á", TenRutGon = "DongABank", BIN = "970406", Logo = "https://api.vietqr.io/img/DOB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "EIB", TenNganHang = "Ngân hàng TMCP Xuất Nhập khẩu Việt Nam", TenRutGon = "Eximbank", BIN = "970431", Logo = "https://api.vietqr.io/img/EIB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "GPB", TenNganHang = "Ngân hàng Thương mại TNHH MTV Dầu Khí Toàn Cầu", TenRutGon = "GPBank", BIN = "970408", Logo = "https://api.vietqr.io/img/GPB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "HDB", TenNganHang = "Ngân hàng TMCP Phát triển Thành phố Hồ Chí Minh", TenRutGon = "HDBank", BIN = "970437", Logo = "https://api.vietqr.io/img/HDB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "HLBVN", TenNganHang = "Ngân hàng TNHH MTV Hong Leong Việt Nam", TenRutGon = "HongLeong", BIN = "970442", Logo = "https://api.vietqr.io/img/HLBVN.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "HSBC", TenNganHang = "Ngân hàng TNHH MTV HSBC (Việt Nam)", TenRutGon = "HSBC", BIN = "458761", Logo = "https://api.vietqr.io/img/HSBC.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "IBK - HCM", TenNganHang = "Ngân hàng Công nghiệp Hàn Quốc - Chi nhánh TP. Hồ Chí Minh", TenRutGon = "IBKHCM", BIN = "970456", Logo = "https://api.vietqr.io/img/IBK.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "IBK - HN", TenNganHang = "Ngân hàng Công nghiệp Hàn Quốc - Chi nhánh Hà Nội", TenRutGon = "IBKHN", BIN = "970455", Logo = "https://api.vietqr.io/img/IBK.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "ICB", TenNganHang = "Ngân hàng TMCP Công thương Việt Nam", TenRutGon = "VietinBank", BIN = "970415", Logo = "https://api.vietqr.io/img/ICB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "IVB", TenNganHang = "Ngân hàng TNHH Indovina", TenRutGon = "IndovinaBank", BIN = "970434", Logo = "https://api.vietqr.io/img/IVB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "KBank", TenNganHang = "Ngân hàng Đại chúng TNHH Kasikornbank", TenRutGon = "KBank", BIN = "668888", Logo = "https://api.vietqr.io/img/KBANK.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "KBHCM", TenNganHang = "Ngân hàng Kookmin - Chi nhánh Thành phố Hồ Chí Minh", TenRutGon = "KookminHCM", BIN = "970463", Logo = "https://api.vietqr.io/img/KBHCM.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "KBHN", TenNganHang = "Ngân hàng Kookmin - Chi nhánh Hà Nội", TenRutGon = "KookminHN", BIN = "970462", Logo = "https://api.vietqr.io/img/KBHN.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "KEBHANAHCM", TenNganHang = "Ngân hàng KEB Hana – Chi nhánh Thành phố Hồ Chí Minh", TenRutGon = "KEBHanaHCM", BIN = "970466", Logo = "https://api.vietqr.io/img/KEBHANAHCM.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "KEBHANAHN", TenNganHang = "Ngân hàng KEB Hana – Chi nhánh Hà Nội", TenRutGon = "KEBHANAHN", BIN = "970467", Logo = "https://api.vietqr.io/img/KEBHANAHN.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "KLB", TenNganHang = "Ngân hàng TMCP Kiên Long", TenRutGon = "KienLongBank", BIN = "970452", Logo = "https://api.vietqr.io/img/KLB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "LPB", TenNganHang = "Ngân hàng TMCP Bưu Điện Liên Việt", TenRutGon = "LienVietPostBank", BIN = "970449", Logo = "https://api.vietqr.io/img/LPB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "MAFC", TenNganHang = "Công ty Tài chính TNHH MTV Mirae Asset (Việt Nam) ", TenRutGon = "MAFC", BIN = "977777", Logo = "https://api.vietqr.io/img/MAFC.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "MB", TenNganHang = "Ngân hàng TMCP Quân đội", TenRutGon = "MBBank", BIN = "970422", Logo = "https://api.vietqr.io/img/MB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "MSB", TenNganHang = "Ngân hàng TMCP Hàng Hải", TenRutGon = "MSB", BIN = "970426", Logo = "https://api.vietqr.io/img/MSB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "NAB", TenNganHang = "Ngân hàng TMCP Nam Á", TenRutGon = "NamABank", BIN = "970428", Logo = "https://api.vietqr.io/img/NAB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "NCB", TenNganHang = "Ngân hàng TMCP Quốc Dân", TenRutGon = "NCB", BIN = "970419", Logo = "https://api.vietqr.io/img/NCB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "NHB HN", TenNganHang = "Ngân hàng Nonghyup - Chi nhánh Hà Nội", TenRutGon = "Nonghyup", BIN = "801011", Logo = "https://api.vietqr.io/img/NHB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "OCB", TenNganHang = "Ngân hàng TMCP Phương Đông", TenRutGon = "OCB", BIN = "970448", Logo = "https://api.vietqr.io/img/OCB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "Oceanbank", TenNganHang = "Ngân hàng Thương mại TNHH MTV Đại Dương", TenRutGon = "Oceanbank", BIN = "970414", Logo = "https://api.vietqr.io/img/OCEANBANK.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "PBVN", TenNganHang = "Ngân hàng TNHH MTV Public Việt Nam", TenRutGon = "PublicBank", BIN = "970439", Logo = "https://api.vietqr.io/img/PBVN.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "PGB", TenNganHang = "Ngân hàng TMCP Xăng dầu Petrolimex", TenRutGon = "PGBank", BIN = "970430", Logo = "https://api.vietqr.io/img/PGB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "PVCB", TenNganHang = "Ngân hàng TMCP Đại Chúng Việt Nam", TenRutGon = "PVcomBank", BIN = "970412", Logo = "https://api.vietqr.io/img/PVCB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "SCB", TenNganHang = "Ngân hàng TMCP Sài Gòn", TenRutGon = "SCB", BIN = "970429", Logo = "https://api.vietqr.io/img/SCB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "SCVN", TenNganHang = "Ngân hàng TNHH MTV Standard Chartered Bank Việt Nam", TenRutGon = "StandardChartered", BIN = "970410", Logo = "https://api.vietqr.io/img/SCVN.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "SEAB", TenNganHang = "Ngân hàng TMCP Đông Nam Á", TenRutGon = "SeABank", BIN = "970440", Logo = "https://api.vietqr.io/img/SEAB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "SGICB", TenNganHang = "Ngân hàng TMCP Sài Gòn Công Thương", TenRutGon = "SaigonBank", BIN = "970400", Logo = "https://api.vietqr.io/img/SGICB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "SHB", TenNganHang = "Ngân hàng TMCP Sài Gòn - Hà Nội", TenRutGon = "SHB", BIN = "970443", Logo = "https://api.vietqr.io/img/SHB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "SHBVN", TenNganHang = "Ngân hàng TNHH MTV Shinhan Việt Nam", TenRutGon = "ShinhanBank", BIN = "970424", Logo = "https://api.vietqr.io/img/SHBVN.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "STB", TenNganHang = "Ngân hàng TMCP Sài Gòn Thương Tín", TenRutGon = "Sacombank", BIN = "970403", Logo = "https://api.vietqr.io/img/STB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "TCB", TenNganHang = "Ngân hàng TMCP Kỹ thương Việt Nam", TenRutGon = "Techcombank", BIN = "970407", Logo = "https://api.vietqr.io/img/TCB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "TIMO", TenNganHang = "Ngân hàng số Timo by Ban Viet Bank (Timo by Ban Viet Bank)", TenRutGon = "Timo", BIN = "963388", Logo = "https://vietqr.net/portal-service/resources/icons/TIMO.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "TPB", TenNganHang = "Ngân hàng TMCP Tiên Phong", TenRutGon = "TPBank", BIN = "970423", Logo = "https://api.vietqr.io/img/TPB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "Ubank", TenNganHang = "TMCP Việt Nam Thịnh Vượng - Ngân hàng số Ubank by VPBank", TenRutGon = "Ubank", BIN = "546035", Logo = "https://api.vietqr.io/img/UBANK.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "UOB", TenNganHang = "Ngân hàng United Overseas - Chi nhánh TP. Hồ Chí Minh", TenRutGon = "UnitedOverseas", BIN = "970458", Logo = "https://api.vietqr.io/img/UOB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "VAB", TenNganHang = "Ngân hàng TMCP Việt Á", TenRutGon = "VietABank", BIN = "970427", Logo = "https://api.vietqr.io/img/VAB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "VBA", TenNganHang = "Ngân hàng Nông nghiệp và Phát triển Nông thôn Việt Nam", TenRutGon = "Agribank", BIN = "970405", Logo = "https://api.vietqr.io/img/VBA.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "VBSP", TenNganHang = "Ngân hàng Chính sách Xã hội", TenRutGon = "VBSP", BIN = "999888", Logo = "https://api.vietqr.io/img/VBSP.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "VCB", TenNganHang = "Ngân hàng TMCP Ngoại Thương Việt Nam", TenRutGon = "Vietcombank", BIN = "970436", Logo = "https://api.vietqr.io/img/VCB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "VCCB", TenNganHang = "Ngân hàng TMCP Bản Việt", TenRutGon = "VietCapitalBank", BIN = "970454", Logo = "https://api.vietqr.io/img/VCCB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "VIB", TenNganHang = "Ngân hàng TMCP Quốc tế Việt Nam", TenRutGon = "VIB", BIN = "970441", Logo = "https://api.vietqr.io/img/VIB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "VIETBANK", TenNganHang = "Ngân hàng TMCP Việt Nam Thương Tín", TenRutGon = "VietBank", BIN = "970433", Logo = "https://api.vietqr.io/img/VIETBANK.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "VNPTMONEY", TenNganHang = "VNPT Money", TenRutGon = "VNPTMoney", BIN = "971011", Logo = "https://api.vietqr.io/img/VNPTMONEY.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "VPB", TenNganHang = "Ngân hàng TMCP Việt Nam Thịnh Vượng", TenRutGon = "VPBank", BIN = "970432", Logo = "https://api.vietqr.io/img/VPB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "VRB", TenNganHang = "Ngân hàng Liên doanh Việt - Nga", TenRutGon = "VRB", BIN = "970421", Logo = "https://api.vietqr.io/img/VRB.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "VTLMONEY", TenNganHang = "Tổng Công ty Dịch vụ số Viettel - Chi nhánh tập đoàn công nghiệp viễn thông Quân Đội", TenRutGon = "ViettelMoney", BIN = "971005", Logo = "https://api.vietqr.io/img/VIETTELMONEY.png" });
                lstBank.Add(new DM_NganHang { Id = Guid.NewGuid(), MaNganHang = "WVN", TenNganHang = "Ngân hàng TNHH MTV Woori Việt Nam", TenRutGon = "Woori", BIN = "970457", Logo = "https://api.vietqr.io/img/WVN.png" });


                //_context.Database.ExecuteSqlRaw(sql);
                _context.DM_NganHang.AddRange(lstBank);
                _context.SaveChanges();
            }
        }
    }
}
