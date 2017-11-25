using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BaseCore.DataBase
{
    [System.AttributeUsage(System.AttributeTargets.Enum)]
    public class CustomColorAttribute : System.Attribute
    {
        public string AssignedColor { get; set; }
    }
}
