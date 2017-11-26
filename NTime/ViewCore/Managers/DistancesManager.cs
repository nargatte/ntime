using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using ViewCore.Entities;

namespace ViewCore.Managers
{
    public class DistancesManager : CompetitionItemBase
    {
        private DistanceRepository _distanceRepository;
        public ObservableCollection<EditableDistance> DefinedDistances { get; set; } = new ObservableCollection<EditableDistance>();

        public DistancesManager(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            _distanceRepository = new DistanceRepository(new ContextProvider(), _currentCompetition.DbEntity);
        }


        public async Task<ObservableCollection<EditableDistance>> DownloadDistancesAsync()
        {
            var dbDistances = await _distanceRepository.GetAllAsync();
            DefinedDistances.Clear();
            foreach (var dbDistance in dbDistances)
            {
                DefinedDistances.Add(new EditableDistance(_currentCompetition)
                {
                    DbEntity = dbDistance
                });
            }
            return DefinedDistances;
        }
    }
}
