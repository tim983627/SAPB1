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
    class InventoryA : INotifyPropertyChanged
    {

        public InventoryA()
        {
            GetData();
        }

        public async void GetData()
        {
            using (var client = new HttpClient())
            {
                var x = Num.DocNum;
                //以防前次資料留著，先進行清除 
                var uri = "http://163.17.9.105:8070/WebAPI/api/A/GetDATAClear";
                var result = await client.GetStringAsync(uri);
                //使用者輸入X為盤點單編號，透過此編號進入MSSQL抓取盤點單裡面的商品編號、倉庫資料
                uri = "http://163.17.9.105:8070/WebAPI/api/A/GetInventory?x=" + x;
                result = await client.GetStringAsync(uri);
                //將商品編號、倉庫當作Select條件抓取位於指定倉庫裡某商品的序號
                uri = "http://163.17.9.105:8070/WebAPI/api/A/GetANumber";
                result = await client.GetStringAsync(uri);
                //將取出的序號放入Data
                List<DataInventoryA>  DataList = JsonConvert.DeserializeObject<List<DataInventoryA>>(result);
                Data = new ObservableCollection<DataInventoryA>(DataList);
                await PopupNavigation.Instance.PopAsync();
            }
        }

        ObservableCollection<DataInventoryA> _Data;
        public ObservableCollection<DataInventoryA> Data
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
        public async void InsertData(string ItemCode,string DistNumber,string Whether)
        {
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/A/GetA?ItemCode=" + ItemCode + "&DistNumber=" + DistNumber + "&Whether=" + Whether;
                var result = await client.GetStringAsync(uri);
            }
        }

    }
}