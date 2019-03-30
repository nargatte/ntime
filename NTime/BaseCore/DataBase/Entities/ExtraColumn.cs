using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class ExtraColumn : IEntityId, ICompetitionId
    {
        public ExtraColumn()
        {

        }
        public ExtraColumn(ExtraColumn column)
        {
            this.Id = column.Id;
            this.CompetitionId = column.CompetitionId;
            this.Title = column.Title;
            this.IsRequired = column.IsRequired;
            this.IsDisplayedInPublicList = column.IsDisplayedInPublicList;
            this.IsDisplayedInPublicDetails = column.IsDisplayedInPublicDetails;
            this.MultiValueCount = column.MultiValueCount;
            this.MinCharactersValidation = column.MinCharactersValidation;
            this.MaxCharactersValidation = column.MaxCharactersValidation;
            this.Type = column.Type;
            this.SortIndex = column.SortIndex;
        }

        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDisplayedInPublicList { get; set; }
        public bool IsDisplayedInPublicDetails { get; set; }
        public string Type { get; set; } = "Text";
        public int? SortIndex { get; set; }
        public int? MultiValueCount { get; set; }
        public int? MinCharactersValidation { get; set; }
        public int? MaxCharactersValidation { get; set; }

        public virtual ICollection<ExtraColumnValue> ExtraColumnValues { get; set; }
    }
}
