using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using BaseCore.DataBase;

namespace ViewCore.Entities
{
    public class EditableCompetition : EditableBaseClass<Competition>, IEditableCompetition
    {
        public EditableCompetition(IEditableCompetition currentComptetition): base(currentComptetition) { }
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
            get { return DbEntity.Organiser; }
            set { DbEntity.Organiser = SetProperty(DbEntity.Organiser, value); }
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
    }
}
