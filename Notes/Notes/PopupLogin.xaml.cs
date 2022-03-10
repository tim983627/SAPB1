using Notes.Models;
using Notes.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace Notes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupLogin : PopupPage
    {
        public PopupLogin()
        {
            InitializeComponent();
        }
        //送出帳密
        private async void Login_Clicked(object sender, EventArgs e)
        {
            string Acc = "manager";
            string Pass = "1234";
            if(Acc == Accouent.Text & Pass==Password.Text)
            {
                await Navigation.PushAsync(new SirPosting());
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
               await DisplayAlert("⚠️警告", "帳號或密碼輸入錯誤", "確定");
            }
        }
        //返回首頁
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }

}