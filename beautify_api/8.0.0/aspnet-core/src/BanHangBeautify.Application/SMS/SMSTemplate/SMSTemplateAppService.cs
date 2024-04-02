using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using BanHangBeautify.Entities;
using BanHangBeautify.SMS.SMSTemplate.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SMS.SMSTemplate
{
    [AbpAuthorize]
    public class SMSTemplateAppService : SPAAppServiceBase
    {
        IRepository<SMS_Template, Guid> _repository;
        public SMSTemplateAppService(IRepository<SMS_Template, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<ExecuteResultDto> CreateOrEdit(CreateOrEditSMSTemplateDto input)
        {

            var checkExist = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (checkExist != null)
            {
                return await Update(checkExist, input);
            }
            return await Create(input);
        }
        public async Task<ExecuteResultDto> Create(CreateOrEditSMSTemplateDto input)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                SMS_Template smsTemp = new SMS_Template();
                smsTemp.Id = Guid.NewGuid();
                smsTemp.TenMauTin = input.TenMauTin;
                smsTemp.NoiDungTinMau = input.NoiDungTinMau;
                smsTemp.IdLoaiTin = input.IdLoaiTin;
                smsTemp.TrangThai = input.TrangThai;
                smsTemp.LaMacDinh = input.LaMacDinh;
                smsTemp.CreationTime = DateTime.Now;
                smsTemp.CreatorUserId = AbpSession.UserId;
                smsTemp.TenantId = AbpSession.TenantId ?? 1;
                await _repository.InsertAsync(smsTemp);
                result.Status = "success";
                result.Message = "Thêm mới thành công!";
            }
            catch (Exception ex)
            {
                result.Status = "error";
                result.Message = "Có lỗi xảy ra vui lòng thử lại!";
                result.Detail = ex.Message;
            }
            return result;
        }
        public async Task<ExecuteResultDto> Update(SMS_Template oldData, CreateOrEditSMSTemplateDto input)
        {
            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                oldData.NoiDungTinMau = input.NoiDungTinMau;
                oldData.IdLoaiTin = input.IdLoaiTin;
                oldData.TrangThai = input.TrangThai;
                oldData.LaMacDinh = input.LaMacDinh;
                oldData.LastModificationTime = DateTime.Now;
                oldData.LastModifierUserId = AbpSession.UserId;
                await _repository.UpdateAsync(oldData);
                result.Status = "success";
                result.Message = "Cập nhật thành công!";
            }
            catch (Exception ex)
            {
                result.Status = "error";
                result.Message = "Có lỗi xảy ra vui lòng thử lại!";
                result.Detail = ex.Message;
            }
            return result;
        }
        public async Task<CreateOrEditSMSTemplateDto> GetForEdit(Guid id)
        {
            CreateOrEditSMSTemplateDto result = new CreateOrEditSMSTemplateDto();
            var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (data != null)
            {
                result.Id = id;
                result.TenMauTin = data.TenMauTin;
                result.NoiDungTinMau = data.NoiDungTinMau;
                result.IdLoaiTin = data.IdLoaiTin;
                result.TrangThai = data.TrangThai;
                result.LaMacDinh = data.LaMacDinh;
            }
            return result;
        }
        [HttpPost]
        public async Task<ExecuteResultDto> Delete(Guid id)
        {

            ExecuteResultDto result = new ExecuteResultDto();
            try
            {
                var data = await _repository.FirstOrDefaultAsync(x => x.Id == id);
                if (data != null)
                {
                    await _repository.DeleteAsync(data);
                    result.Status = "success";
                    result.Message = "Xóa dữ liệu thành công!";
                }
                else
                {
                    result.Status = "error";
                    result.Message = "Bản ghi không tồn tại!";
                }
            }
            catch (Exception ex)
            {
                result.Status = "error";
                result.Message = "Có lỗi xảy ra vui lòng thử lại!";
                result.Detail = ex.Message;
            }
            return result;
        }
        public async Task<PagedResultDto<SMSTemplateViewDto>> GetAll(PagedRequestDto input)
        {
            PagedResultDto<SMSTemplateViewDto> result = new PagedResultDto<SMSTemplateViewDto>();
            try
            {
                input.Keyword = string.IsNullOrEmpty(input.Keyword) ? "" : input.Keyword;
                input.SkipCount = input.SkipCount > 1 ? (input.SkipCount - 1) * input.MaxResultCount : 0;
                var listData = _repository.GetAll().OrderByDescending(x => x.CreationTime).ToList();
                result.TotalCount = listData.Count();
                listData = listData.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                List<SMSTemplateViewDto> items = new List<SMSTemplateViewDto>();
                foreach (var item in listData)
                {
                    SMSTemplateViewDto rdo = new SMSTemplateViewDto();
                    rdo.Id = item.Id;
                    rdo.TenMauTin = item.TenMauTin;
                    rdo.NoiDungTinMau = item.NoiDungTinMau;
                    rdo.IdLoaiTin = int.Parse(item.IdLoaiTin.Value.ToString() ?? "0");
                    items.Add(rdo);
                }
                result.Items = items;

            }
            catch (Exception)
            {
                result.Items = new List<SMSTemplateViewDto>();
                result.TotalCount = 0;
            }
            return result;
        }
    }
}
