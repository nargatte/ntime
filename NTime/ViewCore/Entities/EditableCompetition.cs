using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using BaseCore.DataBase;

namespace ViewCore.Entities
{
    public class EditableCompetition : BindableBase, IEditableCompetition
    {
        public Competition DbEntity { get; set; }
        public EditableCompetition()
        {
            if (DbEntity == null)
                DbEntity = new Competition();
        }

        public EditableCompetition(string name, DateTime eventDate, string description, string link, string organiser, string city)
        {
            DbEntity = new Competition(name, eventDate, description, link, organiser, city);
        }
        public string Name
        {
            get { return DbEntity.Name; }
            set { DbEntity.Name = SetProperty(DbEntity.Name, value); }
        }

        public string City
        {
            get { return DbEntity.City; }
            set { DbEntity.City = SetProperty(DbEntity.City, value); }
        }


        public DateTime EventDate
        {
            get { return DbEntity.EventDate; }
            set { DbEntity.EventDate = SetProperty(DbEntity.EventDate, value); }
        }


        public string Organiser
        {
            get { return DbEntity.Organizer; }
            set { DbEntity.Organizer = SetProperty(DbEntity.Organizer, value); }
        }

        public string Description
        {
            get { return DbEntity.Description; }
            set { DbEntity.Description = SetProperty(DbEntity.Description, value); }
        }

        public string Link
        {
            get { return DbEntity.Link; }
            set { DbEntity.Link = SetProperty(DbEntity.Link, value); }
        }

        public DateTime? SignUpEndDate
        {
            get { return DbEntity.SignUpEndDate; }
            set { DbEntity.SignUpEndDate = SetProperty(DbEntity.SignUpEndDate, value); }
        }

        public bool OrganizerEditLock
        {
            get { return DbEntity.OrganizerEditLock; }
            set { DbEntity.OrganizerEditLock = SetProperty(DbEntity.OrganizerEditLock, value); }
        }
    }
}
