using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Notes.Models
{
    public class DataInventoryB : INotifyPropertyChanged
    {
        // public string ItemCode { get; set; }
        string _ItemCode;
        public string ItemCode
        {
            set { SetProperty(ref _ItemCode, value); }

            get { return _ItemCode; }
        }

        // public string DistNumber { get; set; }
        string _BatchNumber;
        public string BatchNumber
        {
            set { SetProperty(ref _BatchNumber, value); }

            get { return _BatchNumber; }
        }
        //public int Count { get; set; }
        int _count;
        public int Count
        {
            set { SetProperty(ref _count, value); }
            get { return _count; }
        }
        //public int Quantity { get; set; }
        string _quantity;
        public string Quantity
        {
            set { SetProperty(ref _quantity, value); }
            get { return _quantity; }
        }

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
