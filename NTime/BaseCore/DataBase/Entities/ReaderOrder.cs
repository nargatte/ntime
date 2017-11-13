namespace BaseCore.DataBase
{
    public class ReaderOrder : IEntityId
    {
        public ReaderOrder()
        {
        }

        public ReaderOrder(int readerNumber, decimal minTimeBetween)
        {
            ReaderNumber = readerNumber;
            MinTimeBetween = minTimeBetween;
        }

        public int Id { get; set; }

        public int ReaderNumber { get; set; }

        public int OrderNumber { get; set; }

        public decimal MinTimeBetween { get; set; }

        public int DistanceId { get; set; }
        public virtual Distance Distance { get; set; }
    }
}