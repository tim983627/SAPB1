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
    class PostingC : INotifyPropertyChanged
    {

        public PostingC()
        {
            PostData();
        }


        //將主管選擇的盤點單展示出來給主管審查
        public async void PostData()
        {
            using (var client = new HttpClient())
            {
                var x = Num.DocNum;
                var uri = "http://163.17.9.105:8070/WebAPI/api/C/GetCtoSir?x="+x;
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

        //將盤點好的DATA丟回Web API
        public async void InsertData(string ItemCode, string InWhsQty, string CountQty)
        {
            using (var client = new HttpClient())
            {

                var uri = "http://163.17.9.105:8070/WebAPI/api/C/GetC?ItemCode=" + ItemCode + "&InWhsQty=" + InWhsQty + "&CountQty=" + CountQty;
                var result = await client.GetStringAsync(uri);
            }
        }
        //將APP盤點結果先放入盤點單(還沒過帳)
        public async void GoCount()
        {
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/C/GetCount";
                var result = await client.GetStringAsync(uri);
            }
        }

        //執行存貨過帳
        public async void Posting()
        {
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/C/GetCPosting";
                var result = await client.GetStringAsync(uri);
            }
        }
    }
}