namespace BaseCore.DataBase
{
    public class AgeCategoryDistance : IEntityId, ICompetitionId
    {
        public AgeCategoryDistance()
        {
        }

        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }
        public int? AgeCategoryId { get; set; }
        public virtual AgeCategory AgeCategory { get; set; }
        public int? DistanceId { get; set; }
        public virtual Distance Distance { get; set; }
    }
}