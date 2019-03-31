using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class PlayerListViewDto : PlayerBaseDto, IDtoBase<Player>
    {

        public PlayerListViewDto(Player player) : base(player)
        {
            StartNumber = player.StartNumber;
            StartTime = player.StartTime;
            FullCategory = player.FullCategory;
            IsPaidUp = player.IsPaidUp;
            ExtraData = player.ExtraData;
            CompetitionId = player.CompetitionId;
            ExtraColumnValues = player.ExtraColumnValues
                .Where(column => column.Column.IsDisplayedInPublicList)
                .Select(columnValue => new ExtraColumnValueDto(columnValue)).ToArray();
        }

        public override Player CopyDataFromDto(Player player)
        {
            base.CopyDataFromDto(player);
            player.StartNumber = StartNumber;
            player.StartTime = StartTime;
            player.FullCategory = FullCategory;
            player.IsPaidUp = IsPaidUp; 
            player.ExtraData = ExtraData;
            player.CompetitionId = CompetitionId;
            player.ExtraColumnValues = ExtraColumnValues.Select(extraColumn => extraColumn.CopyDataFromDto(new ExtraColumnValue())).ToArray();
            return player;
        }

        public int StartNumber { get; set; }
        public DateTime? StartTime { get; set; }
        [StringLength(255)]
        public string FullCategory { get; set; }
        public bool IsPaidUp { get; set; }
        public string ExtraData { get; set; }
        public int CompetitionId { get; set; }
        public ExtraColumnValueDto[] ExtraColumnValues { get; set; }

    }
}