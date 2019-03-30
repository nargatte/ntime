using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class ExtraColumn : IEntityId, ICompetitionId
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDisplayedInPublicList { get; set; }
        public bool IsDisplayedInPublicDetails { get; set; }
        public string Type { get; set; }
        public int? SortIndex { get; set; }
        public int? MultiValueCount { get; set; }
        public int? MinCharactersValidation { get; set; }
        public int? MaxCharactersValidation { get; set; }

        public ICollection<ExtraColumnValue> ExtraColumnValues { get; set; }
    }
}
