using Abp.Dependency;
using Abp.Runtime.Session;
using BanHangBeautify.BaoKim;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SignalR
{
    public class InvoiceHub: Hub,ITransientDependency, IInvoiceHub
    {
        public IAbpSession AbpSession { get; set; }
        public InvoiceHub()
        {
            AbpSession = NullAbpSession.Instance;
        }
        public async Task ReloadInvoiceList()
        {
            await Clients.All.SendAsync("ReceiveInvoiceListReload",AbpSession.TenantId.HasValue?AbpSession.TenantId.Value.ToString():"null");
        }
        public async Task ThongBaoGiaDich_fromBaoKim(ResponseThongBaoGiaoDich data)
        {
            await Clients.All.SendAsync("ThongBaoGiaDich_fromBaoKim", data);
        }
    }
}
