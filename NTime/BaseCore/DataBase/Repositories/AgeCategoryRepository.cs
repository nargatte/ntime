using BaseCore.Csv;
using BaseCore.Csv.Map;
using BaseCore.Csv.Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseCore.DataBase
{
    public class AgeCategoryRepository : RepositoryCompetitionId<AgeCategory>
    {
        public AgeCategoryRepository(IContextProvider contextProvider, Competition competition) : base(contextProvider, competition)
        {
        }

        protected override IQueryable<AgeCategory> GetSortQuery(IQueryable<AgeCategory> items) =>
            items.OrderBy(e => e.YearFrom);

        public async Task<AgeCategory[]> GetFittingAsync(Player player)
        {
            AgeCategory[] ret = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                ret = await GetAllQuery(ctx.AgeCategories).AsNoTracking().Where(i => i.YearFrom <= player.BirthDate.Year && player.BirthDate.Year <= i.YearTo && player.IsMale == i.Male).ToArrayAsync();
            });
            return ret;
        }

        public Task AddFormCollection(AgeCategoryTemplate ageCategoryCollection)
        {
            return ContextProvider.DoAsync(async ctx =>
            {
                AgeCategoryTemplateItem[] ageCategoryTemplates =
                    await ctx.AgeCategoryTemplateItems
                        .Where(e => e.AgeCategoryCollectionId == ageCategoryCollection.Id).ToArrayAsync();
                AgeCategory[] ageCategories = ageCategoryTemplates
                    .Select(e => new AgeCategory(e.Name, e.YearFrom, e.YearTo, e.Male) { CompetitionId = Competition.Id })
                    .ToArray();
                ctx.AgeCategories.AddRange(ageCategories);
                await ctx.SaveChangesAsync();
            });
        }

        public Task SaveAsCollection(AgeCategoryTemplate ageCategoryCollection)
        {
            return ContextProvider.DoAsync(async ctx =>
            {
                using (DbContextTransaction contextTransaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        ageCategoryCollection.AgeCategoryTemplates = null;
                        ctx.AgeCategoryTemplates.Add(ageCategoryCollection);
                        await ctx.SaveChangesAsync();

                        AgeCategory[] ageCategories = await GetAllQuery(ctx.AgeCategories).ToArrayAsync();
                        AgeCategoryTemplateItem[] ageCategoryTemplates =
                            ageCategories
                                .Select(a => new AgeCategoryTemplateItem(a.Name, a.YearFrom, a.YearTo, a.Male)
                                {
                                    AgeCategoryCollectionId = ageCategoryCollection.Id
                                }).ToArray();
                        ctx.AgeCategoryTemplateItems.AddRange(ageCategoryTemplates);
                        await ctx.SaveChangesAsync();

                        contextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        contextTransaction.Rollback();
                    }
                }
            });
        }

        public override Task RemoveAsync(AgeCategory item)
        {
            CheckNull(item);
            CheckItem(item);
            return ContextProvider.DoAsync(async ctx =>
            {
                ctx.AgeCategories.Attach(item);
                await ctx.Players.Where(p => p.AgeCategoryId == item.Id).ForEachAsync(p =>
                {
                    p.AgeCategory = null;
                    p.AgeCategoryId = null;
                });
                await ctx.AgeCategoryDistances.Where(acd => acd.AgeCategoryId == item.Id).ForEachAsync(acd =>
                {
                    ctx.AgeCategoryDistances.Remove(acd);
                });
                ctx.AgeCategories.Remove(item);
                await ctx.SaveChangesAsync();
            });
        }
        public async Task<bool> IsAgeCategoryAndDistanceMatch(AgeCategory ageCategory, Distance distance)
        {
            bool answer = false;
            await ContextProvider.DoAsync(async ctx =>
            {
                answer = await ctx.AgeCategoryDistances.AnyAsync(acd =>
                    acd.CompetitionId == Competition.Id && acd.AgeCategoryId == ageCategory.Id &&
                    acd.DistanceId == distance.Id);
            });
            return answer;
        }

        /// <summary>
        /// To edit when AgeCategoryDistances are implemented properly
        /// </summary>
        /// <param name="ageCategories"></param>
        /// <param name="distances"></param>
        /// <returns></returns>
        public async Task<bool> ExportAgeCategoriesToCsv(IEnumerable<AgeCategory> ageCategories, IEnumerable<Distance> distances)
        {
            // !!! Read above
            var exportableAgeCategories = new List<AgeCategoryRecord>();
            foreach (var ageCategory in ageCategories)
            {
                foreach (var distance in distances)
                {
                    exportableAgeCategories.Add(new AgeCategoryRecord(ageCategory, distance.Name));
                }
            }
            var actualPath = "";
            var dialog = new SaveFileDialog
            {
                Filter = "csv files (*.csv)|*.csv",
                RestoreDirectory = true,
                Title = "Wybierz gdzie zapisać plik"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(dialog.FileName))
                {
                    actualPath = dialog.FileName;
                }
                else return false;
            }

            var exporter = new CsvExporter<AgeCategoryRecord, AgeCategoryMap>(actualPath, _defaultCsvDelimiter);
            return await exporter.SaveAllRecordsAsync(exportableAgeCategories);
        }

        public async Task<IEnumerable<AgeCategory>> ImportAgeCategoriesFromCsv()
        {
            var fileName = "";
            var dialog = new OpenFileDialog
            {
                Filter = "csv files (*.csv)|*.csv",
                RestoreDirectory = true,
                Title = "Wybierz w dobrej kolejności wyniki dla cyklu",
                Multiselect = false
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileName = dialog.FileName;
            }
            else
            {
                MessageBox.Show("Nie wybrano żadnych zawodów");
                return null;
            }
            var importer = new CsvImporter<AgeCategoryRecord, AgeCategoryMap>(fileName, _defaultCsvDelimiter);
            var ageCategories = await importer.GetAllRecordsAsync();
            if (ageCategories == null)
                return null;
            return ageCategories.Select(record => new AgeCategory(record));
        }

    }
}