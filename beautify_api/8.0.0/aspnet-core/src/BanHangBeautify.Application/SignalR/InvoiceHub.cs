using Abp.Dependency;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.SignalR
{
    public class InvoiceHub: Hub,ITransientDependency
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
    }
}
