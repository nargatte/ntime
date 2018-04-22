using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public abstract class AgeCategoryBase : IEntityId
    {
        public AgeCategoryBase()
        {
        }

        protected AgeCategoryBase(string name, int yearFrom, int yearTo, bool male)
        {
            Name = name;
            YearFrom = yearFrom;
            YearTo = yearTo;
            Male = male;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        public int YearFrom { get; set; }

        public int YearTo { get; set; }

        public bool Male { get; set; }
    }
}