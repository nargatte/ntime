using System.Collections.ObjectModel;

namespace ViewCore.Entities
{
    public interface IEditableGate
    {
        ObservableCollection<EditableTimeReadsLog> AssignedLogs { get; set; }
        string Name { get; set; }
        int Number { get; set; }
    }
}