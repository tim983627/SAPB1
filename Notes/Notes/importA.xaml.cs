using Notes.Models;
using Notes.ViewModels;
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
    public partial class importA : ContentPage
    {
        ImportA evm;
        public importA()
        {  
            InitializeComponent();
            evm = new ImportA();
            BindingContext = evm;
        }

        //Barcode相機設立
        public void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            var option = new ZXing.Mobile.MobileBarcodeScanningOptions()
            {
                PossibleFormats = new List<ZXing.BarcodeFormat>() { ZXing.BarcodeFormat.QR_CODE },
                CameraResolutionSelector = DependencyService.Get<IZXingHelper>().SelectLowestResolutionMatchingDisplayAspectRatio
            };
            //相機掃描到的資料顯示
            var x = result.Text.Split(',');
            string ItemCode = x[0];
            string DistNumber = x[1];
            int gg = 0;
            Device.BeginInvokeOnMainThread(() =>
            {
                if(evm.Data.Count==0) 
                {
                    gg = 0;
                    //序號沒出現過 新增至收貨單
                    if (Num.ItemCode == ItemCode && gg == 0)
                    {
                        evm.Data.Add(new DataInventoryA {ItemCode=ItemCode,DistNumber=DistNumber,Whether="GreenTick.png" });
                    }
                    //編號不同 提醒非此次收貨的商品
                    else if (Num.ItemCode != ItemCode && gg == 0)
                    {
                        DisplayAlert("⚠️警告", "編號:" + ItemCode + ",非此次收貨商品", "確認");
                    }
                }
                else
                {
                    foreach (var Peko in evm.Data)
                    {
                        //確認掃到的條碼是否存在於Data裡
                        if (Num.ItemCode == ItemCode && Peko.DistNumber == DistNumber)
                        {
                            gg = 1;
                        }
                    }
                    foreach (var Peko in evm.Data)
                    {
                        //序號重複提醒使用者
                        if (Num.ItemCode == ItemCode && Peko.DistNumber == DistNumber && gg == 1)
                        {
                            DisplayAlert("⚠️警告", "序號:" + DistNumber + ",已經新增過了", "確認");
                        }
                        //序號沒出現過 新增至收貨單
                        else if (Num.ItemCode == ItemCode && gg == 0)
                        {
                            evm.Data.Add(new DataInventoryA { ItemCode = ItemCode, DistNumber = DistNumber, Whether = "GreenTick.png" });
                            break;
                        }
                        //編號不同 提醒非此次收貨的商品
                        else if (Num.ItemCode != ItemCode && gg == 0)
                        {
                            DisplayAlert("⚠️警告", "編號:" + ItemCode + ",非此次收貨商品", "確認");
                            break;
                        }
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
        //完成盤點，返回首頁
        private async void gotoimport(Object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("⚠️警告", "確定要收貨嗎", "確定", "取消");
            if (answer)
            {
                Num.Quantity = 0;
                foreach (var item in evm.Data)
                {
                    //將序號存入API
                    using (var client = new HttpClient())
                    {
                        var uri = "http://163.17.9.105:8070/WebAPI/api/IO/GetExit?N=" + item.DistNumber +"&Q="+1;
                        var result = await client.GetStringAsync(uri);
                        Num.Quantity += 1;
                    }
                }
                //執行發貨
                evm.GoImport();
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