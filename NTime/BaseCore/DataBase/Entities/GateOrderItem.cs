namespace BaseCore.DataBase
{
    public class GateOrderItem : IEntityId
    {
        public GateOrderItem()
        {
        }

        public GateOrderItem(decimal minTimeBefore)
        {
            MinTimeBefore = minTimeBefore;
        }

        public int Id { get; set; }

        public int OrderNumber { get; set; }

        public decimal MinTimeBefore { get; set; }

        public int? GateId { get; set; }
        public virtual Gate Gate { get; set; }

        public int DistanceId { get; set; }
        public virtual Distance Distance { get; set; }
    }
}