using System;

namespace ViewCore.Entities
{
    public interface IEditableCompetition
    {
        string City { get; set; }
        string Description { get; set; }
        DateTime EventDate { get; set; }
        string Link { get; set; }
        string Name { get; set; }
        string Organiser { get; set; }
        BaseCore.DataBase.Competition DbEntity { get; set; }
    }
}