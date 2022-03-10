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
    class MoveB : INotifyPropertyChanged
    {
        public MoveB()
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
                //以防資料留著 先進行清除
                var uri = "http://163.17.9.105:8070/WebAPI/api/Move/GetDATAClear";
                var result = await client.GetStringAsync(uri);
                //使用者輸入商品編號以及倉庫，用他們抓出此商品在此倉庫的序號
                uri = "http://163.17.9.105:8070/WebAPI/api/Move/GetBNumber?ItemCode=" + ItemCode + "&WhsCode=" + WhsCode;
                result = await client.GetStringAsync(uri);
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


        //將發貨好的DATA丟回Web API
        public async void InsertData(string Number, int Count)
        {
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/Move/GetTransfer?N=" + Number + "&Q=" + Count;
                var result = await client.GetStringAsync(uri);
            }

        }
    }
}
