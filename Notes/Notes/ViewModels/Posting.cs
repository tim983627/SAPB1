using Newtonsoft.Json;
using Notes.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Notes.inventory;
namespace Notes.ViewModels
{
    class Posting : INotifyPropertyChanged
    {

        public Posting()
        {
            PostData();
        }
        //將未結且已盤點的盤點單傳給主管，讓主管選擇要審核哪張盤點單
        public async void PostData()
        {
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/A/GetCountNToSir";
                var result = await client.GetStringAsync(uri);
                List<DataABC> DataList = JsonConvert.DeserializeObject<List<DataABC>>(result);
                Data = new ObservableCollection<DataABC>(DataList);
            }
        }

        ObservableCollection<DataABC> _Data;
        public ObservableCollection<DataABC> Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}