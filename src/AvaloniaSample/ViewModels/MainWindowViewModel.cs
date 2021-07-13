using Avalonia.Media;
using Ongar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace AvaloniaSample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public ICommand WelcomeCommand { get; private set; }

        public ICommand ChangeBackgroundColorCommand { get; private set; }

        ISolidColorBrush bg;
        public ISolidColorBrush BackgroundColor
        {
            get => bg;
            set => this.RaiseRemoteAndSetIfChanged(ref bg, value, new System.ComponentModel.PropertyChangedEventArgs(nameof(BackgroundColor)));
        }


        string greetings;
        public string Greeting
        {
            get => greetings;
            set => this.RaiseRemoteAndSetIfChanged(ref greetings, value, new System.ComponentModel.PropertyChangedEventArgs(nameof(Greeting)));
        }

        public MainWindowViewModel()
        {
            BackgroundColor = Brushes.White;

            Greeting = "Welcome to Avalonia!";
            WelcomeCommand = DistributedCommand.Create((s) => 
            { 
                Greeting = "Message Sent"; 

            }, this, nameof(WelcomeCommand));

            ChangeBackgroundColorCommand = DistributedCommand.Create((s) =>
            {
                ChangeBackgroundColor();
            }, this, nameof(ChangeBackgroundColorCommand));
        }


        private void ChangeBackgroundColor()
        {
            if (BackgroundColor == Brushes.White)
                BackgroundColor = Brushes.Red;
            else
                BackgroundColor = Brushes.White;
        }

    }
}
