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
    class PostingB : INotifyPropertyChanged
    {

        public PostingB()
        {
            PostData();
        }


        //將主管選擇的盤點單展示出來給主管審查
        public async void PostData()
        {
            using (var client = new HttpClient())
            {
                //X主管選擇的盤點單編號
                var x = Num.DocNum;
                var uri = "http://163.17.9.105:8070/WebAPI/api/B/GetBtoSir?x=" + x;
                var result = await client.GetStringAsync(uri);
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
                var x = Num.DocNum;
                var uri = "http://163.17.9.105:8070/WebAPI/api/B/GetB?ItemCode=" + ItemCode + "&BatchNumber=" + BatchNumber + "&Count=" + Count + "&Quantity=" + Quantity;
                var result = await client.GetStringAsync(uri);
            }
        }
        //將APP盤點結果先放入盤點單(還沒過帳)
        public async void GoCount()
        {
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/B/GetCount";
                var result = await client.GetStringAsync(uri);
            }
        }

        //執行存貨過帳
        public async void Posting()
        {
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/B/GetBPosting";
                var result = await client.GetStringAsync(uri);
            }
        }
    }
}