using System.Linq;
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
            AgeCategoryRepository ageCategoryRepository = new AgeCategoryRepository(contextProvider, competition);
            DistanceRepository distanceRepository = new DistanceRepository(contextProvider, competition);
            SubcategoryRepository subcategoryRepository = new SubcategoryRepository(contextProvider, competition);
            AgeCategoryDistanceRepository ageCategoryDistanceRepository = new AgeCategoryDistanceRepository(contextProvider, competition);

            AgeCategories = (await ageCategoryRepository.GetAllAsync()).Select(ag => new AgeCategoryDto(ag))
                .ToArray();
            Distances = (await distanceRepository.GetAllAsync()).Select(d => new DistanceDto(d)).ToArray();
            Subcategories = (await subcategoryRepository.GetAllAsync()).Select(s => new SubcategoryDto(s)).ToArray();
            AgeCategoryDistances = (await ageCategoryDistanceRepository.GetAllAsync())
                .Select(acd => new AgeCategoryDistanceDto(acd)).ToArray();
        }

        public AgeCategoryDto[] AgeCategories { get; set; }

        public DistanceDto[] Distances { get; set; }

        public SubcategoryDto[] Subcategories { get; set; }

        public AgeCategoryDistanceDto[] AgeCategoryDistances { get; set; }
    }
}