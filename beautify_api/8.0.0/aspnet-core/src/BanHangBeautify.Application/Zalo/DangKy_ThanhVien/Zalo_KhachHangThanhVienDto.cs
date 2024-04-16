using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Zalo.DangKyThanhVien
{
    public class Zalo_KhachHangThanhVienDto
    {
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public string ZOAUserId { get; set; }

        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "user_id_by_app")]
        public string UserIdByApp { get; set; }

        [JsonProperty(PropertyName = "user_is_follower")]
        public bool? UserIsFollower { get; set; }// khách hàng quan tâm/chưa quan tâm OA

        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }
    }
    //public class Zalo_KhachHangThanhVienDto2
    //{
    //    public string user_id { get; set; }
    //    public string user_alias { get; set; }
    //    public string display_name { get; set; }
    //    public string user_id_by_app { get; set; }
    //    public bool user_is_follower { get; set; }
    //    public bool is_sensitive { get; set; }
    //    public string avatar { get; set; }
    //}

    public class ResultDataZaloCommon<T>
    {
        public int error;
        public string message;
        public T data;
    } 
   
    public class ZaloUserShareInforDto
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string ward { get; set; }
    }
}
