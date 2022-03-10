using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Notes.Models
{
    public class DataABC
    {
        string _ABCNumber;
        public string ABCNumber
        {
            set { SetProperty(ref _ABCNumber, value);}

            get { return _ABCNumber; }
        }

        string _Entry;
        public string Entry
        {
            set { SetProperty(ref _Entry, value); }

            get { return _Entry; }
        }

        string _ItemCode;
        public string ItemCode
        {
            set { SetProperty(ref _ItemCode, value); }

            get { return _ItemCode; }
        }

        string _ItemDesc;
        public string ItemDesc
        {
            set { SetProperty(ref _ItemDesc, value); }

            get { return _ItemDesc; }
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
