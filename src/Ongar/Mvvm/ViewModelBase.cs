using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ongar
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        public void RaiseAndSetIfChanged<T>(ref T field, T value, PropertyChangedEventArgs e)
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, e);
            }
        }
    }
}
