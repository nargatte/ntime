using System.Collections.Generic;

namespace BaseCore.DataBase
{
    public class OrganizerAccount : IEntityId, IAccountId
    {
        public int Id { get; set; }

        public string AccountId { get; set; }

        public virtual ICollection<Competition> Competitions { get; set; }
    }
}