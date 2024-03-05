﻿using Abp.Application.Services;
using Asd.AbpZeroTemplate.DashboardCustomization;
using Asd.AbpZeroTemplate.DashboardCustomization.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BanHangBeautify.DashboardCustomization
{
    public interface IDashboardCustomizationAppService : IApplicationService
    {
        Task<Dashboard> GetUserDashboard(GetDashboardInput input);

        Task SavePage(SavePageInput input);

        Task RenamePage(RenamePageInput input);

        Task<AddNewPageOutput> AddNewPage(AddNewPageInput input);

        Task<Widget> AddWidget(AddWidgetInput input);

        Task DeletePage(DeletePageInput input);

        DashboardOutput GetDashboardDefinition(GetDashboardInput input);

        List<WidgetOutput> GetAllWidgetDefinitions(GetDashboardInput input);
    }
}
