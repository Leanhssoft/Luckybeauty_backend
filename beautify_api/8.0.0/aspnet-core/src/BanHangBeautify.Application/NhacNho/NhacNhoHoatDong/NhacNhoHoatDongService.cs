using Abp;
using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Net.Mail;
using Abp.Notifications;
using Abp.Runtime.Session;
using AutoMapper.Internal.Mappers;
using BanHangBeautify.Authorization.Users;
using BanHangBeautify.Entities;
using BanHangBeautify.MultiTenancy;
using BanHangBeautify.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.NhacNho.NhacNhoHoatDong
{
    public class NhacNhoHoatDongService :SPAAppServiceBase, INhacNhoHoatDongService
    {

        private IRepository<Tenant> _tenantRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private IRepository<User, long> _userRepository;
        private readonly IRepository<HT_NhatKyThaoTac, Guid> _nhatKyThaoTacRepository;
        private readonly IEmailSender _emailSender;
        private readonly INotificationAppService _notificationAppService;
        public NhacNhoHoatDongService(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<Tenant> tenantRepository,
            IRepository<User, long> userRepository,
            IRepository<HT_NhatKyThaoTac, Guid> nhatKyThaoTacRepository,
            IEmailSender emailSender,
            INotificationAppService notificationAppService
            )
        {
            _unitOfWorkManager = unitOfWorkManager;
            _userRepository = userRepository;
            _nhatKyThaoTacRepository = nhatKyThaoTacRepository;
            _tenantRepository = tenantRepository;
            _emailSender = emailSender;
            _notificationAppService = notificationAppService;
        }

        public async Task SendEmailRemindActivity()
        {
            using var unitOfWork = _unitOfWorkManager.Begin();
            var tenantIds = _tenantRepository.GetAll().Select(x => x.Id).ToList();
            if (tenantIds != null && tenantIds.Count() > 0)
            {

                foreach (var id in tenantIds)
                {
                    using (_unitOfWorkManager.Current.SetTenantId(id))
                    {

                        var hoatDongGanNhat = _nhatKyThaoTacRepository.GetAll().OrderByDescending(x => x.CreationTime).Take(1).FirstOrDefault();
                        var now = DateTime.Now;
                        if (now.Subtract(hoatDongGanNhat.CreationTime).TotalDays >= 7)
                        {
                            var users = _userRepository.GetAll().Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                            if (users != null && users.Count() > 0)
                            {
                                foreach (var item in users)
                                {
                                    var emailContent = new StringBuilder();
                                    emailContent.AppendLine("<div><h1>" + "Xin chào, " + item.FullName + "</h1></div>");
                                    emailContent.AppendLine("<div><span>" + "Hy vọng bạn đang cảm thấy tốt vào ngày hôm nay." + "</span></div><br/>");
                                    emailContent.AppendLine(string.Format("<div><span>" + "Chúng tôi nhận thấy rằng tài khoản của bạn trên " +
                                        "<a href='login.luckybeauty.vn'>login.luckybeauty.vn</a> đã lâu không hoạt động. " +
                                        "Chúng tôi rất mong muốn thấy bạn trở lại và tận hưởng các tính năng và dịch vụ mà chúng tôi cung cấp." + "</span></div>"));
                                    emailContent.AppendLine(string.Format("<div><span>" + "Để đảm bảo rằng bạn không bỏ lỡ bất kỳ thông tin hoặc ưu đãi đặc biệt nào, " +
                                        "chúng tôi muốn nhắc nhở bạn kiểm tra lại tài khoản và cập nhật thông tin của mình." + "</span></div>"));
                                    emailContent.AppendLine(string.Format("<div><span>" + "Nếu có bất kỳ câu hỏi nào hoặc cần hỗ trợ từ chúng tôi, " +
                                        "vui lòng đừng ngần ngại liên hệ với đội ngũ dịch vụ khách hàng của chúng tôi." + "</span></div>"));
                                    emailContent.AppendLine(string.Format("<div><span>" + "Chúng tôi rất mong đợi được phục vụ bạn một lần nữa và" +
                                        " chúng tôi cảm ơn sự ủng hộ của bạn đối với <a href='login.luckybeauty.vn'>login.luckybeauty.vn</a>" + "</span></div><br/>"));
                                    emailContent.AppendLine("<div><span>" + "Trân trọng." + "</span></div><br/>");
                                    emailContent.AppendLine("<div><span>Công ty cổ phần công nghệ phần mềm SSOFT VIỆT NAM</span></div>");
                                    _emailSender.Send(new MailMessage
                                    {
                                        To = { item.EmailAddress },
                                        Subject = "Nhắc nhở hoạt động",
                                        Body = emailContent.ToString(),
                                        IsBodyHtml = true,
                                    });
                                }
                            }
                        }

                    }
                }
            }
        }

        public async Task SendNotoficationContractExpired()
        {
            using var unitOfWork = _unitOfWorkManager.Begin();
            var tenants = _tenantRepository.GetAll().ToList();
            if (tenants != null && tenants.Count() > 0)
            {

                foreach (var tenant in tenants)
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenant.Id))
                    {
                        var now = DateTime.Now;
                        if (tenant.SubscriptionEndDate.HasValue)
                        {
                            if (tenant.SubscriptionEndDate.Value.Subtract(now).TotalDays <= 7 && tenant.SubscriptionEndDate.Value.Subtract(now).TotalDays>=1)
                            {
                                var users = _userRepository.GetAll().Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                                var listUser =new  List<UserIdentifier>();
                                foreach (var user in users)
                                {
                                    UserIdentifier rdo = new UserIdentifier(tenantId:user.TenantId,userId: user.Id);
                                    listUser.Add(rdo);
                                }
                                string mess = string.Format("Hợp đồng sử dụng của quý khách sắp hết hạn vào ngày {0}! " +
                                    "Vui lòng liên hệ cho quản trị viên hoặc qua số điện thoại 0247.303.9333/0936.363.069 để tiếp tục gia hạn sử dụng phần mềm. SSOFT hân hạnh hỗ trợ.",tenant.SubscriptionEndDate.Value.ToString("dd-MM-yyyy HH:mm"));
                                var messageNotiData = _notificationAppService.SetLocalizableMessageNotificationData(mess);
                                await _notificationAppService.SendMessageAsync("Cảnh báo gia hạn hợp đồng!", messageNotiData,listUser, NotificationSeverity.Warn);
                            }
                        }
                    }
                }
            }
        }
    }
}
