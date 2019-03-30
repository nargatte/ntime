using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class ExtraColumnEqualityComparer : IEqualityComparer<ExtraColumn>
    {
        public bool Equals(ExtraColumn x, ExtraColumn y)
        {
            return x.Id == y.Id &&
                x.CompetitionId == y.CompetitionId &&
                x.Title == y.Title &&
                x.IsRequired == y.IsRequired &&
                x.IsDisplayedInPublicList == y.IsDisplayedInPublicList &&
                x.IsDisplayedInPublicDetails == y.IsDisplayedInPublicDetails &&
                x.SortIndex == y.SortIndex &&
                x.MultiValueCount == y.MultiValueCount &&
                x.MinCharactersValidation == y.MinCharactersValidation &&
                x.MaxCharactersValidation == y.MaxCharactersValidation;
        }

        // This is not a really good hashcode
        public int GetHashCode(ExtraColumn obj)
        {
            return 13 + 17 * (
                obj.Id +
                obj.CompetitionId +
                obj.Title?.Length
                ).Value.GetHashCode();
        }
    }
}
