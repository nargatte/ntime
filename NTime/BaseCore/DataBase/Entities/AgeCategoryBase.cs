using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public abstract class AgeCategoryBase : IEntityId
    {
        public AgeCategoryBase()
        {
        }

        public AgeCategoryBase(string name, int yearFrom, int yearTo)
        {
            Name = name;
            YearFrom = yearFrom;
            YearTo = yearTo;
        }

        public int Id { get; set; }

        [StringLength(256), Required]
        public string Name { get; set; }

        public int YearFrom { get; set; }

        public int YearTo { get; set; }
    }
}