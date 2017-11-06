namespace BaseCore.DataBase
{
    public class AgeCategoryTemplate : AgeCategoryBase
    {
        public AgeCategoryTemplate()
        {
        }

        public AgeCategoryTemplate(string name, int yearFrom, int yearTo) : base(name, yearFrom, yearTo)
        {
        }

        public int AgeCategoryCollectionId { get; set; }
        public virtual AgeCategoryCollection AgeCategoryCollection { get; set; }
    }
}