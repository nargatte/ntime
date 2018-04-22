namespace BaseCore.DataBase
{
    public class SubcategoryDistance : IEntityId, ICompetitionId
    {
        public SubcategoryDistance()
        {
        }

        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }
        public int? SubcategoryId { get; set; }
        public virtual Subcategory Subcategory { get; set; }
        public int? DistanceId { get; set; }
        public virtual Distance Distance { get; set; }
    }
}