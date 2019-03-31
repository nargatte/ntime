using System;

namespace ViewCore.Entities
{
    public interface IEditableCompetition
    {
        int Id { get; set; }
        string Name { get; set; }
        string City { get; set; }
        string Description { get; set; }
        DateTime EventDate { get; set; }
        DateTime? SignUpEndDate { get; set; }
        string Link { get; set; }
        string LinkDisplayedName { get; set; }
        string Organiser { get; set; }
        bool OrganizerEditLock { get; set; }
        BaseCore.DataBase.Competition DbEntity { get; set; }
    }
}