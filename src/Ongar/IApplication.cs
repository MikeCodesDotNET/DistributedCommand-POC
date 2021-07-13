using Ongar.Modularity;
using System;
using System.Collections.Generic;

namespace Ongar
{
    public interface IApplication
    {
        void OnStart();

        void OnStop();

        /// <summary>
        /// Unique to this device (if single instance only) or app is multiple instances are supported.
        /// </summary>
        Guid InstanceId { get; }

        Dictionary<string, ViewModelBase> ViewModels { get; }

        void ConfigureViewModelLocator();

        void Initialize();


        IAssemblyResolver AssemblyResolver { get; }
    }
}
