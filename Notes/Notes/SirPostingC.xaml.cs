using Notes.Models;
using Notes.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Notes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SirPostingC : ContentPage
    {
        PostingC peko;
        public SirPostingC()
        {
            InitializeComponent();
            //把盤點單編號丟給Label顯示給主管
            loadingAsync();
            var x = Num.DocNum;
            N.Text = "盤點單編號:" + x;
            peko = new PostingC();
            BindingContext = peko;
        }
        void UpdateListView()
        {
            var itemsSource = Listview.ItemsSource;
            Listview.ItemsSource = null;
            Listview.ItemsSource = itemsSource;
        }
        async Task loadingAsync()
        {
            await PopupNavigation.Instance.PushAsync(new Loading());
        }

        //完成盤點，返回盤點首頁
        private async void gotomainpage(Object sender, EventArgs e)
        {

            bool answer = await DisplayAlert("⚠️警告", "要執行存貨過帳嗎?", "確定", "取消");
            if (answer)
            {
                using (var client = new HttpClient())
                {
                    var uri = "http://163.17.9.105:8070/WebAPI/api/C/GetDATAClear";
                    var result = await client.GetStringAsync(uri);
                }
                foreach (var item in peko.Data)
                {
                    peko.InsertData(item.ItemCode, item.InWhsQty, item.CountQty);
                }
                peko.GoCount();
                peko.Posting();
                int delay = 3000;
                await Task.Delay(delay);
                await Navigation.PushAsync(new MainPage());
                await Navigation.PushAsync(new SirPosting());
            }
            else
            {
            }
        }
    }
}