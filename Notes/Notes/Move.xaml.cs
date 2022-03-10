using Newtonsoft.Json;
using Notes.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Notes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Move : ContentPage
    {
        public Move()
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
            MoveWhsCode.Items.Add("A00商品");
            MoveWhsCode.Items.Add("A01半成品");
            MoveWhsCode.Items.Add("A10原料");
            MoveWhsCode.Items.Add("A20物料");
            MoveWhsCode.Items.Add("A30在製品");
            MoveWhsCode.Items.Add("A40製成品");
            MoveWhsCode.Items.Add("A58退驗商品");
            MoveWhsCode.Items.Add("A40報廢");
        }
        private async void gotoimport(Object sender, EventArgs e)
        {
            Num.ItemCode = ItemCode.Text;
            Num.WhsCode = WhsCode.Items[WhsCode.SelectedIndex];
            Num.MoveWhsCode = MoveWhsCode.Items[MoveWhsCode.SelectedIndex];
            if (Num.WhsCode == Num.MoveWhsCode)
            {
                await DisplayAlert("⚠️警告", "調撥倉庫重複選擇,請重新選擇", "確認");
            }
            else if (Num.ItemCode=="")
            {
                await DisplayAlert("⚠️警告", "請確實輸入商品編號", "確認");
            }
            else
            {
                using (var client = new HttpClient())
                {
                    //利用使用者輸入的商品編號進入資料庫，並找到它是屬於ABC哪類商品。
                    var uri = "http://163.17.9.105:8070/WebAPI/api/Move/GetABC?itemcode=" + ItemCode.Text;
                    var result = await client.GetStringAsync(uri);
                    var DataList = JsonConvert.DeserializeObject<List<DataABC>>(result);
                    Data = new ObservableCollection<DataABC>(DataList);
                    //透過商品編號進入ABC商品的頁面
                    foreach (var i in Data)
                    {
                        if (i.ABCNumber == "A")
                        {
                            await Navigation.PushAsync(new moveA());
                            await PopupNavigation.Instance.PushAsync(new Loading());
                        }
                        else if (i.ABCNumber == "B")
                        {
                            await Navigation.PushAsync(new moveB());
                            await PopupNavigation.Instance.PushAsync(new Loading());
                        }
                        else if (i.ABCNumber == "C")
                        {
                            await Navigation.PushAsync(new moveC());
                            await PopupNavigation.Instance.PushAsync(new Loading());
                        }
                        else
                        {
                            await DisplayAlert("⚠️警告", "輸入的商品編號有錯或不存在於倉庫", "確認");
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