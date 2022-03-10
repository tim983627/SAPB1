using Notes.Models;
using Notes.ViewModels;
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
    public partial class SirPostingA : ContentPage
    {
        PostingA peko;
        public SirPostingA()
        {
            InitializeComponent();
            //把盤點單編號丟給Label顯示給主管
            var x = Num.DocNum;
            N.Text = "盤點單編號:"+x;
            peko = new PostingA();
        }
        
       //主管可編輯盤點結果
        private async void Listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //盤到的序號刪除
            if (this.peko.Data[e.SelectedItemIndex].Whether == "GreenTick.png")
            {
                bool answer = await DisplayAlert("⚠️警告", "確定要刪除此序號的盤點結果嗎?", "確定", "取消");
                if (answer)
                {
                    this.peko.Data[e.SelectedItemIndex].Whether = "RedCross.png";
                    BindingContext = peko;
                    UpdateListView();
                }
                else
                {
                }
                
            }
            //沒盤到的序號新增
            else if (this.peko.Data[e.SelectedItemIndex].Whether == "RedCross.png")
            {
                bool answer = await DisplayAlert("⚠️警告", "確定要新增此序號嗎?", "確定", "取消");
                if (answer)
                {
                    this.peko.Data[e.SelectedItemIndex].Whether = "GreenTick.png";
                    BindingContext = peko;
                    UpdateListView();
                }
                else
                {
                }
            }
            //盤到的序號刪除
            else if (this.peko.Data[e.SelectedItemIndex].Whether == "NewTick.png")
            {
                bool answer = await DisplayAlert("⚠️警告", "確定要刪除此序號的盤點結果嗎?", "確定", "取消");
                if (answer)
                {
                    this.peko.Data[e.SelectedItemIndex].Whether = "RedCross.png";
                    BindingContext = peko;
                    UpdateListView();
                }
                else
                {
                }
            }

        }
        //更新頁面
        void UpdateListView()
        {
            var itemsSource = Listview.ItemsSource;
            Listview.ItemsSource = null;
            Listview.ItemsSource = itemsSource;
        }

        //完成盤點並返回盤點首頁
        private async void gotomainpage(Object sender, EventArgs e)
        {
            //執行存貨過帳
            bool answer = await DisplayAlert("⚠️警告", "要執行存貨過帳嗎?", "確定", "取消");
            if (answer)
            {
                using (var client = new HttpClient())
                {
                    var uri = "http://163.17.9.105:8070/WebAPI/api/A/GetDATAClear";
                    var result = await client.GetStringAsync(uri);
                }
                foreach (var item in peko.Data)
                {
                    peko.InsertData(item.ItemCode, item.DistNumber, item.Whether);
                }
                peko.GoCount();
                peko.Posting();
                int delay = 3000;
                await Task.Delay(delay);
                //await Application.Current.MainPage.Navigation.PopAsync();
                await Navigation.PushAsync(new MainPage());
                await Navigation.PushAsync(new SirPosting());
            }
            else
            {
            }
        }
    }
}