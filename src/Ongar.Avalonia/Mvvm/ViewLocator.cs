using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;

namespace Ongar
{
    public class ViewLocator : IDataTemplate
    {
        DistributedApplication distributedApplication = Application.Current as DistributedApplication;

        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            var name = data.GetType().FullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}