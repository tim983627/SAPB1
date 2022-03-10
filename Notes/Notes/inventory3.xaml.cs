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
//B類商品盤點
namespace Notes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class inventory3 : ContentPage
    {
        InventoryB Ib = new InventoryB();
        public inventory3()
        {
            InitializeComponent();
            BindingContext = Ib;
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
            string batchnum = x[1];
            int key = 0;

            Device.BeginInvokeOnMainThread(async() =>
            {
                foreach (var data in Ib.Data)
                {
                    //確認掃到的條碼是否存在於Data裡
                    if (data.ItemCode == ItemCode && data.BatchNumber == batchnum)
                    {
                        key = 1;
                    }
                }
                foreach (var item in Ib.Data)
                {
                    if (item.ItemCode == ItemCode && item.BatchNumber == batchnum && key == 1)
                    {
                        bool ans = await DisplayAlert("⚠️警告","批號:"+item.BatchNumber+"盤點數量+1", "確定", "取消");
                        if (ans)
                        {
                            item.Count += 1;
                        }
                    }
                    else if (item.ItemCode == ItemCode && key == 0)
                    {
                        Add(ItemCode, batchnum, "A00商品", 1, "1");
                        break;
                    }

                    else if (item.ItemCode != ItemCode)
                    {
                        AlertAsync();
                        break;
                    }
                }
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
        //新增商品
        void Add(string cardcode, string batchnum, string whs, int count, string quantity)
        {
            DataInventoryB ADD = new DataInventoryB() { ItemCode = cardcode, BatchNumber = batchnum, Count = count, Quantity = quantity };
            Ib.Data.Add(ADD);
        }
        //非盤點商品，警告使用者
        async void AlertAsync()
        {
            await DisplayAlert("⚠️警告", "此條碼並非盤點商品", "確認");
        }

        //完成盤點，返回首頁
        private async void gotoinventory(Object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("⚠️警告", "要送出盤點結果嗎?", "確定", "取消");
            if (answer)
            {
                foreach (var item in Ib.Data)
                {
                    Ib.InsertData(item.ItemCode, item.BatchNumber, item.Count, item.Quantity);
                }
                await Navigation.PushAsync(new MainPage());
                //盤點資料放入盤點單(還沒過帳)
                using (var client = new HttpClient())
                {
                    var uri = "http://163.17.9.105:8070/WebAPI/api/B/GetCount";
                    var result = await client.GetStringAsync(uri);
                    uri = "http://163.17.9.105:8070/WebAPI/api/B/GetBat";
                    result = await client.GetStringAsync(uri);
                }
                //將頁面移除、回到首頁
                //await Navigation.PushAsync(new MainPage());
                for (var i = 1; i <= 2; i++)
                {
                    Navigation.RemovePage(Navigation.NavigationStack[1]);
                }

            }
            

        }

    }


}