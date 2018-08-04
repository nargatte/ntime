using System.ComponentModel;
using BaseCore.DataBase;

namespace BaseCore.PlayerFilter
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PlayerSort
    {
        [Description("Nazwisko")]
        ByLastName,
        [Description("Imię")]
        ByFirstName,
        [Description("Klub")]
        ByTeam,
        [Description("Numer startowy")]
        ByStartNumber,
        [Description("Czas startu")]
        ByStartTime,
        [Description("Kategoria")]
        ByFullCategory,
        [Description("Data urodzenia")]
        ByBirthDate,
        [Description("Wyniki Open")]
        ByRank,
        [Description("ByExtraData")]
        ByExtraData
    }
}