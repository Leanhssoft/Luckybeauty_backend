using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Users.Dto
{
    public class ProfileDto
    {
        public long Id { get; set; }
        public Guid? NhanSuId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string NgaySinh { get; set; }
        public string CCCD { get; set; }
        public byte? GioiTinh { get; set; }
        public string Avatar { get; set; }
        public string NgayCap { get; set; }
        public string NoiCap { get; set; }
        public string DiaChi { get; set; }
        public string Password { get; set; }
    }
}
