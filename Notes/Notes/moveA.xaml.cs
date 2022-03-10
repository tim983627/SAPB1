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
    public partial class moveA : ContentPage
    {
        MoveA evm;
        public moveA()
        {
            InitializeComponent();
            evm = new MoveA();
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
            string DistNumber = x[1];
            int gg = 0;
            Device.BeginInvokeOnMainThread(() =>
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
                    //編號、序號一樣(放入調撥單
                    if (Num.ItemCode == ItemCode && Peko.DistNumber == DistNumber && gg == 1)
                    {
                        if (Peko.Whether == "ffalse.png")
                        {
                            Peko.Whether = "ttrue.png";
                            break;
                        }
                        else
                        {
                            break;
                        }

                    }
                    //編號一樣，序號不一樣(警告商品不再此倉庫，無法放入調撥單
                    else if (Num.ItemCode == ItemCode && gg == 0)
                    {
                        DisplayAlert("⚠️警告", "此序號不存在於" + Num.WhsCode + "中", "確認");
                        break;
                    }
                    //編號、序號不一樣(警告此商品不是此次調撥的商品
                    else if (Num.ItemCode != ItemCode && gg == 0)
                    {
                        DisplayAlert("⚠️警告", "此商品不是" + Num.ItemCode + ",非此次調撥商品", "確認");
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

        //使用者點擊則選取那個序號或取消那個序號
        private void Listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (this.evm.Data[e.ItemIndex].Whether == "ffalse.png")
            {
                this.evm.Data[e.ItemIndex].Whether = "ttrue.png";
                BindingContext = evm;
                UpdateListView();
            }
            else
            {
                this.evm.Data[e.ItemIndex].Whether = "ffalse.png";
                BindingContext = evm;
                UpdateListView();
            }
        }

        private async void GoMove(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("⚠️警告", "確定要庫存調撥?", "確定", "取消");
            if (answer)
            {
                //資料傳回API
                //傳序號回去
                Num.Quantity = 0;
                foreach (var item in evm.Data.Where(w => w.Whether == "ttrue.png"))
                {
                    using (var client = new HttpClient())
                    {
                        var uri = "http://163.17.9.105:8070/WebAPI/api/Move/GetTransfer?N=" + item.DistNumber;
                        var result = await client.GetStringAsync(uri);
                    }
                    Num.Quantity += 1;
                }
                using (var client = new HttpClient())
                {
                    //執行調撥
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