using System.Collections.ObjectModel;
using BaseCore.DataBase;

namespace ViewCore.Entities
{
    public interface IEditableGate
    {
        ObservableCollection<EditableTimeReadsLogInfo> AssignedLogs { get; set; }
        Gate DbEntity { get; set; }
        string Name { get; set; }
        int Number { get; set; }
    }
}