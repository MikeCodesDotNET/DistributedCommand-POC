using Ongar.Messaging;
using Ongar.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ongar
{
    public abstract class DistributedApplication : Avalonia.Application, IApplication
    {
        public override void OnFrameworkInitializationCompleted()
        {
            OnStart();
            base.OnFrameworkInitializationCompleted();
        }

        public DistributedApplication()
        {
            PopulateViewModels();
        }

        public Dictionary<string, ViewModelBase> ViewModels { get; private set; }


        public MessagingService MessagingService;

        public Guid InstanceId { get; private set; }

        public IAssemblyResolver AssemblyResolver => throw new NotImplementedException();

        public void ConfigureViewModelLocator()
        {
            throw new NotImplementedException();
        }

        public virtual void OnStart()
        {
            InstanceId = Guid.NewGuid();
            MessagingService = new MessagingService(8910, TimeSpan.FromSeconds(2));
            MessagingService.Start(this);
        }

        private void PopulateViewModels()
        {
            ViewModels = new Dictionary<string, ViewModelBase>();

            var subclasses =
            from assembly in AppDomain.CurrentDomain.GetAssemblies()
            from type in assembly.GetTypes()
            where type.IsSubclassOf(typeof(ViewModelBase))
            select type;

            foreach (var sc in subclasses)
            {
                var instance = Activator.CreateInstance(sc) as ViewModelBase;
                ViewModels.Add(sc.Name, instance);
            }
        }

        public void OnStop()
        {
            throw new NotImplementedException();
        }
    }
}
