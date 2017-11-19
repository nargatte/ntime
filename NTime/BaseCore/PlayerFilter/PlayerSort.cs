using System.ComponentModel;
using BaseCore.DataBase;

namespace BaseCore.PlayerFilter
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PlayerSort
    {
        [Description("Imię")]
        ByFirstName,
        [Description("Nazwisko")]
        ByLastName,
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
        ByRank
    }
}