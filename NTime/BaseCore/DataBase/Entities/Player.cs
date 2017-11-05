using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class Player
    {
        public Player()
        {
        }

        public int Id { get; set; }
        
        [StringLength(256), Required]
        public string FirstName { get; set; }

        [StringLength(256), Required]
        public string SecondName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public bool IsMale { get; set; }

        [StringLength(256)]
        public string Team { get; set; }

        public int StartNumber { get; set; }

        [StringLength(256)]
        public string FullCategory { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public int ExtraPlayerInfoId { get; set; }
        public virtual ExtraPlayerInfo ExtraPlayerInfo { get; set; }

        public int DistanceId { get; set; }
        public virtual Distance Distance { get; set; }

        public int AgeCategoryId { get; set; }
        public virtual AgeCategory AgeCategory { get; set; }

        public virtual ObservableCollection<TimeRead> TimeReads { get; set; }
    }
}