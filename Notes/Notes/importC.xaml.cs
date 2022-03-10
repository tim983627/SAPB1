using Notes.Models;
using Rg.Plugins.Popup.Pages;
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
    public partial class importC : PopupPage
    {
        public importC()
        {
            InitializeComponent();
            ItemCode.Text = Num.ItemCode;
            WhsCode.Text = Num.WhsCode;
        }

        private async void GoImport(object sender, EventArgs e)
        { 
            bool answer = await DisplayAlert("⚠️警告", "確定要收貨嗎?", "確定", "取消");
            if (answer)
            {
                using (var client = new HttpClient())
                {
                    //利用盤點單編號搜尋到盤點單，並用裡面的商品編號抓出它是屬於ABC哪類商品。
                    var uri = "http://163.17.9.105:8070/WebAPI/api/IO/GetImport?itemcode=" + Num.ItemCode + "&warehouse=" + Num.WhsCode + "&quantity=" + CountQty.Text + "&price=" + Num.Price;
                    var result = await client.GetStringAsync(uri);
                    //await Navigation.PushAsync(new MainPage());
                }
                for (var i = 1; i <= 2; i++)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[1]);
                }
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {

            }
            
        }
        //返回
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }

}