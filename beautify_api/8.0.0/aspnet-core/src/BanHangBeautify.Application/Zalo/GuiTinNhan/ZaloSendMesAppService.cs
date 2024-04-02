using BanHangBeautify.SMS.Dto;
using BanHangBeautify.Zalo.ZaloTemplate;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using BanHangBeautify.Consts;
using System.Globalization;
using System.Text.RegularExpressions;
using BanHangBeautify.Zalo.DangKyThanhVien;

namespace BanHangBeautify.Zalo.GuiTinNhan
{
    public class ZaloSendMesAppService : SPAAppServiceBase, IZaloSendMes
    {
        private readonly IZalo_TemplateRepository _zaloTemplateRepo;
        public ZaloSendMesAppService(IZalo_TemplateRepository zaloTemplateRepo)
        {
            _zaloTemplateRepo = zaloTemplateRepo;
        }

        protected string ReplaceContent_Withkey(PageKhachHangSMSDto cutomer, string key)
        {
            string txt = string.Empty;
            Regex regex = new(@"^<.*>$");
            if (regex.IsMatch(key))
            {
                switch (key)
                {
                    case "<TenKhachHang>":
                        txt = cutomer.TenKhachHang;
                        break;
                    case "<SoDienThoai>":
                        txt = cutomer.SoDienThoai;
                        break;
                    case "<NgaySinh>":
                        txt = cutomer.NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        break;
                    case "<BookingDate>":
                        txt = cutomer.StartTime?.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture);
                        break;
                    case "<ThoiGianHen>":
                        txt = cutomer.ThoiGianHen;
                        break;
                    case "<TenDichVu>":
                        txt = cutomer.TenHangHoa;
                        break;
                    case "<MaHoaDon>":
                        txt = cutomer.MaHoaDon;
                        break;
                    case "<NgayLapHoaDon>":
                        txt = cutomer.NgayLapHoaDon?.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture);
                        break;
                    case "<TongTienHang>":
                        {
                            double number = cutomer.TongThanhToan ?? 0;
                            txt = number.ToString("N0", new CultureInfo("vi-VN"));
                        }
                        break;
                    case "<TenChiNhanh>":
                        txt = cutomer.TenChiNhanh;
                        break;
                    case "<SoDienThoaiChiNhanh>":
                        txt = cutomer.SoDienThoaiChiNhanh;
                        break;
                    case "<DiaChiChiNhanh>":
                        txt = cutomer.DiaChiChiNhanh;
                        break;
                }
            }
            return txt;
        }
        protected string ReplaceContent(PageKhachHangSMSDto cutomer, string noiDungTin)
        {
            var ss = noiDungTin.Replace("<TenKhachHang>", cutomer.TenKhachHang);
            ss = ss.Replace("<NgaySinh>", cutomer.NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            ss = ss.Replace("<SoDienThoai>", cutomer.SoDienThoai);
            ss = ss.Replace("<BookingDate>", cutomer.StartTime?.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture));
            ss = ss.Replace("<ThoiGianHen>", cutomer.ThoiGianHen);
            ss = ss.Replace("<TenHangHoa>", cutomer.TenHangHoa);// dichvuhen
            ss = ss.Replace("<MaGiaoDich>", cutomer.MaHoaDon);
            ss = ss.Replace("<NgayGiaoDich>", cutomer.NgayLapHoaDon?.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture));
            ss = ss.Replace("<TongTienHang>", (cutomer.TongThanhToan ?? 0).ToString("N0", new CultureInfo("vi-VN")));
            ss = ss.Replace("<TenChiNhanh>", cutomer.TenChiNhanh);
            ss = ss.Replace("<SoDienThoaiChiNhanh>", cutomer.SoDienThoaiChiNhanh);
            ss = ss.Replace("<DiaChiChiNhanh>", cutomer.DiaChiChiNhanh);
            return ss;
        }

        [HttpPost]
        public async Task<ResultMessageZaloDto> GuiTinGiaoDich_fromDataDB(PageKhachHangSMSDto dataSend, string accessToken, Zalo_TemplateDto tempItem)
        {
            ZaloRequestData requestData = new();
            requestData.recipient = new ZaloRecipient { user_id = dataSend.ZOAUserId };

            List<object> lstElem = new List<object>();
            if (tempItem.elements.Count > 0)
            {
                foreach (var item in tempItem.elements)
                {
                    switch (item.ElementType)
                    {
                        case ZaloElementType.BANNER:
                            {
                                if (item.IsImage ?? false)
                                {
                                    lstElem.Add(new { type = item.ElementType, image_url = item.Content });
                                }
                                else
                                {
                                    lstElem.Add(new { type = item.ElementType, attachment_id = item.Content });
                                }
                            }
                            break;
                        case ZaloElementType.HEADER:
                        case ZaloElementType.TEXT:
                            {
                                lstElem.Add(new { type = item.ElementType, content = ReplaceContent(dataSend, item.Content) });
                            }
                            break;
                        case ZaloElementType.TABLE:
                            {
                                List<ZaloPayloadContent> lstContentTbl = new();
                                foreach (var itemTbl in item.tables)
                                {
                                    ZaloPayloadContent objNew = new ZaloPayloadContent { key = itemTbl.Key, value = ReplaceContent_Withkey(dataSend, itemTbl.Value) };
                                    lstContentTbl.Add(objNew);
                                }
                                // chỉ get key có giá trị != empty
                                lstContentTbl = lstContentTbl.Where(x => !string.IsNullOrEmpty(x.value)).ToList();
                                lstElem.Add(new { type = item.ElementType, content = lstContentTbl });
                            }
                            break;
                    }
                }
            }

            if (tempItem.buttons != null)
            {
                List<ZaloButton> lstBtn = new();
                foreach (var item in tempItem.buttons)
                {
                    ZaloButton newBtn = new()
                    {
                        title = item.Title,
                        type = item.Type,
                        image_icon = item.ImageIcon,
                    };

                    switch (item.Type)
                    {
                        case ZaloButtonType.URL:
                            {
                                newBtn.payload = new { url = item.Payload };
                            }
                            break;
                        case ZaloButtonType.PHONE:
                            {
                                newBtn.payload = new { phone_code = item.Payload };
                            }
                            break;
                        case ZaloButtonType.SHOW:
                            {
                                newBtn.payload = item.Payload;
                            }
                            break;
                    }
                    lstBtn.Add(newBtn);
                }

                requestData.message = new()
                {
                    attachment = new ZaloAttachment()
                    {
                        type = "template",
                        payload = new ZaloPayload()
                        {
                            template_type = tempItem.TemplateType,
                            language = tempItem.Language,
                            elements = lstElem,
                            buttons = lstBtn,
                        }
                    }
                };
            }
            else
            {
                requestData.message = new()
                {
                    attachment = new ZaloAttachment()
                    {
                        type = "template",
                        payload = new ZaloPayload()
                        {
                            template_type = tempItem.TemplateType,
                            language = tempItem.Language,
                            elements = lstElem,
                        }
                    }
                };
            }

            // Chuyển đổi thành chuỗi JSON
            string jsonData = JsonSerializer.Serialize(requestData, new JsonSerializerOptions
            {
                WriteIndented = true,// Để định dạng dữ liệu JSON
                IgnoreNullValues = true // xóa thuộc tính nếu giá trị = null
            });
            HttpClient client = new();
            string url = string.Empty;
            switch (tempItem.TemplateType)
            {
                case ZaloTemplateType.PROMOTION:
                    url = "https://openapi.zalo.me/v3.0/oa/message/promotion";
                    break;
                case ZaloTemplateType.TRANSACTION:
                case ZaloTemplateType.BOOKING:
                    url = "https://openapi.zalo.me/v3.0/oa/message/transaction";
                    break;
            }
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("access_token", accessToken);
            HttpResponseMessage response = await client.PostAsync(url, stringContent);
            string htmltext = await response.Content.ReadAsStringAsync();

            var dataMes = JsonSerializer.Deserialize<ResultMessageZaloDto>(htmltext);
            return dataMes;
        }
    }
}
