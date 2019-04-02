using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Dtos
{
    public class ExtraColumnDto : IDtoBase<ExtraColumn>
    {
        public ExtraColumnDto()
        {

        }
        public ExtraColumnDto(ExtraColumn column)
        {
            Id = column.Id;
            CompetitionId = column.CompetitionId;
            Title = column.Title;
            IsRequired = column.IsRequired;
            IsDisplayedInPublicList = column.IsDisplayedInPublicList;
            IsDisplayedInPublicDetails = column.IsDisplayedInPublicDetails;
            Type = column.Type;
            SortIndex = column.SortIndex;
            MultiValueCount = column.MultiValueCount;
            MinCharactersValidation = column.MinCharactersValidation;
            MaxCharactersValidation = column.MaxCharactersValidation;
        }

        public ExtraColumn CopyDataFromDto(ExtraColumn column)
        {
            column.Id = Id;
            column.CompetitionId = CompetitionId;
            column.Title = Title;
            column.IsRequired = IsRequired;
            column.IsDisplayedInPublicList = IsDisplayedInPublicList;
            column.IsDisplayedInPublicDetails = IsDisplayedInPublicDetails;
            column.Type = Type;
            column.SortIndex = SortIndex;
            column.MultiValueCount = MultiValueCount;
            column.MinCharactersValidation = MinCharactersValidation;
            column.MaxCharactersValidation = MaxCharactersValidation;
            return column;
        }

        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDisplayedInPublicList { get; set; }
        public bool IsDisplayedInPublicDetails { get; set; }
        public string Type { get; set; } = "Text";
        public int? SortIndex { get; set; }
        public int? MultiValueCount { get; set; }
        public int? MinCharactersValidation { get; set; }
        public int? MaxCharactersValidation { get; set; }
    }
}