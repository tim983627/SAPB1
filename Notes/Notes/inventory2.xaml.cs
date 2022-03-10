using Notes.Models;
using Notes.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
//C類商品盤點
namespace Notes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class inventory2 : ContentPage
    {
        InventoryC Ic;
        static int C = 0;
        public inventory2()
        {
            InitializeComponent();
            Ic = new InventoryC();
        }
        //Barcode相機設立
        void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            var option = new ZXing.Mobile.MobileBarcodeScanningOptions()
            {
                PossibleFormats = new List<ZXing.BarcodeFormat>() { ZXing.BarcodeFormat.QR_CODE },
                CameraResolutionSelector = DependencyService.Get<IZXingHelper>().SelectLowestResolutionMatchingDisplayAspectRatio
            };
            string ItemCode = result.Text;
            int gg = 0;
            //相機掃描到的資料
            Device.BeginInvokeOnMainThread(async() =>
            {
                foreach (var item in Ic.Data)
                {
                    //確認掃到的條碼是否存在於Data裡
                    if (item.ItemCode == ItemCode)
                    {
                        gg = 1;
                    }
                }
                foreach (var item in Ic.Data)
                {
                    //編號一樣，盤點成功
                    if (item.ItemCode == ItemCode && gg == 1)
                    {
                        bool ans = await DisplayAlert("⚠️警告", "盤點數量加1 ", "確定", "取消");
                        if(ans)
                        {
                            C += 1;
                            item.CountQty = Convert.ToString(C);
                        }
                    }
                    else if (item.ItemCode != ItemCode && gg == 0)
                    {
                        AlertAsync();
                        break;
                    }
                }
                BindingContext = Ic;
                UpdateListView();
            });
        }
        void UpdateListView()
        {
            var itemsSource = Listview.ItemsSource;
            Listview.ItemsSource = null;
            Listview.ItemsSource = itemsSource;
        }
        async void AlertAsync()
        {
            await DisplayAlert("警告", "此條碼並非盤點商品", "確認");
        }
        //完成掃描，返回盤點首頁
        private async void gotoinventory(Object sender, EventArgs e)
        {
            C = 0;
            bool answer = await DisplayAlert("⚠️警告", "要送出盤點結果嗎?", "確定", "取消");

            if (answer)
            {
                foreach (var item in Ic.Data)
                {
                    Ic.InsertData(item.ItemCode, item.InWhsQty, item.CountQty);
                }
                //先放入盤點單(還沒過帳)
                await Navigation.PushAsync(new MainPage());
                using (var client = new HttpClient())
                {
                    var uri = "http://163.17.9.105:8070/WebAPI/api/C/GetCount";
                    var result = await client.GetStringAsync(uri);
                }
                //將頁面移除、回到首頁
                //await Navigation.PushAsync(new MainPage());
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