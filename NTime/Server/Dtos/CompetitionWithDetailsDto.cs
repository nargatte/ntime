﻿using System.Linq;
using System.Threading.Tasks;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class CompetitionWithDetailsDto : CompetitionDto
    {
        public CompetitionWithDetailsDto()
        {
        }

        public CompetitionWithDetailsDto(Competition competition) : base(competition)
        {
        }

        public async Task SetDetails(Competition competition, IContextProvider contextProvider)
        {
            var ageCategoryRepository = new AgeCategoryRepository(contextProvider, competition);
            var distanceRepository = new DistanceRepository(contextProvider, competition);
            var subcategoryRepository = new SubcategoryRepository(contextProvider, competition);
            var ageCategoryDistanceRepository = new AgeCategoryDistanceRepository(contextProvider, competition);

            var ageCategoriesTask = ageCategoryRepository.GetAllAsync();
            var distancesTask = distanceRepository.GetAllAsync();
            var subcategoriesTask = subcategoryRepository.GetAllAsync();
            var ageCategoryDistancesTask = ageCategoryDistanceRepository.GetAllAsync();

            await Task.WhenAll(ageCategoriesTask, distancesTask, subcategoriesTask, ageCategoryDistancesTask);
            // TODO: change to Task.WhenAll - https://stackoverflow.com/questions/17197699/awaiting-multiple-tasks-with-different-results
            AgeCategories = (await ageCategoriesTask).Select(ag => new AgeCategoryDto(ag))
                .ToArray();
            Distances = (await distancesTask).Select(d => new DistanceDto(d)).ToArray();
            Subcategories = (await subcategoriesTask).Select(s => new SubcategoryDto(s)).ToArray();
            AgeCategoryDistances = (await ageCategoryDistancesTask)
                .Select(acd => new AgeCategoryDistanceDto(acd)).ToArray();
        }

        public AgeCategoryDto[] AgeCategories { get; set; }

        public DistanceDto[] Distances { get; set; }

        public SubcategoryDto[] Subcategories { get; set; }

        public AgeCategoryDistanceDto[] AgeCategoryDistances { get; set; }
    }
}