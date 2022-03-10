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
    public partial class export : ContentPage
    {
        public export()
        {
            InitializeComponent();
            WhsCode.Items.Add("A00商品");
            WhsCode.Items.Add("A01半成品");
            WhsCode.Items.Add("A10原料");
            WhsCode.Items.Add("A20物料");
            WhsCode.Items.Add("A30在製品");
            WhsCode.Items.Add("A40製成品");
            WhsCode.Items.Add("A58退驗商品");
            WhsCode.Items.Add("A40報廢");
        }
        //前往發貨
        private async void gotoexport(Object sender, EventArgs e)
        {
            Num.ItemCode = ItemCode.Text;
            Num.WhsCode = WhsCode.Items[WhsCode.SelectedIndex];
            if (Num.ItemCode == "")
            {
                await DisplayAlert("⚠️警告", "請確實輸入商品編號", "確認");
            }
            else
            {
                using (var client = new HttpClient())
                {
                    //利用使用者輸入的商品編號進入資料庫，並找到它是屬於ABC哪類商品。
                    var uri = "http://163.17.9.105:8070/WebAPI/api/IO/GetABC?itemcode=" + ItemCode.Text;
                    var result = await client.GetStringAsync(uri);
                    var DataList = JsonConvert.DeserializeObject<List<DataABC>>(result);
                    Data = new ObservableCollection<DataABC>(DataList);
                    //透過商品編號進入ABC商品的頁面
                    if (Data.Count == 0)
                    {
                        await DisplayAlert("⚠️警告", "輸入的商品編號不存在或不在倉庫中", "確認");
                    }
                    else
                    {
                        foreach (var i in Data)
                        {
                            if (i.ABCNumber == "A")
                            {
                                await Navigation.PushAsync(new exportA());
                                await PopupNavigation.Instance.PushAsync(new Loading());
                            }
                            else if (i.ABCNumber == "B")
                            {
                                await Navigation.PushAsync(new exportB());
                                await PopupNavigation.Instance.PushAsync(new Loading());
                            }
                            else if (i.ABCNumber == "C")
                            {
                                await PopupNavigation.Instance.PushAsync(new exportC());
                                await PopupNavigation.Instance.PushAsync(new Loading());
                            }
                        }
                    }   
                }
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