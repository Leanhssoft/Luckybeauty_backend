using BanHangBeautify.NhanSu.NhanVien_DichVu.Dto;
using System.Threading.Tasks;

namespace BanHangBeautify.NhanSu.NhanVien_DichVu
{
    public interface INhanVienDichVuAppService
    {
        public Task<ExecuteResultDto> CreateOrUpdateEmployeeByService(CreateManyEmployeeDto input);
        public Task<ExecuteResultDto> CreateOrUpdateServicesByEmployee(CreateServiceManyDto input);
    }
}