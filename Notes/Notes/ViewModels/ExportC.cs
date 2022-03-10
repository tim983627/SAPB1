using Newtonsoft.Json;
using Notes.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace Notes.ViewModels
{
    class ExportC : INotifyPropertyChanged
    {
        public ExportC()
        {
            GetData();
        }

        public async void GetData()
        {
            //Get
            using (var client = new HttpClient())
            {
                var ItemCode = Num.ItemCode;
                var WhsCode = Num.WhsCode;
                //使用者輸入商品編號以及倉庫，抓出庫存
                var uri = "http://163.17.9.105:8070/WebAPI/api/IO/GetCNumber?ItemCode=" + ItemCode + "&WhsCode=" + WhsCode;
                var result = await client.GetStringAsync(uri);
                List<DataInventoryC> DataList = JsonConvert.DeserializeObject<List<DataInventoryC>>(result);
                Data = new ObservableCollection<DataInventoryC>(DataList);
                await PopupNavigation.Instance.PopAsync();
            }
        }

        ObservableCollection<DataInventoryC> _Data;
        public ObservableCollection<DataInventoryC> Data
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

        //執行發貨
        public async void GoExport()
        {
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/IO/GetGenExit?itemcode=" + Num.ItemCode + "&quantity=" + Num.Quantity + "&warehouse=" + Num.WhsCode;
                var result = await client.GetStringAsync(uri);
            }

        }
    }
}
