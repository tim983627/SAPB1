using Newtonsoft.Json;
using Notes.Models;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace Notes
{
    class Test : INotifyPropertyChanged
    {
      
            public Test()
            {
                GetData();
            }
            public async void GetData()
            {
                await PopupNavigation.Instance.PushAsync(new Loading());
                using (var client = new HttpClient())
                {
                // send a GET request  
                var uri = "http://163.17.9.105:8070/WebAPI/api/A/GetGGs";
                var result = await client.GetStringAsync(uri);
                //handling the answer  
                var DataList = JsonConvert.DeserializeObject<List<datatest>>(result);
                Data = new ObservableCollection<datatest>(DataList);
                }
                await PopupNavigation.Instance.PopAsync();
            }

            ObservableCollection<datatest> _Data;
            public ObservableCollection<datatest> Data
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
