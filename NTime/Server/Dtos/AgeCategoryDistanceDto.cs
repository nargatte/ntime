using BaseCore.DataBase;

namespace Server.Dtos
{
    public class AgeCategoryDistanceDto : IDtoBase<AgeCategoryDistance>
    {
        public AgeCategoryDistanceDto(AgeCategoryDistance ageCategoryDistance)
        {
            Id = ageCategoryDistance.Id;
            AgeCategoryId = ageCategoryDistance.AgeCategoryId;
            DistanceId = ageCategoryDistance.DistanceId;
        }

        public AgeCategoryDistance CopyDataFromDto(AgeCategoryDistance ageCategoryDistance)
        {
            ageCategoryDistance.Id = Id;
            ageCategoryDistance.AgeCategoryId = AgeCategoryId;
            ageCategoryDistance.DistanceId = DistanceId;
            return ageCategoryDistance;
        }

        public int Id { get; set; }
        public int? AgeCategoryId { get; set; }
        public int? DistanceId { get; set; }
    }
}