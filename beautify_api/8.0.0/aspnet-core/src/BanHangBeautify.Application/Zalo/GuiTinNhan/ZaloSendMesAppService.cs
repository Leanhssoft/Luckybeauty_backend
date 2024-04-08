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
using System;
using BanHangBeautify.ZaloSMS_Common;

namespace BanHangBeautify.Zalo.GuiTinNhan
{
    public class ZaloSendMesAppService : SPAAppServiceBase, IZaloSendMes
    {
        private readonly IZalo_TemplateRepository _zaloTemplateRepo;
        private readonly ICommonZaloSMS _commonZaloSMS;
        public ZaloSendMesAppService(IZalo_TemplateRepository zaloTemplateRepo, ICommonZaloSMS commonZaloSMS)
        {
            _zaloTemplateRepo = zaloTemplateRepo;
            _commonZaloSMS = commonZaloSMS;
        }


        [HttpPost]
        public async Task<ResultMessageZaloDto> GuiTinTruyenThongorGiaoDich_fromDataDB(PageKhachHangSMSDto dataSend, string accessToken, Guid zaloTempId)
        {
            var tempItem = _zaloTemplateRepo.GetZaloTemplate_byId(zaloTempId);
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
                                lstElem.Add(new { type = item.ElementType, content = _commonZaloSMS.ReplaceContent(dataSend, item.Content) });
                            }
                            break;
                        case ZaloElementType.TABLE:
                            {
                                List<ZaloPayloadContent> lstContentTbl = new();
                                foreach (var itemTbl in item.tables)
                                {
                                    ZaloPayloadContent objNew = new ZaloPayloadContent { key = itemTbl.Key, value = _commonZaloSMS.ReplaceContent_Withkey(dataSend, itemTbl.Value) };
                                    lstContentTbl.Add(objNew);
                                }
                                // chỉ get key có giá trị != empty
                                lstContentTbl = lstContentTbl.Where(x => !string.IsNullOrEmpty(x.value)).ToList();
                                if(lstContentTbl.Count > 0)
                                {
                                    lstElem.Add(new { type = item.ElementType, content = lstContentTbl });
                                }
                            }
                            break;
                        //case ZaloElementType.IMAGE:// tin tư vấn kèm ảnh
                        //    {
                        //        lstElem.Add(new { media_type = item.ElementType, url = item.Content });
                        //    }
                            //break;
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
                                newBtn.payload = item.Title;
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
                case ZaloTemplateType.PROMOTION:// tin truyền thông
                    url = "https://openapi.zalo.me/v3.0/oa/message/promotion";
                    break;
                case ZaloTemplateType.TRANSACTION:// tin giao dịch
                case ZaloTemplateType.BOOKING:
                case ZaloTemplateType.PARTNERSHIP:
                case ZaloTemplateType.MEMBERSHIP:
                case ZaloTemplateType.EVENT:
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
