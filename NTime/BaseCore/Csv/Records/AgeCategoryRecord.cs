using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.Records
{
    public class AgeCategoryRecord
    {
        public AgeCategoryRecord(AgeCategory ageCategory, string distanceName = "")
        {
            Name = ageCategory.Name;
            YearFrom = ageCategory.YearFrom;
            YearTo = ageCategory.YearTo;
            IsMale = ageCategory.Male;
            DistanceName = distanceName;
        }


        public string Name { get; set; }
        public int YearFrom { get; set; }
        public int YearTo { get; set; }
        public bool IsMale { get; set; }
        public string DistanceName { get; set; }
    }
}
