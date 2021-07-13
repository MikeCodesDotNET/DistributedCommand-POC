using Avalonia;
using Ongar.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ongar
{
    public static class ViewModelBaseExtensions
    {
        public static void RaiseRemoteAndSetIfChanged<T>(this ViewModelBase vm, ref T field, T value, PropertyChangedEventArgs e)
        {
            vm.RaiseAndSetIfChanged(ref field, value, e);

            var application = Application.Current as DistributedApplication;


            if(application != null)
                application.MessagingService.SendMessage(new ViewModelPropertyDidChangeMessage() { ViewModelName = vm.GetType().Name, FieldName = e.PropertyName , NewValue = value });
        }


      
    }
}
