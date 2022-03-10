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
    class InventoryC : INotifyPropertyChanged
    {

        public InventoryC()
        {
            GetData();
        }

        public async void GetData()
        {
            //Get
            using (var client = new HttpClient())
            {
                var x = Num.DocNum;
                //以防資料留著 先進行清除
                var uri = "http://163.17.9.105:8070/WebAPI/api/C/GetDATAClear";
                var result = await client.GetStringAsync(uri);
                //使用者輸入X為盤點單編號，透過此編號進入MSSQL抓取盤點單資料，並將資料存入Class cdata
                uri = "http://163.17.9.105:8070/WebAPI/api/C/Getamount?x=" + x;
                result = await client.GetStringAsync(uri); 
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


        //將盤點好的DATA丟回Web API
        public async void InsertData(string ItemCode, string CountQty, string InWhsQty)
        {
            using (var client = new HttpClient())
            {
                var x = Num.DocNum;
                var uri = "http://163.17.9.105:8070/WebAPI/api/C/GetC?ItemCode=" + ItemCode  + "&InWhsQty=" + InWhsQty + "&CountQty=" + CountQty;
                var result = await client.GetStringAsync(uri);
            }
        }

    }
}


