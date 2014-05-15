using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Loko.DataModel
{
    public class LokoTask
    {
        public string Title { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public ObservableCollection<Geopoint> TaskLocations { get; set; }
    }

    public class DataSource
    {
        private ObservableCollection<LokoTask> _lokoTasks;

        public DataSource()
        {
            _lokoTasks = new ObservableCollection<LokoTask>();
        }
        
    }
}
