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
                && x.BirthDate.Year == y.BirthDate.Year)
                return true;
            else
                return false;
        }

        public int GetHashCode(PlayerWithScores obj)
        {
            var first = obj.FirstName.ToLower();
            var last = obj.LastName.ToLower();
            var year = obj.BirthDate.Year;
            return first.Length * last.Length
                * first[0] * first.Last()
                * last[0] * last.Last()
                * year * year;
        }
    }
}
