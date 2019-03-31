using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Dtos
{
    public class ExtraColumnValueDto : IDtoBase<ExtraColumnValue>
    {
        public ExtraColumnValueDto(ExtraColumnValue extraColumnValue)
        {
            Id = extraColumnValue.Id;
            PlayerId = extraColumnValue.PlayerId;
            ColumnId = extraColumnValue.ColumnId;
            CustomValue = extraColumnValue.CustomValue;
            LookupId = extraColumnValue.LookupId;
        }

        public ExtraColumnValue CopyDataFromDto(ExtraColumnValue entity)
        {
            entity.Id = Id;
            entity.PlayerId = PlayerId;
            entity.ColumnId = ColumnId;
            entity.CustomValue = CustomValue;
            entity.LookupId = LookupId;
            return entity;
        }

        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int ColumnId { get; set; }
        public string CustomValue { get; set; }
        public int? LookupId { get; set; }
    }
}