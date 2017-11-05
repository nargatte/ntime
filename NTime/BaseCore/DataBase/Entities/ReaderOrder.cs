namespace BaseCore.DataBase
{
    public class ReaderOrder
    {
        public ReaderOrder()
        {
        }

        public ReaderOrder(int readerNumber, int orderNumber, decimal minTimeBetween, int distanceId)
        {
            ReaderNumber = readerNumber;
            OrderNumber = orderNumber;
            MinTimeBetween = minTimeBetween;
            DistanceId = distanceId;
        }

        public int Id { get; set; }

        public int ReaderNumber { get; set; }

        public int OrderNumber { get; set; }

        public decimal MinTimeBetween { get; set; }

        public int DistanceId { get; set; }
        public virtual Distance Distance { get; set; }
    }
}