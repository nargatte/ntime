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

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}