using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.Records
{
    class PlayerWithPointsEqualityComparer : IEqualityComparer<PlayerWithScores>
    {
        public bool Equals(PlayerWithScores x, PlayerWithScores y)
        {
            if (x.FirstName.ToLower() == y.FirstName.ToLower()
                && x.LastName.ToLower() == y.LastName.ToLower()
                && x.BirthDate.Year == y.BirthDate.Year
                && x.AgeCategory == y.AgeCategory)
                return true;
            else
                return false;
        }

        public int GetHashCode(PlayerWithScores obj)
        {
            var first = obj.FirstName.ToLower();
            var last = obj.LastName.ToLower();
            var year = obj.BirthDate.Year;
            var ageCategory = obj.AgeCategory;
            return first.Length * last.Length
                * first.First() * first.Last()
                * last.First() * last.Last()
                * ageCategory.First() * ageCategory.Last()
                + year * year;
        }
    }
}
