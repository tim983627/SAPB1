using Notes.Models;
using Notes.ViewModels;
using Rg.Plugins.Popup.Services;
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
//A類商品盤點
namespace Notes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class inventory4 : ContentPage
    {
        InventoryA evm;
        public inventory4()
        {
            InitializeComponent();
            evm = new InventoryA();
        }

        //Barcode相機設立
        public void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            var option = new ZXing.Mobile.MobileBarcodeScanningOptions()
            {
                PossibleFormats = new List<ZXing.BarcodeFormat>() { ZXing.BarcodeFormat.QR_CODE },
                CameraResolutionSelector = DependencyService.Get<IZXingHelper>().SelectLowestResolutionMatchingDisplayAspectRatio
            };
            //相機掃描到的資料處理
            var x = result.Text.Split(',');
            string ItemCode = x[0];
            string DistNumber = x[1];
            int gg = 0;
            Device.BeginInvokeOnMainThread(async () =>
            {
                foreach (var Peko in evm.Data)
                {
                    //確認掃到的條碼是否存在於Data裡
                    if (Peko.ItemCode == ItemCode && Peko.DistNumber == DistNumber)
                    {
                        gg = 1;
                    }
                }
                foreach (var Peko in evm.Data)
                {
                    //商品編號、序號一樣，盤點成功
                    if (Peko.ItemCode == ItemCode && Peko.DistNumber == DistNumber && gg == 1)
                    {
                        if (Peko.Whether == "RedCross.png")
                        {
                            Peko.Whether = "GreenTick.png";
                            break;
                        }
                        else
                        {  
                            break;
                        }
                        
                    }
                    //商品編號一樣，序號不一樣，盤營新增
                    else if (Peko.ItemCode == ItemCode && gg==0)
                    {
                        Add(ItemCode, DistNumber);
                        break;
                    }
                    //商品編號、序號不一樣，不是盤點商品，提醒使用者
                    else if (Peko.ItemCode != ItemCode && gg == 0)
                    {
                        AlertAsync();
                        break;
                    }
                }
                BindingContext = evm;
                UpdateListView();
            });
        }
        //更新ListView
        void UpdateListView()
        {
            var itemsSource = Listview.ItemsSource;
            Listview.ItemsSource = null;
            Listview.ItemsSource = itemsSource;
        }
        //新增盤營商品
        void Add(string ItemCode, string DistNumber)
        {
            DataInventoryA InventoeyADD = new DataInventoryA() { ItemCode = ItemCode, DistNumber = DistNumber, Whether = "NewTick.png" };
            evm.Data.Add(InventoeyADD);
        }
        //非盤點商品，警告使用者
        async void AlertAsync ()
        {
            await DisplayAlert("⚠️警告", "此條碼並非盤點商品", "確認");
        }

        //完成掃描，返回首頁
        private async void gotoinventory(Object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("⚠️警告", "要送出盤點結果嗎?", "確定", "取消");
            if(answer)
            {
                //掃描結果放入盤點單(還沒過帳)
                foreach (var item in evm.Data)
                {
                    evm.InsertData(item.ItemCode, item.DistNumber, item.Whether);
                }
                using (var client = new HttpClient())
                {
                    var uri = "http://163.17.9.105:8070/WebAPI/api/A/GetCount";
                    var result = await client.GetStringAsync(uri);
                    uri = "http://163.17.9.105:8070/WebAPI/api/A/GetSer";
                    result = await client.GetStringAsync(uri);
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