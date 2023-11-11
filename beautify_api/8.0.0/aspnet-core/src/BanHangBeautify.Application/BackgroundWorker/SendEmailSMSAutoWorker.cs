using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using BanHangBeautify.Configuration.Common;
using BanHangBeautify.Configuration.Common.Consts;
using BanHangBeautify.Entities;
using BanHangBeautify.SMS.Dto;
using BanHangBeautify.SMS.GuiTinNhan.Repository;
using System;
using System.Globalization;
using System.Linq;
using BanHangBeautify.SMS.ESMS;

namespace BanHangBeautify.BackgroundWorker
{
    public class SendEmailSMSAutoWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        public readonly IESMS _eSMS;
        public readonly IRepository<SMS_CaiDat_NhacNho, Guid> _caiDatNhacNho;
        public readonly IRepository<DM_ChiNhanh, Guid> _dmChiNhanh;

        public SendEmailSMSAutoWorker(AbpTimer timer,
            IESMS eSMS,
            IRepository<SMS_CaiDat_NhacNho, Guid> caiDatNhacNho,
            IRepository<DM_ChiNhanh, Guid> dmChiNhanh) : base(timer)
        {
            Timer.Period = 5000;
            _eSMS = eSMS;
            _caiDatNhacNho = caiDatNhacNho;
            _dmChiNhanh = dmChiNhanh;
        }

        [UnitOfWork]
        protected async override void DoWork()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var dtNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                // get allchinhanh of tenantId
                var chiNhanh = _dmChiNhanh.GetAll().Select(x => x.Id.ToString()).ToList();

                CommonClass.ParamSearch param = new()
                {
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    IdChiNhanhs = chiNhanh,
                    CurrentPage = 1,
                    PageSize = 10000,
                };
                // todo check idChiNhanh in setup
                var lstSetup = _caiDatNhacNho.GetAllList(x => x.IdLoaiTin == ConstSMS.LoaiTin.SinhNhat
                || x.IdLoaiTin == ConstSMS.LoaiTin.GiaoDich
                || x.IdLoaiTin == ConstSMS.LoaiTin.LichHen).Select(x => new { x.IdLoaiTin, x.NoiDungTin });
                if (lstSetup.Any())
                {
                    var lstLoaiTin = lstSetup.Select(x => x.IdLoaiTin);
                    ////// todo check CaiDat_NhacTuDong_ChiTiet: HinhThucGui(sms / zalo / gmail)
                    if (lstLoaiTin.Contains(ConstSMS.LoaiTin.SinhNhat))
                    {
                        string noidungTinMau = lstSetup.Select(x => x.NoiDungTin)?.FirstOrDefault()?.ToString();

                        EmailDto obj = new()
                        {
                            TieuDeEmail = "Chuc mung sinh nhat",
                            NoiDungEmail = "Chuc ban sinh nhat vui ve"
                        };

                        await _eSMS.SendSMS_ToListCustomer(param, ConstSMS.LoaiTin.SinhNhat, obj);
                        await _eSMS.SenEmail_ToListCustomer(param, ConstSMS.LoaiTin.SinhNhat, obj);

                    }
                    if (lstLoaiTin.Contains(ConstSMS.LoaiTin.LichHen))
                    {
                        EmailDto obj = new()
                        {
                            TieuDeEmail = "Lichhen",
                            NoiDungEmail = "ban co lich hen"
                        };

                        await _eSMS.SendSMS_ToListCustomer(param, ConstSMS.LoaiTin.LichHen, obj);
                        await _eSMS.SenEmail_ToListCustomer(param, ConstSMS.LoaiTin.LichHen, obj);
                    }
                    if (lstLoaiTin.Contains(ConstSMS.LoaiTin.GiaoDich))
                    {
                        EmailDto obj = new ()
                        {
                            TieuDeEmail = "Giao dich thanh cong",
                            NoiDungEmail = "Ban vua giao dich thanh cong tai cuahang"
                        };

                        await _eSMS.SendSMS_ToListCustomer(param, ConstSMS.LoaiTin.GiaoDich, obj);
                        await _eSMS.SenEmail_ToListCustomer(param, ConstSMS.LoaiTin.GiaoDich, obj);
                    }
                }
                CurrentUnitOfWork.SaveChanges();
            }
        }
    }
}
