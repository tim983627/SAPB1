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

namespace Notes.ViewModels
{
    class InventoryB : INotifyPropertyChanged
    {

        public InventoryB()
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
                var uri = "http://163.17.9.105:8070/WebAPI/api/B/GetDATAClear";
                var result = await client.GetStringAsync(uri);
                //使用者輸入X為盤點單編號，透過此編號進入MSSQL抓取盤點單資料，並將資料存入Class Inventory
                uri = "http://163.17.9.105:8070/WebAPI/api/B/GetInventory?x=" + x;
                result = await client.GetStringAsync(uri);
                //將Inventory裡的商品編號、倉庫當作條件抓取位於指定倉庫裡某商品的批號，並將資料存入Class BNumber
                uri = "http://163.17.9.105:8070/WebAPI/api/B/GetBNumber";
                result = await client.GetStringAsync(uri);
                //handling the answer  
                List<DataInventoryB> DataList = JsonConvert.DeserializeObject<List<DataInventoryB>>(result);
                Data = new ObservableCollection<DataInventoryB>(DataList);
                await PopupNavigation.Instance.PopAsync();
            }
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

        //將盤點好的DATA丟回Web API
        public async void InsertData(string ItemCode, string BatchNumber, int Count, string Quantity)
        {
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/B/GetB?ItemCode="+ItemCode + "&BatchNumber=" + BatchNumber + "&Count=" + Count + "&Quantity=" + Quantity;
                var result = await client.GetStringAsync(uri);
            }
        }

    }
}


