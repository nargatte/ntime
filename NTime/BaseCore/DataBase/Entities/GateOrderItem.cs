namespace BaseCore.DataBase
{
    public class GatesOrderItem : IEntityId
    {
        public GatesOrderItem()
        {

        }

        public GatesOrderItem(decimal minTimeBefore)
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