using BaseCore.Csv.Records;
using System.Collections.Generic;

namespace BaseCore.DataBase
{
    public class AgeCategory : AgeCategoryBase, ICompetitionId
    {
        public AgeCategory()
        {
        }

        public AgeCategory(string name, int yearFrom, int yearTo, bool male) : base(name, yearFrom, yearTo, male)
        {
        }

        public AgeCategory(AgeCategoryRecord record) : this(record.Name, record.YearFrom, record.YearTo, record.IsMale)
        {

        }

        public AgeCategory(AgeCategoryBase ageCategory): this(ageCategory.Name, ageCategory.YearFrom, ageCategory.YearTo, ageCategory.Male)
        {

        }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<AgeCategory> AgeCategories { get; set; }
    }
}