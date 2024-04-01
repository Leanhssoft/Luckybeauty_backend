using Abp.Application.Editions;
using Abp.Application.Features;
using BanHangBeautify.Editions;
using BanHangBeautify.Entities;
using BanHangBeautify.Features;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BanHangBeautify.EntityFrameworkCore.Seed.Host
{
    public class DefaultEditionCreator
    {
        private readonly SPADbContext _context;

        public DefaultEditionCreator(SPADbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateEditions();
        }

        private void CreateEditions()
        {
            var defaultEdition = _context.SubscribableEditions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
            if (defaultEdition == null)
            {
                defaultEdition = new SubscribableEdition { Name = EditionManager.DefaultEditionName, DisplayName = EditionManager.DefaultEditionName };
                _context.SubscribableEditions.Add(defaultEdition);

                _context.SaveChanges();

                /* Add desired features to the standard edition, if wanted... */
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatureConst.MaxBranchCount, "1");
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatureConst.MaxUserCount, "5");
            }
            if (defaultEdition.Id > 0)
            {
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatureConst.MaxBranchCount, "1");
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatureConst.MaxUserCount, "5");
            }
        }

        private void CreateFeatureIfNotExists(int editionId, string featureName, bool isEnabled)
        {
            if (_context.EditionFeatureSettings.IgnoreQueryFilters().Any(ef => ef.EditionId == editionId && ef.Name == featureName))
            {
                return;
            }

            _context.EditionFeatureSettings.Add(new EditionFeatureSetting
            {
                Name = featureName,
                Value = isEnabled.ToString(),
                EditionId = editionId
            });
            _context.SaveChanges();
        }
        private void CreateFeatureIfNotExists(int editionId, string featureName, string featureValue)
        {
            if (_context.EditionFeatureSettings.IgnoreQueryFilters().Any(ef => ef.EditionId == editionId && ef.Name == featureName))
            {
                return;
            }

            _context.EditionFeatureSettings.Add(new EditionFeatureSetting
            {
                Name = featureName,
                Value = featureValue,
                EditionId = editionId
            });
            _context.SaveChanges();
        }
    }
}
