using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class ExtraPlayerInfoDto : IDtoBase<ExtraPlayerInfo>
    {
        public ExtraPlayerInfoDto()
        {

        }

        public ExtraPlayerInfoDto(ExtraPlayerInfo extraPlayerInfo)
        {
            Id = extraPlayerInfo.Id;
            Name = extraPlayerInfo.Name;
            ShortName = extraPlayerInfo.ShortName;
        }

        public ExtraPlayerInfo CopyDataFromDto(ExtraPlayerInfo extraPlayerInfo)
        {
            extraPlayerInfo.Id = Id;
            extraPlayerInfo.Name = Name;
            extraPlayerInfo.ShortName = ShortName;
            return extraPlayerInfo;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        [StringLength(255), Required]
        public string ShortName { get; set; }
    }
}