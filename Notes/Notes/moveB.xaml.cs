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
    public partial class moveB : ContentPage
    {
        MoveB evm;
        public moveB()
        {
            InitializeComponent();
            evm = new MoveB();
            BindingContext = evm;
        }
        //返回
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Move());
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
            string BatchNumber = x[1];
            int gg = 0;
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var data in evm.Data)
                {
                    //確認掃到的條碼是否存在於Data裡
                    if (data.ItemCode == ItemCode && data.BatchNumber == BatchNumber)
                    {
                        gg = 1;
                    }
                }
                foreach (var data in evm.Data)
                {
                    //調撥數量+1
                    if (data.ItemCode == ItemCode && data.BatchNumber == BatchNumber && gg == 1)
                    {
                        data.Count += 1;
                        break;
                    }
                    //警告此批號不存在於此倉庫
                    else if (data.ItemCode == ItemCode && gg == 0)
                    {
                        DisplayAlert("⚠️警告", "此批號不存在於" + Num.WhsCode + "中", "確定");
                        break;
                    }
                    //警告此商品不是此次調撥的商品
                    else if (data.ItemCode != ItemCode)
                    {
                        DisplayAlert("⚠️警告", "此商品不是" + Num.ItemCode + ",非此次調撥商品", "確認");
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
        private async void GoMove(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("⚠️警告", "確定要庫存調撥?", "確定", "取消");
            if (answer)
            {
                //資料傳回API
                //傳批號回去
                Num.Quantity = 0;
                foreach (var item in evm.Data.Where(w => w.Count != 0))
                {
                    evm.InsertData(item.BatchNumber, item.Count);
                    Num.Quantity += item.Count;
                }
                using (var client = new HttpClient())
                {
                    var uri = "http://163.17.9.105:8070/WebAPI/api/Move/GetTN?itemcode=" + Num.ItemCode + "&FromWarehouse=" + Num.WhsCode + "&warehouse=" + Num.MoveWhsCode + "&Quantity=" + Num.Quantity;
                    var result = await client.GetStringAsync(uri);
                }
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