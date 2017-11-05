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

        public int AgeCategoryTemplateCollectionId { get; set; }
        public virtual AgeCategoryTemplateCollection AgeCategoryTemplateCollection { get; set; }
    }
}