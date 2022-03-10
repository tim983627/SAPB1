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
    class PostingA : INotifyPropertyChanged
    {

        public PostingA()
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
                var uri = "http://163.17.9.105:8070/WebAPI/api/A/GetAtoSir?x=" + x;
                var result = await client.GetStringAsync(uri);
                List<DataInventoryA> DataList = JsonConvert.DeserializeObject<List<DataInventoryA>>(result);
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
        public async void InsertData(string ItemCode, string DistNumber, string Whether)
        {
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/A/GetA?ItemCode=" + ItemCode + "&DistNumber=" + DistNumber + "&Whether=" + Whether;
                var result = await client.GetStringAsync(uri);
            }
        }
        //剛API的資料放入盤點單(還沒過帳)
        public async void GoCount()
        {
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/A/GetCount";
                var result = await client.GetStringAsync(uri);
            }
        }

        //執行存貨過帳
        public async void Posting()
        {
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/A/GetAPosting";
                var result = await client.GetStringAsync(uri);
            }
        }
    }
}