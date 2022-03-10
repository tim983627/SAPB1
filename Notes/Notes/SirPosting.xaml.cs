using Newtonsoft.Json;
using Notes.Models;
using Notes.ViewModels;
using Rg.Plugins.Popup.Services;
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
    public partial class SirPosting : ContentPage
    {
        Posting peko;
        public SirPosting()
        {
            InitializeComponent();
            peko = new Posting();
        }
        protected virtual void OnAppearing()
        {
            var itemsSource = Listview.ItemsSource;
            Listview.ItemsSource = null;
            Listview.ItemsSource = itemsSource;
        }
        //判斷主管選擇的盤點單是ABC哪類商品，並進入那類商品的過帳畫面
        private async void Listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Num.DocNum = this.peko.Data[e.ItemIndex].Entry;
            var itemcode = this.peko.Data[e.ItemIndex].ItemCode;
            using (var client = new HttpClient())
            {
                var uri = "http://163.17.9.105:8070/WebAPI/api/A/GetSirABC?x=" + itemcode;
                var result = await client.GetStringAsync(uri);
                List<DataABC> DataList = JsonConvert.DeserializeObject<List<DataABC>>(result);
                Data = new ObservableCollection<DataABC>(DataList);
            }
            foreach (var i in Data)
            {
                Num.ABC = i.ABCNumber;
                if (i.ABCNumber == "A")
                {
                    await Navigation.PushAsync(new SirPostingA());
                    await PopupNavigation.Instance.PushAsync(new Loading());
                }
                else if (i.ABCNumber == "B")
                {
                    await Navigation.PushAsync(new SirPostingB());
                    await PopupNavigation.Instance.PushAsync(new Loading());
                }
                else if (i.ABCNumber == "C")
                {
                    await Navigation.PushAsync(new SirPostingC());
                }
            }
            
        }
        ObservableCollection<DataABC> _Data;
        public ObservableCollection<DataABC> Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}