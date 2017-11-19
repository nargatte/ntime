using System.ComponentModel;

namespace BaseCore.DataBase
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SortOrderEnum
    {
        [Description("Rosnąco")]
        Ascending,
        [Description("Malejąco")]
        Descending,
    }
}