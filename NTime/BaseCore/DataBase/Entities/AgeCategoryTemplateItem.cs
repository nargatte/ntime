namespace BaseCore.DataBase
{
    public class AgeCategoryTemplateItem : AgeCategoryBase
    {
        public AgeCategoryTemplateItem()
        {
        }

        public AgeCategoryTemplateItem(string name, int yearFrom, int yearTo, bool male) : base(name, yearFrom, yearTo, male)
        {
        }

        public int AgeCategoryCollectionId { get; set; }
        public virtual AgeCategoryTemplate AgeCategoryCollection { get; set; }
    }
}