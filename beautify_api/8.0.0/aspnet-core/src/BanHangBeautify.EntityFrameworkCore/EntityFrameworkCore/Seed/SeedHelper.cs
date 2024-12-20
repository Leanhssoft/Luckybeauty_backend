﻿using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using BanHangBeautify.EntityFrameworkCore.Seed.Custom;
using BanHangBeautify.EntityFrameworkCore.Seed.Host;
using BanHangBeautify.EntityFrameworkCore.Seed.LoaiHangHoa;
using BanHangBeautify.EntityFrameworkCore.Seed.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Transactions;

namespace BanHangBeautify.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<SPADbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(SPADbContext context)
        {
            context.SuppressAutoSetTenantId = true;
            context.Database.Migrate();

            // Host seed
            new InitialHostDbBuilder(context).Create();

            // Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();

            // Default DM_LoaiHangHoa seed
            new LoaiHangHoaBuilder(context).Create();

            //LoaiKhach seed
            new LoaiKhachBuilder(context).Create();

            //Setting tenants
            new DefaultTenantSettings(context).Create();

            new DMNganHangBuilder(context).InitDMNganHang(MultiTenancyConsts.DefaultTenantId);
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);
                    //context.Database.Migrate();
                    contextAction(context);
                    uow.Complete();
                }
            }
        }
    }
}
