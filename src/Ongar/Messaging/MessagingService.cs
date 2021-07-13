using Newtonsoft.Json;
using Ongar.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace Ongar.Messaging
{
    public class MessagingService
    {
        public TimeSpan TimeOut { get; private set; }
        IApplication _application;

        Dictionary<string, TCP.Client> ServerToClientConnections { get; }

        public int MessageNumber { get; private set; }

        private TCP.Server _server;
        private int _portNumber;

        public MessagingService(int portNumber, TimeSpan timeout)
        {
            ServerToClientConnections = new Dictionary<string, TCP.Client>();
            TimeOut = timeout;

            _portNumber = portNumber;
            _server = new TCP.Server();

        }


        public void Start(IApplication application)
        {
            MessageNumber = 0;
            _application = application;

            try
            {
                _server.Start(_portNumber);
                _server.ClientConnected += HandleClientConnected;
                _server.ClientDisconnected += HandleClientDisconnected;
                _server.DataReceived += HandleDataReceived;
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message == "Port was already occupied for all network interfaces")
                {
                    //Likely means that this device is already running an instance, thus we should connect to it as a client.
                    var client = new TCP.Client();

                    var homeIP = "127.0.0.1";

                    client.Connect(homeIP, _portNumber);
                    client.DataReceived += HandleDataReceived;
                    System.Diagnostics.Debug.WriteLine("Connected as client");
                    ServerToClientConnections.Add(homeIP, client);
                    SendWelcomeMessage(client);
                }
                else
                    throw exception;
            }
        }

        public void SendMessage<T>(T payload)
        {
            MessageNumber++;
            var msgWrapper = new MessageWrapper() { Message = payload, TypeName = typeof(T).Name, MessageNumber = MessageNumber };

            var json = JsonConvert.SerializeObject(msgWrapper);
            foreach (var connection in ServerToClientConnections)
            {
                connection.Value.WriteLine(json);
            }
        }

        private void HandleDataReceived(object sender, Message e)
        {
            var msgWrapper = JsonConvert.DeserializeObject<MessageWrapper>(e.MessageString);
            if (msgWrapper.TypeName == typeof(WelcomeMessage).Name)
            {
                var welcomeMessage = JsonConvert.DeserializeObject<WelcomeMessage>(msgWrapper.Message.ToString());
                e.ReplyLine("Welcome to the gang");


                var response = new ClusterInformationResponseMessage();                
                e.Reply(response);
            }

            if (msgWrapper.TypeName == typeof(ClusterInformationResponseMessage).Name)
            {

            }


            if (msgWrapper.TypeName == typeof(DistributedCommandExecuted).Name)
            {
                var dce = JsonConvert.DeserializeObject<DistributedCommandExecuted>(msgWrapper.Message.ToString());
                RemoteICommandExecuted(dce);
            }

            if (msgWrapper.TypeName == typeof(ViewModelPropertyDidChangeMessage).Name)
            {
                var vpdc = JsonConvert.DeserializeObject<ViewModelPropertyDidChangeMessage>(msgWrapper.Message.ToString());

                HandleRemotePropertyChange(vpdc);

                Console.WriteLine(vpdc.NewValue);
            }

        }

        private void HandleClientDisconnected(object sender, System.Net.Sockets.TcpClient e)
        {
    
        }

        private void HandleClientConnected(object sender, System.Net.Sockets.TcpClient e)
        {
           
        }


        private void RemoteICommandExecuted(DistributedCommandExecuted message)
        {
            ViewModelBase vm;
            _application.ViewModels.TryGetValue(message.Sender.ToString(), out vm);

            if (vm != null)
            {
                //PropertyInfo property = vm.GetType().GetProperty(message.TargetCommand);
                //MethodInfo method = property.get
                //method.Invoke(vm, null);


                var miHandler = vm.GetType().GetProperty(message.TargetCommand);
                var command = miHandler.GetValue(vm) as ICommand;

                command.Execute("remote");
            }

            Console.WriteLine($"{message.Sender}.{message.TargetCommand} | {message.ExecutedAt}");
        }


        private void HandleRemotePropertyChange(ViewModelPropertyDidChangeMessage viewModelPropertyDidChangeMessage)
        {
            ViewModelBase vm;
            _application.ViewModels.TryGetValue(viewModelPropertyDidChangeMessage.ViewModelName, out vm);

            if (vm != null)
            {
                var miHandler = vm.GetType().GetProperty(viewModelPropertyDidChangeMessage.FieldName);
                miHandler.SetValue(vm, viewModelPropertyDidChangeMessage.NewValue);
            }

        }


        /// <summary>
        /// When an application connects as a client to a server, it'll introduce itself using this method.  
        /// </summary>
        private void SendWelcomeMessage(TCP.Client client)
        {
            SendMessage(new Messages.WelcomeMessage() { Id = _application.InstanceId });
        }
    }
}
