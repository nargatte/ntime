namespace BaseCore.DataBase
{
    public interface ICompetitionId
    {
        int CompetitionId { get; set; }
        Competition Competition { get; set; }
    }
}