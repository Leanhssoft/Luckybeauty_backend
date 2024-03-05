using System;
using System.Threading.Tasks;

namespace BanHangBeautify.AppDanhMuc.AppCuaHang
{
    public interface ICuaHangAppService
    {
        public Task CreateCuaHangWithTenant(string tenCuaHang, int IdTenant);
        public Task<bool> DeleteCongTy(Guid id);
    }
}
