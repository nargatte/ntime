using System.ComponentModel;

namespace BaseCore.DataBase
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum DistanceTypeEnum
    {
        [Description("na dystans")]
        DeterminedDistance,
        [Description("na okrążenia")]
        DeterminedLaps,
        [Description("na czas w pętli")]
        LimitedTime
    }
}