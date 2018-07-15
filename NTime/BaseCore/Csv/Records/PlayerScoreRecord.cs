using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.Records
{
    class PlayerScoreRecord
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public DateTime BirthDate { get; set; }
        public string FullCategory { get; set; }
        public ExtraPlayerInfo Subcategory { get; set; }
        public Distance Distance { get; set; }
        public decimal Time { get; set; }
        public int DistancePlaceNumber { get; set; }
        public int CategoryPlaceNumber { get; set; }
        public int LapsCount { get; set; }
        public decimal MeasurementTime { get; set; }
    }
}
