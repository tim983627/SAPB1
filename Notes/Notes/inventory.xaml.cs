using Notes.Models;
using Notes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Net.Http;
using System.Runtime.CompilerServices;
using static Notes.inventory;
using Rg.Plugins.Popup.Services;

namespace Notes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class inventory : ContentPage
    {
        public inventory()
        {
            InitializeComponent();
        }
        //前往盤點 決定前往ABC商品的盤點頁面
        private async void gotoinventory2(Object sender, EventArgs e)
        {
            //取出Entry的Text
            string x = DocNum.Text;
            //將DocNum放入Num類別裡
            Num.DocNum = x;
            if (x != "")
            {
                using (var client = new HttpClient())
                {
                    //利用盤點單編號搜尋到盤點單，並用裡面的商品編號抓出它是屬於ABC哪類商品。
                    var uri = "http://163.17.9.105:8070/WebAPI/api/A/GetInventory?x=" + x;
                    var result = await client.GetStringAsync(uri);
                    uri = "http://163.17.9.105:8070/WebAPI/api/A/GetABC";
                    result = await client.GetStringAsync(uri);
                    var DataList = JsonConvert.DeserializeObject<List<DataABC>>(result);
                    Data = new ObservableCollection<DataABC>(DataList);
                    foreach (var i in Data)
                    {
                        //先將商品資訊存起來 存貨過帳時要判斷進入哪個過帳畫面
                        Num.ABC = i.ABCNumber;
                        if (i.ABCNumber == "A")
                        {
                            await Navigation.PushAsync(new inventory4());
                            await PopupNavigation.Instance.PushAsync(new Loading());
                        }
                        else if (i.ABCNumber == "B")
                        {
                            await Navigation.PushAsync(new inventory3());
                            await PopupNavigation.Instance.PushAsync(new Loading());
                        }
                        else if (i.ABCNumber == "C")
                        {
                            await Navigation.PushAsync(new inventory2());
                            await PopupNavigation.Instance.PushAsync(new Loading());
                        }
                        else if (i.ABCNumber == "Error")
                        {
                            await DisplayAlert("⚠️警告", "輸入的單號查無資料", "確認");
                        }
                        else if (i.ABCNumber == "Close")
                        {
                            await DisplayAlert("⚠️警告", "此盤點單已結,無法進行盤點", "確認");
                        }
                    }

                }
            }
            else
            {
                await DisplayAlert("⚠️警告", "請輸入盤點單編號", "確認");
            }
        }
        ObservableCollection<DataABC> _Data;
        public ObservableCollection<DataABC> Data
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
           
        
        //返回首頁
        private async void gotomainpage(Object sender, EventArgs e)
        {
            Navigation.RemovePage(Navigation.NavigationStack[1]);
            //await Navigation.PushAsync(new MainPage());
        }
    }
}