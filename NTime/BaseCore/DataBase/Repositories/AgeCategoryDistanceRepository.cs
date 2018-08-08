namespace BaseCore.DataBase
{
    public class AgeCategoryDistanceRepository : RepositoryCompetitionId<AgeCategoryDistance>
    {
        public AgeCategoryDistanceRepository(IContextProvider contextProvider, Competition competition) : base(contextProvider, competition)
        {
        }
    }
}