using Abp;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.UI;
using BanHangBeautify.Authorization;
using BanHangBeautify.Editions.Dto;
using BanHangBeautify.Entities;
using BanHangBeautify.MultiTenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Editions
{
    public class EditionAppService:SPAAppServiceBase
    {
        private readonly EditionManager _editionManager;
        private readonly IRepository<SubscribableEdition> _editionRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IBackgroundJobManager _backgroundJobManager;
        public EditionAppService(EditionManager editionManager,
            IRepository<SubscribableEdition> editionRepository,
            IRepository<Tenant> tenantRepository,
            IBackgroundJobManager backgroundJobManager)
        {
            _editionManager = editionManager;
            _editionRepository = editionRepository;
            _tenantRepository = tenantRepository;
            _backgroundJobManager = backgroundJobManager;
        }
        [AbpAuthorize(PermissionNames.Pages_Editions_Create, PermissionNames.Pages_Editions_Edit)]
        public async Task<GetEditionEditOutput> GetEditionForEdit(NullableIdDto input)
        {
            var features = FeatureManager.GetAll()
                .Where(f => f.Scope.HasFlag(FeatureScopes.Edition));

            EditionEditDto editionEditDto;
            List<NameValue> featureValues;

            if (input.Id.HasValue) //Editing existing edition?
            {
                var edition = await _editionManager.FindByIdAsync(input.Id.Value);
                featureValues = (await _editionManager.GetFeatureValuesAsync(input.Id.Value)).ToList();
                editionEditDto = ObjectMapper.Map<EditionEditDto>(edition);
            }
            else
            {
                editionEditDto = new EditionEditDto();
                featureValues = features.Select(f => new NameValue(f.Name, f.DefaultValue)).ToList();
            }

            var featureDtos = ObjectMapper.Map<List<FlatFeatureDto>>(features).OrderBy(f => f.DisplayName).ToList();

            return new GetEditionEditOutput
            {
                Edition = editionEditDto,
                Features = featureDtos,
                FeatureValues = featureValues.Select(fv => new NameValueDto(fv)).ToList()
            };
        }

        [AbpAuthorize(PermissionNames.Pages_Editions_Create)]
        [HttpPost]
        public async Task CreateEdition(CreateEditionDto input)
        {
            var edition = ObjectMapper.Map<SubscribableEdition>(input.Edition);
            await _editionManager.CreateAsync(edition);
            await CurrentUnitOfWork.SaveChangesAsync(); //It's done to get Id of the edition.

            await SetFeatureValues(edition, input.FeatureValues);
        }
        [AbpAuthorize(PermissionNames.Pages_Editions_Edit)]
        [HttpPost]
        public async Task UpdateEdition(UpdateEditionDto input)
        {
            if (input.Edition.Id != null)
            {
                var edition = await _editionManager.GetByIdAsync(input.Edition.Id.Value);

                edition.DisplayName = input.Edition.DisplayName;

                await SetFeatureValues(edition, input.FeatureValues);
            }
        }
        [AbpAuthorize(PermissionNames.Pages_Editions_Delete)]
        [HttpPost]
        public async Task DeleteEdition(EntityDto input)
        {
            var tenantCount = await _tenantRepository.CountAsync(t => t.EditionId == input.Id);
            if (tenantCount > 0)
            {
                throw new UserFriendlyException(L("ThereAreTenantsSubscribedToThisEdition"));
            }

            var edition = await _editionManager.GetByIdAsync(input.Id);
            await _editionManager.DeleteAsync(edition);
        }
        [AbpAuthorize(PermissionNames.Pages_Editions)]
        public async Task<ListResultDto<EditionListDto>> GetEditions()
        {
            var editions = await _editionRepository.GetAll().ToListAsync();

            var result = new List<EditionListDto>();

            foreach (var edition in editions)
            {
                var resultEdition = ObjectMapper.Map<EditionListDto>(edition);
                result.Add(resultEdition);
            }

            return new ListResultDto<EditionListDto>(result);
        }
        private Task SetFeatureValues(Edition edition, List<NameValueDto> featureValues)
        {
            return _editionManager.SetFeatureValuesAsync(edition.Id,
                featureValues.Select(fv => new NameValue(fv.Name, fv.Value)).ToArray());
        }
    }
}
