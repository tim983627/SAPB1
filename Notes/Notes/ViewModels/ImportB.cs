using Newtonsoft.Json;
using Notes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Notes.ViewModels
{
    class ImportB : INotifyPropertyChanged
    {

        public ImportB()
        {
            List<DataInventoryB> DataList = new List<DataInventoryB>();
            Data = new ObservableCollection<DataInventoryB>(DataList);
        }

        ObservableCollection<DataInventoryB> _Data;
        public ObservableCollection<DataInventoryB> Data
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


        //執行收貨
        public async void GoImport()
        {
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/IO/GetImport?itemcode=" + Num.ItemCode + "&warehouse=" + Num.WhsCode + "&quantity=" + Num.Quantity + "&price=" + Num.Price;
                var result = await client.GetStringAsync(uri);
            }
        }
    }
}