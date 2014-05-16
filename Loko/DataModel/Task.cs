using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage;
using System.IO;

namespace Loko.DataModel
{
    public class LokoTask
    {
        public string Title { get; set; }
        public bool HasExpiry { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public ObservableCollection<Geopoint> TaskLocations { get; set; }
    }

    public class DataSource
    {
        private const string fileName = "lokoTasks.json";
        private ObservableCollection<LokoTask> _lokoTasks;

        public DataSource()
        {
            _lokoTasks = new ObservableCollection<LokoTask>();
        }

        public async Task<ObservableCollection<LokoTask>> GetLokoTasks()
        {
            await ensureDataLoaded();
            return _lokoTasks;
        }
        private async Task ensureDataLoaded()
        {
            if (_lokoTasks.Count == 0)
            {
                await getLokoTaskDataAsync();
            }
            return;
        }

        private async Task getLokoTaskDataAsync()
        {
            if (_lokoTasks.Count != 0)
            {
                return;
            }

            var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<LokoTask>));

            try
            {
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(fileName))
                {
                    _lokoTasks = (ObservableCollection<LokoTask>)jsonSerializer.ReadObject(stream);
                }
            }
            catch
            {
                _lokoTasks = new ObservableCollection<LokoTask>();
            }
        }

        public async void AddLokoTask(string title)
        {
            var lokoTask = new LokoTask();
            lokoTask.Title = title;
            lokoTask.HasExpiry = false;
            lokoTask.ExpiryDateTime = DateTime.MaxValue;
            lokoTask.TaskLocations = new ObservableCollection<Geopoint>();

            _lokoTasks.Add(lokoTask);
            await saveLokoTaskDataAsync();
        }

        public async void AddLokoTask(string title, DateTime expiry)
        {
            var lokoTask = new LokoTask();
            lokoTask.Title = title;
            lokoTask.HasExpiry = true;
            lokoTask.ExpiryDateTime = expiry;
            lokoTask.TaskLocations = new ObservableCollection<Geopoint>();

            _lokoTasks.Add(lokoTask);
            await saveLokoTaskDataAsync();
        }
        public async void AddLokoTask(string title, DateTime expiry, ObservableCollection<Geopoint> locations)
        {
            var lokoTask = new LokoTask();
            lokoTask.Title = title;
            lokoTask.HasExpiry = true;
            lokoTask.ExpiryDateTime = expiry;
            lokoTask.TaskLocations = locations;

            _lokoTasks.Add(lokoTask);
            await saveLokoTaskDataAsync();
        }
        public async void AddLokoTask(string title, ObservableCollection<Geopoint> locations)
        {
            var lokoTask = new LokoTask();
            lokoTask.Title = title;
            lokoTask.HasExpiry = false;
            lokoTask.ExpiryDateTime = DateTime.MaxValue;
            lokoTask.TaskLocations = locations;

            _lokoTasks.Add(lokoTask);
            await saveLokoTaskDataAsync();
        }
        private async Task saveLokoTaskDataAsync()
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<LokoTask>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(fileName, CreationCollisionOption.ReplaceExisting))
            {
                jsonSerializer.WriteObject(stream, _lokoTasks);
            }
        }

    }
}
