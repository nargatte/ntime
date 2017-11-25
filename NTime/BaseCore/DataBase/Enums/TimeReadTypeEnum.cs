using System.ComponentModel;
using System.Windows.Media;

namespace BaseCore.DataBase
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum TimeReadTypeEnum
    {
        [Description("Nieprzeliczony")]
        Unprocessed,
        [Description("Poprawny")]
        Significant,
        [Description("Przed startem")]
        NonsignificantBefore,
        [Description("Po zakończeniu")]
        NonsignificantAfter,
        [Description("Powtórzony")]
        Repeated,
        [Description("Pominięty")]
        Skipped,
        [Description("Sztuczny")]
        Void
    }
}