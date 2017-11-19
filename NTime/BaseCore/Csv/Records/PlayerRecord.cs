using System;

namespace BaseCore.Csv
{
    public class PlayerRecord
    {
        public int StartNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Team { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsMale { get; set; }
        public string StringAditionalInfo { get; set; }
        public DateTime StartTime { get; set; }
        public string StringDistance { get; set; }
    }
}