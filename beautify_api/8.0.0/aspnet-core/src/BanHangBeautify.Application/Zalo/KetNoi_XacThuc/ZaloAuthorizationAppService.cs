using Abp.Domain.Repositories;
using Azure.Core;
using BanHangBeautify.Configuration;
using BanHangBeautify.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.KhachHang.KhachHang.Dto;
using BanHangBeautify.SMS.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.KetNoi_XacThuc
{
    public class ZaloAuthorizationAppService : SPAAppServiceBase
    {
        private readonly IRepository<ZaloAuthorization, Guid> _zaloAuthorization;
        private readonly IConfiguration _config;
        private List<Zalo_Template> _lstZaloTemp;
        public ZaloAuthorizationAppService(IRepository<ZaloAuthorization, Guid> zaloAuthorization, IConfiguration config)
        {
            _zaloAuthorization = zaloAuthorization;
            _config = config;
            InnitData_TempZalo();
        }

        private Zalo_Template InitData_TempSinhNhat()
        {
            Guid zaloIdTemp = new("583D011C-78F5-48D4-A284-898441E00987");
            Zalo_Template zalotemp = new()
            {
                Id = zaloIdTemp,
                IdLoaiTin = ConstSMS.LoaiTin.SinhNhat,
                TemplateType = "promotion",
                Language = "VI"
            };

            Guid zaloIdElement = new("E6041AC0-806E-4835-8F21-DBD69CC6A170");
            List<Zalo_Element> lstElm = new()
            {
                new Zalo_Element
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = "header",
                    Content = "Const Content header",
                    IsImage = false,
                    ThuTuSapXep = 1,
                },
                new Zalo_Element
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = "text",
                    Content = @"<TenChiNhanh> kính chúc <TenKhachHang> có một ngày sinh nhật ý nghĩa bên người thân và gia đình",
                    IsImage = false,
                    ThuTuSapXep = 2,
                }
            };
            zalotemp.elements = lstElm;
            return zalotemp;
        }
        private Zalo_Template InitData_TempGiaoDich()
        {
            Guid zaloIdTemp = new("3ABE0F1B-D70E-4A1A-B5DE-2BC8CCD71514");
            Zalo_Template zalotemp = new()
            {
                Id = zaloIdTemp,
                IdLoaiTin = ConstSMS.LoaiTin.SinhNhat,
                TemplateType = "transaction_transaction",
                Language = "VI"
            };

            Guid zaloIdElement = new("F8A61D16-801F-4B31-8A60-EAAE3BE72CC7");

            List<Zalo_TableDetails> tables = new()
            {
                new Zalo_TableDetails
                {
                    Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Mã hóa đơn",
                    Value = "<MaHoaDon>",
                    ThuTuSapXep= 1,
                },
                new Zalo_TableDetails
                {
                     Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Ngày mua hàng",
                    Value = "<NgayLapHoaDon>",
                    ThuTuSapXep= 1,
                },
                new Zalo_TableDetails
                {
                     Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Key = "Tổng tiền",
                    Value = "<TongTienHang>",
                    ThuTuSapXep= 3,
                }
            };

            List<Zalo_Element> lstElm = new()
            {
                new Zalo_Element
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = "banner",
                    Content = @"https://drive.usercontent.google.com/download?id=1TDXeqE458lvu9DJXFg85FtBEuC_1OHUw&export=view&authuser=0",
                    IsImage = true,
                    ThuTuSapXep = 1,
                },
                new Zalo_Element
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = "header",
                    Content = @"Thông báo giao dịch",
                    IsImage = false,
                    ThuTuSapXep = 2,
                },
                new Zalo_Element
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = "text",
                    Content = @"Xin chào <TenKhachHang>, cảm ơn bạn đã mua hàng tại cửa hàng. Chúng tôi đã ghi nhận thanh toán của bạn với chi tiết như sau:",
                    IsImage = false,
                    ThuTuSapXep = 3,
                },
                new Zalo_Element
                {
                    Id = zaloIdElement,
                    IdTemplate = zaloIdTemp,
                    ElementType = "table",
                    IsImage = false,
                    ThuTuSapXep = 4,
                    tables = tables,
                }
            };

            List<Zalo_ButtonDetails> buttons = new()
            {
                new Zalo_ButtonDetails
                {
                    Id = Guid.NewGuid(),
                    IdElement = zaloIdElement,
                    Type = "oa.open.url",
                    Title = "Xem chi tiết đơn hàng",
                    Payload = "url...",
                    ThuTuSapXep= 1,
                },
            };
            zalotemp.elements = lstElm;
            zalotemp.buttons = buttons;

            return zalotemp;
        }
        [HttpGet]
        public List<Zalo_Template> InnitData_TempZalo()
        {
            var obj1 = InitData_TempSinhNhat();
            var obj2 = InitData_TempGiaoDich();
            List<Zalo_Template> lst = new()
            {
                obj1,
                obj2
            };
            _lstZaloTemp = lst;
            return _lstZaloTemp;
        }

        [HttpPost]
        public ZaloAuthorizationDto Insert(ZaloAuthorizationDto dto)
        {
            if (dto == null) { return new ZaloAuthorizationDto(); };
            ZaloAuthorization objNew = ObjectMapper.Map<ZaloAuthorization>(dto);
            objNew.Id = Guid.NewGuid();
            objNew.TenantId = AbpSession.TenantId ?? 1;
            objNew.CreationTime = DateTime.Now;
            objNew.CreatorUserId = AbpSession.UserId;
            objNew.IsDeleted = false;
            _zaloAuthorization.InsertAsync(objNew);
            var result = ObjectMapper.Map<ZaloAuthorizationDto>(objNew);
            return result;
        }
        [HttpPost]
        public async Task<string> Update(ZaloAuthorizationDto dto)
        {
            try
            {
                if (dto == null) { return "Data null"; };
                ZaloAuthorization objUp = await _zaloAuthorization.FirstOrDefaultAsync(dto.Id ?? Guid.Empty);
                if (objUp == null)
                {
                    return "object null";
                }
                //objUp.OAId = dto.CodeVerifier;// todo
                objUp.CodeVerifier = dto.CodeVerifier;
                objUp.CodeChallenge = dto.CodeChallenge;
                objUp.AuthorizationCode = dto.AuthorizationCode;
                objUp.AccessToken = dto.AccessToken;
                objUp.RefreshToken = dto.RefreshToken;
                objUp.ExpiresToken = dto.ExpiresToken;
                objUp.LastModificationTime = DateTime.Now;
                objUp.LastModifierUserId = AbpSession.UserId;
                await _zaloAuthorization.UpdateAsync(objUp);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Concat(ex.InnerException + ex.Message);
            }
        }

        [HttpGet]
        public async Task<string> XoaKetNoi(Guid Id)
        {
            try
            {
                ZaloAuthorization objUp = await _zaloAuthorization.FirstOrDefaultAsync(Id);
                if (objUp == null)
                {
                    return "object null";
                }
                objUp.DeleterUserId = AbpSession.TenantId ?? 1;
                objUp.DeletionTime = DateTime.Now;
                objUp.IsDeleted = true;
                await _zaloAuthorization.UpdateAsync(objUp);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Concat(ex.InnerException + ex.Message);
            }
        }
        /// <summary>
        /// mỗi refreshToken chỉ dùng 1 lần, sau đó sẽ không dùng dc nữa
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<ZaloToken> GetNewAccessToken_fromRefreshToken(string refreshToken)
        {
            try
            {
                string appId = _config["Zalo:AppId"];
                string appSecret = _config["Zalo:AppSecret"];
                //var json = JsonSerializer.Serialize(new { app_id = appId, refresh_token = refreshToken, grant_type = "refresh_token" });

                // Tạo nội dung của body
                Dictionary<string, string> postParams = new()
                {
                    { "app_id", appId },
                    { "refresh_token", refreshToken },
                    { "grant_type", "refresh_token" }
                };

                var body = new FormUrlEncodedContent(postParams).ReadAsStringAsync().Result;
                // Chuyển đổi body thành chuỗi x-www-form-urlencoded
                var stringContent = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

                HttpClient client = new();
                string url = $"https://oauth.zaloapp.com/v4/oa/access_token";

                using HttpRequestMessage request = new(HttpMethod.Post, url);
                request.Headers.Add("secret_key", appSecret);
                request.Content = stringContent;

                HttpResponseMessage response = client.SendAsync(request).Result;
                // Kiểm tra xem yêu cầu có thành công hay không
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var newToken = JsonSerializer.Deserialize<ZaloToken>(apiResponse);
                    return newToken;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GuiTinGiaoDich_fromDataDB(PageKhachHangSMSDto dataSend, string accessToken, byte? idLoaiTin)
        {
            Zalo_Template tempItem = _lstZaloTemp.Where(x => x.IdLoaiTin == idLoaiTin).FirstOrDefault();
            if (tempItem != null)
            {
                RequestData requestData = new();
                requestData.recipient = new Recipient { user_id = dataSend.ZOAUserId };

                //Payload payload = new();
                //payload.elements = new List<object>();

                List<object> lstElem = new List<object>();
                if (tempItem.elements.Count > 0)
                {
                    foreach (var item in tempItem.elements)
                    {
                        switch (item.ElementType)
                        {
                            case ZaloElementTypeType.BANNER:
                                {
                                    if (item.IsImage ?? false)
                                    {
                                        //payload.elements.Add(new { type = item.ElementType, image_url = item.Content });
                                        lstElem.Add(new { type = item.ElementType, image_url = item.Content });
                                    }
                                    else
                                    {
                                        //payload.elements.Add(new { type = item.ElementType, attachment_id = item.Content });
                                        lstElem.Add(new { type = item.ElementType, attachment_id = item.Content });
                                    }
                                }
                                break;
                            case ZaloElementTypeType.HEADER:
                            case ZaloElementTypeType.TEXT:
                                {
                                    lstElem.Add(new { type = item.ElementType, content = ReplaceContent(dataSend, item.Content) });
                                    //payload.elements.Add(new { type = item.ElementType, content = ReplaceContent(dataSend, item.Content)});
                                }
                                break;
                            case ZaloElementTypeType.TABLE:
                                {
                                    List<PayloadContent> lstContentTbl = new();
                                    foreach (var itemTbl in item.tables)
                                    {
                                        PayloadContent objNew = new PayloadContent { key = itemTbl.Key, value = ReplaceContent_Withkey(dataSend, itemTbl.Value) };
                                        lstContentTbl.Add(objNew);
                                    }
                                    //payload.elements.Add(new { type = item.ElementType, content = lstContentTbl });
                                    lstElem.Add(new { type = item.ElementType, content = lstContentTbl });
                                }
                                break;
                        }
                    }
                }

                if (tempItem.buttons != null)
                {
                    List<Button> lstBtn = new();
                    foreach (var item in tempItem.buttons)
                    {
                        Button newBtn = new() { title = item.Title, type = item.Type, image_icon = item.ImageIcon, payload = new ButtonPayload { url = item.Payload } };
                        lstBtn.Add(newBtn);
                    }

                    requestData.message = new()
                    {
                        attachment = new Attachment()
                        {
                            type = "template",
                            payload = new Payload()
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
                        attachment = new Attachment()
                        {
                            type = "template",
                            payload = new Payload()
                            {
                                template_type = tempItem.TemplateType,
                                language = tempItem.Language,
                                elements = lstElem,
                            }
                        }
                    };
                }

                // Chuyển đổi thành chuỗi JSON
                string jsonData = System.Text.Json.JsonSerializer.Serialize(requestData, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true // Để định dạng dữ liệu JSON
                });

                HttpClient client = new();
                string url = $@"https://openapi.zalo.me/v3.0/oa/message/{tempItem.TemplateType}";
                var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("access_token", accessToken);
                HttpResponseMessage response = await client.PostAsync(url, stringContent);
                string htmltext = await response.Content.ReadAsStringAsync();
                return htmltext;
            }
            return string.Empty;
        }

        protected string ReplaceContent_Withkey(PageKhachHangSMSDto cutomer, string key)
        {
            string txt = string.Empty;
            switch (key)
            {
                case "<TenKhachHang>":
                    txt = cutomer.TenKhachHang;
                    break;
                case "<NgaySinh>":
                    txt = cutomer.NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    break;
                case "<BookingDate>":
                    txt = cutomer.BookingDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    break;
                case "<ThoiGianHen>":
                    txt = cutomer.ThoiGianHen;
                    break;
                case "<TenDichVu>":
                    txt = cutomer.TenHangHoa;
                    break;
                case "<MaGiaoDich>":
                    txt = cutomer.MaHoaDon;
                    break;
                case "<NgayGiaoDich>":
                    txt = cutomer.NgayLapHoaDon?.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture);
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
            return txt;
        }
        protected string ReplaceContent(PageKhachHangSMSDto cutomer, string noiDungTin)
        {
            var ss = noiDungTin.Replace("<TenKhachHang>", cutomer.TenKhachHang);
            ss = ss.Replace("<NgaySinh>", cutomer.NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            ss = ss.Replace("<BookingDate>", cutomer.BookingDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            ss = ss.Replace("<ThoiGianHen>", cutomer.ThoiGianHen);
            ss = ss.Replace("<TenHangHoa>", cutomer.TenHangHoa);// dichvuhen
            ss = ss.Replace("<MaGiaoDich>", cutomer.MaHoaDon);
            ss = ss.Replace("<NgayGiaoDich>", cutomer.NgayLapHoaDon?.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
            ss = ss.Replace("<TenChiNhanh>", cutomer.TenChiNhanh);
            ss = ss.Replace("<SoDienThoaiChiNhanh>", cutomer.SoDienThoaiChiNhanh);
            ss = ss.Replace("<DiaChiChiNhanh>", cutomer.DiaChiChiNhanh);
            return ss;
        }
        /// <summary>
        /// Gửi tin nhắn tự động
        /// </summary>
        /// <param name="dataSend"></param>
        /// <param name="accessToken"></param>
        [HttpPost]
        public async Task<string> GuiTinTuVan(PageKhachHangSMSDto dataSend, string accessToken)
        {
            // Tạo đối tượng recipient
            var recipient = new
            {
                user_id = dataSend.ZOAUserId
            };

            // Tạo đối tượng elements
            var elements = new[]
            {
                new
                {
                    type = "header",
                    content = "Content Header"
                },
                new
                {
                    type = "text",
                    content = $@"{dataSend?.TenChiNhanh} chào khách hàng {dataSend?.TenKhachHang}"
                }
            };

            // Tạo đối tượng payload
            var payload = new
            {
                template_type = "promotion",
                language = "VI",
                elements
            };

            // Tạo đối tượng attachment
            var attachment = new
            {
                type = "template",
                payload
            };

            // Tạo đối tượng message
            var message = new
            {
                attachment
            };

            // Tạo đối tượng JSON chứa recipient và message
            var jsonData = new
            {
                recipient,
                message
            };

            // Chuyển đối tượng JSON dynamic thành chuỗi JSON và thực hiện gửi tin nhắn
            string jsonString = JsonSerializer.Serialize(jsonData);

            HttpClient client = new();
            const string url = "https://openapi.zalo.me/v3.0/oa/message/promotion";
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("access_token", accessToken);
            HttpResponseMessage response = await client.PostAsync(url, stringContent);
            string htmltext = await response.Content.ReadAsStringAsync();
            return htmltext;
        }

        [HttpPost]
        public async Task<string> GuiTinGiaoDich(PageKhachHangSMSDto dataSend, string accessToken)
        {
            // Tạo đối tượng recipient
            var recipient = new
            {
                user_id = dataSend.ZOAUserId
            };

            var payload = new
            {
                template_type = "promotion",
                elements = new object[]
            {
                new
                {
                    attachment_id = "aERC3A0iYGgQxim8fYIK6fxzsXkaFfq7ZFRB3RCyZH6RyziRis3RNydebK3iSPCJX_cJ3k1nW1EQufjN_pUL1f6Ypq3rTef5nxp6H_HnXKFDiyD5y762HS-baqRpQe5FdA376lTfq1sRyPr8ypd74ecbaLyA-tGmuJ-97W",
                    type = "banner"
                },
                new
                {
                    type = "header",
                    content = "💥💥Ưu đãi thành viên Platinum💥💥"
                },
                new
                {
                    type = "text",
                    align = "left",
                    content = "Ưu đãi dành riêng cho khách hàng Nguyen Van A hạng thẻ Platinum<br>Voucher trị giá 150$"
                },
                new
                {
                    type = "table",
                    content = new object[]
                    {
                        new
                        {
                            value = "VC09279222",
                            key = "Voucher"
                        },
                        new
                        {
                            value = "30/12/2023",
                            key = "Hạn sử dụng"
                        }
                    }
                },
                new
                {
                    type = "text",
                    align = "center",
                    content = "Áp dụng tất cả cửa hàng trên toàn quốc"
                }
            },
                buttons = new object[]
            {
                new
                {
                    title = "Tham khảo chương trình",
                    image_icon = "",
                    type = "oa.open.url",
                    payload = new
                    {
                        url = "https://oa.zalo.me/home"
                    }
                },
                new
                {
                    title = "Liên hệ chăm sóc viên",
                    image_icon = "aeqg9SYn3nIUYYeWohGI1fYRF3V9f0GHceig8Ckq4WQVcpmWb-9SL8JLPt-6gX0QbTCfSuQv40UEst1imAm53CwFPsQ1jq9MsOnlQe6rIrZOYcrlWBTAKy_UQsV9vnfGozCuOvFfIbN5rcXddFKM4sSYVM0D50I9eWy3",
                    type = "oa.query.hide",
                    payload = "#tuvan"
                }
            }
            };

            var message = new
            {
                attachment = new
                {
                    type = "template",
                    payload
                }
            };

            var requestData = new
            {
                recipient,
                message
            };

            // Chuyển đổi thành chuỗi JSON
            string jsonData = JsonSerializer.Serialize(requestData, new JsonSerializerOptions
            {
                WriteIndented = true // Để định dạng dữ liệu JSON
            });

            // Chuyển đối tượng JSON dynamic thành chuỗi JSON và thực hiện gửi tin nhắn
            string jsonString = JsonSerializer.Serialize(jsonData);

            HttpClient client = new();
            const string url = "https://openapi.zalo.me/v3.0/oa/message/promotion";
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("access_token", accessToken);
            HttpResponseMessage response = await client.PostAsync(url, stringContent);
            string htmltext = await response.Content.ReadAsStringAsync();
            return htmltext;
        }

        public async Task<string> SendMessageToUser(string accessToken)
        {
            dynamic objJson = new ExpandoObject();
            objJson.recipient = new ExpandoObject();
            objJson.recipient.user_id = "6441788310775550433";

            objJson.message = new ExpandoObject();
            objJson.message.text = "tét ábc";

            var json = JsonSerializer.Serialize(objJson);

            HttpClient client = new();
            const string url = "https://openapi.zalo.me/v3.0/oa/message/cs";
            StringContent content = new(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("access_token", accessToken);
            HttpResponseMessage response = await client.PostAsync(url, content);

            string htmltext = await response.Content.ReadAsStringAsync();
            return htmltext;
        }

        /// <summary>
        /// !!! luôn tạo codeVerifier, codeChallenge trước khi kết nối zalo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ZaloAuthorizationDto> Innit_orGetToken()
        {
            ZaloAuthorization lastObj = _zaloAuthorization.GetAllList().OrderByDescending(x => x.CreationTime).FirstOrDefault();
            if (lastObj != null)
            {
                // tinh thoihan expire_in: tu CreateTime - DateNow
                var dataReturn = ObjectMapper.Map<ZaloAuthorizationDto>(lastObj);
                var totalSecond = (decimal)(DateTime.Now - dataReturn.CreationTime).Value.TotalSeconds;
                dataReturn.IsExpiresAccessToken = totalSecond > 9000;
                if (totalSecond > 9000)
                {
                    // hết hạn access token: tạo access token từ refresh_token
                    var newToken = await GetNewAccessToken_fromRefreshToken(dataReturn.RefreshToken);
                    if (newToken != null && newToken.access_token != null)
                    {
                        var codeVerifier = GenerateCodeVerifier();
                        var codeChallenge = GenerateCodeChallenge(codeVerifier);
                        ZaloAuthorizationDto zaloToken = new()
                        {
                            AccessToken = newToken.access_token,
                            RefreshToken = newToken.refresh_token,
                            ExpiresToken = newToken.expires_in,
                            CodeVerifier = codeVerifier,
                            CodeChallenge = codeChallenge,
                        };
                        var data = Insert(zaloToken);
                        return data;
                    }
                    else
                    {
                        // token refreh không đúng --> tạo mới lại
                        return await CreateCodeVerifier_andCodeChallenge(); ;
                    }
                }
                return dataReturn;// chưa hết hạn access token
            }
            else
            {
                // nếu chưa có data: {CodeVerifier, odeChalleng}: tạo trước
                return await CreateCodeVerifier_andCodeChallenge();
            }
        }

        static string GenerateCodeVerifier()
        {
            // Define the characters that can be used in the code verifier
            const string allowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";

            // Set the length of the code verifier (should be between 43 and 128 characters)
            const int codeVerifierLength = 43;

            // Generate a random code verifier using a cryptographic random number generator
            byte[] randomBytes = new byte[codeVerifierLength];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            // Convert the random bytes to characters based on the allowed characters
            char[] codeVerifier = new char[codeVerifierLength];
            for (int i = 0; i < codeVerifierLength; i++)
            {
                int charIndex = randomBytes[i] % allowedCharacters.Length;
                codeVerifier[i] = allowedCharacters[charIndex];
            }

            // Return the generated code verifier as a string
            return new string(codeVerifier);
        }

        static string GenerateCodeChallenge(string codeVerifier)
        {
            // Convert the code verifier to bytes
            byte[] codeVerifierBytes = Encoding.ASCII.GetBytes(codeVerifier);

            // Compute the SHA-256 hash of the code verifier
            byte[] hashBytes = SHA256.HashData(codeVerifierBytes);

            // Encode the hash with Base64
            string codeChallenge = Convert.ToBase64String(hashBytes);

            // URL encode the Base64 string (replace '+' with '-', '/' with '_')
            codeChallenge = codeChallenge.Replace("+", "-").Replace("/", "_");

            return codeChallenge;
        }
        [HttpGet]
        public async Task<ZaloAuthorizationDto> CreateCodeVerifier_andCodeChallenge()
        {
            var codeVerifier = GenerateCodeVerifier();
            var codeChallenge = GenerateCodeChallenge(codeVerifier);
            ZaloAuthorizationDto zaloToken = new()
            {
                CodeVerifier = codeVerifier,
                CodeChallenge = codeChallenge,
            };
            return Insert(zaloToken);
        }
    }
}
