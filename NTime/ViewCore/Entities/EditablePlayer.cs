using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.TimesProcess;
using MvvmHelper;

namespace ViewCore.Entities
{
    public class EditablePlayer : BindableBase
    {
        private BaseCore.DataBase.Player player = new BaseCore.DataBase.Player();
        public BaseCore.DataBase.Player Player
        {
            get { return player; }
            set { player = value; }
        }

        public int StartNumber
        {
            get { return player.StartNumber; }
            set { player.StartNumber = SetProperty(Player.StartNumber, value); }
        }

        public string FirstName
        {
            get { return Player.FirstName; }
            set { Player.FirstName = SetProperty(player.FirstName, value); }
        }


        public string LastName
        {
            get { return Player.LastName; }
            set { Player.LastName = SetProperty(Player.LastName, value); }
        }

        //TODO Grzesiek
        public string DistanceName
        {
            get { return Player.Distance.Name; }
            set { Player.Distance.Name = SetProperty(Player.Distance.Name, value); }
        }


        public DateTime BirthDate
        {
            get { return Player.BirthDate; }
            set { Player.BirthDate = SetProperty(Player.BirthDate, value); }
        }


        public DateTime? StartTime
        {
            get { return Player.StartTime; }
            set { Player.StartTime = SetProperty(Player.StartTime, value); }
        }


        public string Team
        {
            get { return Player.Team; }
            set { Player.Team = SetProperty(Player.Team, value); }
        }


        public string ExtraPlayerInfo
        {
            get { return Player.ExtraPlayerInfo.Name; }
            set { Player.ExtraPlayerInfo.Name = SetProperty(Player.ExtraPlayerInfo.Name, value); }
        }

        public string PhoneNumber
        {
            get { return Player.PhoneNumber; }
            set { Player.PhoneNumber = SetProperty(Player.PhoneNumber, value); }
        }

        public bool IsMale
        {
            get { return Player.IsMale; }
            set { Player.IsMale = SetProperty(Player.IsMale, value); }
        }

        public string FullCategory
        {
            get { return Player.FullCategory; }
            set { Player.FullCategory = SetProperty(Player.FullCategory, value); }
        }

        public int DistancePlaceNumber
        {
            get { return Player.DistancePlaceNumber; }
            set { Player.DistancePlaceNumber = SetProperty(Player.DistancePlaceNumber, value); }
        }

        public int CategoryPlaceNumber
        {
            get { return Player.CategoryPlaceNumber; }
            set { Player.CategoryPlaceNumber = SetProperty(Player.CategoryPlaceNumber, value); }
        }


        public int LapsCount
        {
            get { return Player.LapsCount; }
            set { Player.LapsCount = SetProperty(Player.LapsCount, value); }
        }

        public string Time
        {
            get { return Player.Time.ToDateTime().ConvertToString(); }
            set
            {
                if (value.TryConvertToDateTime(out DateTime dateTime))
                    Player.Time = SetProperty(Player.Time, dateTime.ToDecimal());
            }
        }


    }
}
