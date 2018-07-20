using BaseCore.Csv.Records;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.Map
{
    public class PlayerWithPointsMap : ClassMap<PlayerWithPoints> 
    {
        public PlayerWithPointsMap()
        {
            Map(m => m.FirstName).Name("imie");
            Map(m => m.FirstName).Name("nazwisko");
            Map(m => m.BirthDate).Name("data_urodzenia").ConvertUsing(date => date.BirthDate.Year.ToString());
            Map(m => m.AgeCategory).Name("kat_wiek");
            Map(m => m.FirstName).Name("imie");
            Map(m => m.FirstName).Name("imie");
            Map(m => m.FirstName).Name("imie");
            //Map().
        }
    }
}
