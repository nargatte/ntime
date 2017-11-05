using System.Collections.ObjectModel;

namespace BaseCore.DataBase
{
    public class AgeCategory : AgeCategoryBase
    {
        public AgeCategory()
        {
        }

        public AgeCategory(string name, int yearFrom, int yearTo) : base(name, yearFrom, yearTo)
        {
        }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }
        public virtual ObservableCollection<Player> Players { get; set; }
    }
}