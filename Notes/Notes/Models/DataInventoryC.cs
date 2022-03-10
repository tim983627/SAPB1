using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Notes.Models
{
    public class DataInventoryC : INotifyPropertyChanged
    {
        // public string ItemCode { get; set; }
        string _ItemCode;
        public string ItemCode
        {
            set { SetProperty(ref _ItemCode, value); }

            get { return _ItemCode; }
        }

        // public string InWhsQty { get; set; }
        string _InWhsQty;
        public string InWhsQty
        {
            set { SetProperty(ref _InWhsQty, value); }

            get { return _InWhsQty; }
        }
        //public int CountQty { get; set; }
        string _CountQty;
        public string CountQty
        {
            set { SetProperty(ref _CountQty, value); }
            get { return _CountQty; }
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