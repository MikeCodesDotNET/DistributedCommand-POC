using Avalonia;
using Ongar.Messaging.Messages;
using Ongar.Mvvm;
using System;
using System.Reflection;
using System.Windows.Input;

namespace Ongar
{
    public static class DistributedCommand
    {
        public static DistributedCommandBase Create(Action<object> action, object sender, string target)
        {
            
            Action<object> wrappedAction = (obj) =>
            {
                var message = new DistributedCommandExecuted() { Sender = sender.GetType().Name, ExecutedAt = DateTime.Now, TargetCommand = target };
                var distributedApp = Application.Current as DistributedApplication;
                distributedApp.MessagingService.SendMessage(message);

                action(obj);
            };

            return new DistributedCommandBase(wrappedAction);
        }


    }
}
