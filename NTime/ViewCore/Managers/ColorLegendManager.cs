using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;

namespace ViewCore.Managers
{
    public class ColorLegendManager
    {
        private ObservableCollection<TimeReadColorsLegendItem> _legendItems;
        public ObservableCollection<TimeReadColorsLegendItem> GetLegendItems()
        {
            _legendItems = new ObservableCollection<TimeReadColorsLegendItem>();
            var enumValues = Enum.GetValues(typeof(TimeReadTypeEnum)) as TimeReadTypeEnum[];
            for (int i = 0; i < enumValues.Length; i++)
            {
                _legendItems.Add(new TimeReadColorsLegendItem()
                {
                    Name = EnumHelper.GetEnumDescription(enumValues[i]),
                    TimeReadType = enumValues[i]
                });
            }
            return _legendItems;
        }
    }
}
