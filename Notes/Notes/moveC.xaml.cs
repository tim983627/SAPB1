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
    public partial class moveC : ContentPage
    {
        MoveC evm;
        public moveC()
        {
            InitializeComponent();
            evm = new MoveC();
            BindingContext = evm;
        }
        //返回
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Move());
        }

        private async void GoMove(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("⚠️警告", "確定要庫存調撥嗎?", "確定", "取消");
            if (answer)
            {
                using (var client = new HttpClient())
                {
                    var uri = "http://163.17.9.105:8070/WebAPI/api/Move/GetDATAClear";
                    var result = await client.GetStringAsync(uri);
                }
                Num.Quantity = 0;
                foreach (var item in evm.Data)
                {
                    Num.Quantity += Convert.ToInt32(item.CountQty);
                }
                evm.GoMove();
                for (var i = 1; i <= 2; i++)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[1]);
                }
            }
            else
            {
            }
        }
    }
}