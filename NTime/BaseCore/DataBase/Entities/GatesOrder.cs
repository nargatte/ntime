namespace BaseCore.DataBase
{
    public class GatesOrder : IEntityId
    {
        public GatesOrder()
        {
        }

        public GatesOrder(int gateNumber, decimal minTimeBefore)
        {
            GateNumber = gateNumber;
            MinTimeBefore = minTimeBefore;
        }

        public int Id { get; set; }

        public int GateNumber { get; set; }

        //TODO Wydaje mi się, że coś tego typu powinniśmy zrobić, ale to konieczna byłaby oddzielna tabela, którego każdemu punktowi pomiarowemu poza numerem przypisywałaby nazwę
        //public string GateName { get; set; }

        public int OrderNumber { get; set; }

        public decimal MinTimeBefore { get; set; }

        public int DistanceId { get; set; }
        public virtual Distance Distance { get; set; }
    }
}