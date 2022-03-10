using Notes.Models;
using Notes.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using static Notes.ViewModels.ExportC;

namespace Notes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class exportC : PopupPage
    {
        ExportC evm;
        public exportC()
        {
            InitializeComponent();
            evm = new ExportC();
            BindingContext = evm;
        }
        //返回
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void GoExport(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("⚠️警告", "確定要發貨嗎?", "確定", "取消");
            if (answer)
            {
                using (var client = new HttpClient())
                {
                    var uri = "http://163.17.9.105:8070/WebAPI/api/IO/GetDATAClear";
                    var result = await client.GetStringAsync(uri);
                }
                Num.Quantity = 0;
                //統計數量
                foreach (var item in evm.Data)
                {
                    Num.Quantity += Convert.ToInt32(item.CountQty);
                }
                //執行發貨
                evm.GoExport();
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
                     
    }

}