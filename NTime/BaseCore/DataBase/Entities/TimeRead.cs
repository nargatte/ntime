using System.Diagnostics.Eventing.Reader;

namespace BaseCore.DataBase
{
    public class TimeRead
    {
        public TimeRead()
        {
        }

        public TimeRead(decimal time, int reader, int playerId)
        {
            Time = time;
            Reader = reader;
            PlayerId = playerId;
        }

        public int Id { get; set; }

        public decimal Time { get; set; }

        public int Reader { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsSkipped { get; set; }

        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
    }
}