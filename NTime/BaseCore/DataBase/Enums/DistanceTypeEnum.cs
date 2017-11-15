using System.ComponentModel;

namespace BaseCore.DataBase
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum DistanceTypeEnum
    {
        [Description("Wyścig na określonym dystansie")]
        DeterminedDistance,
        [Description("Wyścig na okrążeniach")]
        DeterminedLaps,
        [Description("Wyścig z na okrążeniach z limitem czasu")]
        LimitedTime
    }
}