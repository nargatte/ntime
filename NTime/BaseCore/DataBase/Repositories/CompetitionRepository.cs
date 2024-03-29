﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using BaseCore.Models;

namespace BaseCore.DataBase
{
    public class CompetitionRepository : Repository<Competition>
    {
        public CompetitionRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        public char DelimiterForExtraData { get; set; } = '|';

        protected override IQueryable<Competition> GetSortQuery(IQueryable<Competition> items) =>
            items.OrderByDescending(e => e.EventDate);

        protected override IQueryable<Competition> GetIncludeQuery(IQueryable<Competition> items) => 
            items.Include(competition => competition.ExtraColumns);


        public async Task<Competition> GetByRelatedEntityId<T>(int relatedEntityId)
            where T : class, ICompetitionId, IEntityId
        {
            Competition competition = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                competition = (await ctx.Set<T>().FirstOrDefaultAsync(i => i.Id == relatedEntityId))?.Competition;
            });
            return competition;
        }

        public override async Task<Competition> GetById(int id)
        {
            Competition item = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                item = await GetAllFullQuery(ctx.Set<Competition>()).AsNoTracking<Competition>().FirstOrDefaultAsync(i => i.Id == id);
                item.ExtraColumns = item.ExtraColumns
                    .OrderByDescending(column => column.SortIndex.HasValue)
                    .ThenBy(column => column.SortIndex.Value)
                    .ToArray();
            });
            return item;
        }


        public async Task<Competition[]> GetOpens()
        {
            Competition[] competitions = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                competitions = await ctx.Competitions.Where(c => c.SignUpEndDate > DateTime.Now).ToArrayAsync();
            });
            return competitions;
        }

        public async Task<bool> CanOrganizerEdit(string accountId, Competition competition)
        {
            bool b = false;
            await ContextProvider.DoAsync(async ctx =>
            {
                b = await ctx.Competitions.Where(c => c.Id == competition.Id)
                    .AllAsync(c => c.OrganizerAccounts.Any(oc => oc.AccountId == accountId));
            });
            return b;
        }

        public async Task<Competition[]> GetCompetitionsByOrganizer(string accountId)
        {
            Competition[] competitions = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                competitions =
                    await GetAllFullQuery(ctx.Competitions.Where(c =>
                        c.OrganizerAccounts.Any(o => o.AccountId == accountId))).AsNoTracking().ToArrayAsync();
            });
            return competitions;
        }

        public Task<PageViewModel<Competition>> GetCompetitionsByPlayerAccount(PlayerAccount playerAccount,
            PageBindingModel pageBindingModel) =>
            PageTemplate<Competition>(pageBindingModel,
                e => GetSortQuery(e.Where(c => c.Players.Any(p => p.PlayerAccountId == playerAccount.Id))));


        public async Task<IEnumerable<HeaderPermutationPair>> GetHeaderPermutationPairsAsync(int competitionId)
        {
            var competition = await GetById(competitionId);
            return competition == null ? null : GetHeaderPermutationPairs(competition.ExtraDataHeaders);
        }
        public IEnumerable<HeaderPermutationPair> GetHeaderPermutationPairs(string extraData)
        {
            if (String.IsNullOrWhiteSpace(extraData))
                return new List<HeaderPermutationPair>();
            var iterator = 0;
            return extraData.Split(DelimiterForExtraData).Select(headerName =>
                new HeaderPermutationPair { PermutationElement = iterator++, HeaderName = headerName });
        }
    }
}