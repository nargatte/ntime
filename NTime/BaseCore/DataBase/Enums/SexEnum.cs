using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SexEnum
    {
        [Description("M")]
        Male,
        [Description("K")]
        Female
    }
}
