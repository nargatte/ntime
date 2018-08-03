namespace BaseCore.DataBase
{
    public class AgeCategoryTemplate : AgeCategoryBase
    {
        public AgeCategoryTemplate()
        {
        }

        public AgeCategoryTemplate(string name, int yearFrom, int yearTo, bool male) : base(name, yearFrom, yearTo, male)
        {
        }

        public int AgeCategoryCollectionId { get; set; }
        public virtual AgeCategoryCollection AgeCategoryCollection { get; set; }
    }
}