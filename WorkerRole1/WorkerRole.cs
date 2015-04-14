using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.ServiceBus.Messaging;


namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        #region Private Constants
        //*******************************
        // Messages & Formats
        //*******************************
        private const string RoleEnvironmentSettingFormat = "Configuration Setting [{0}] = [{1}].";
        private const string RoleEnvironmentConfigurationSettingChangedFormat = "The setting [{0}] is changed: new value = [{1}].";
        private const string RoleEnvironmentConfigurationSettingChangingFormat = "The setting [{0}] is changing: old value = [{1}].";
        private const string RoleEnvironmentTopologyChangedFormat = "The  topology for the [{0}] role is changed.";
        private const string RoleEnvironmentTopologyChangingFormat = "The  topology for the [{0}] role is changing.";
        private const string RoleInstanceCountFormat = "[Role {0}] instance count = [{1}].";
        private const string RoleInstanceEndpointCountFormat = "[Role {0}] instance endpoints count = [{1}].";
        private const string RoleInstanceEndpointFormat = "[Role {0}] instance endpoint [{1}]: protocol = [{2}] address = [{3}] port = [{4}].";
        private const string RoleInstanceIdFormat = "[Role {0}] instance Id = [{1}].";
        private const string RoleInstanceStatusFormat = "[Role {0}] instance Id = [{1}] Status = [{2}].";
        private const string Unknown = "Unknown";
        private const string RegisteringEventProcessor = "Registering Event Processor [EventProcessor]... ";
        private const string EventProcessorRegistered = "Event Processor [EventProcessor] successfully registered. ";

        //*******************************
        // Settings
        //*******************************
        private const string SqlDatabaseConnectionStringSetting = "Server=tcp:vh47syb1gp.database.windows.net,1433;Database=IoTnate;User ID=nmrose@vh47syb1gp;Password=115!Nate;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
        private const string StorageAccountConnectionStringSetting = "http;AccountName=iotspace;AccountKey=JvDBulZEWQ4ep9jOkZZgucti7OPBUQyh82iMWUNxBWURzT57AQtZ6HGBt+9dAGqfCjL1zMNnpfpffOp7qqDI9Q==";
        private const string ServiceBusConnectionStringSetting = "Endpoint=sb://iotnate.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+XRBh06RRMu0/Ew/DSEtdOvOXm7fk9401S/AtmAMEAs=";
        private const string EventHubNameSetting = "ioteventh";
        private const string ConsumerGroupNameSetting = "trolls";
        #endregion

        #region Private Fields
        private string eventHubName;
        private string consumerGroupName;
        private string sqlDatabaseConnectionString;
        private string storageAccountConnectionString;
        private string serviceBusConnectionString;
        private EventProcessorHost eventProcessorHost;
        #endregion

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole1 is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("WorkerRole1 has been started");

            string storage = "DefaultEndpointsProtocol=http;AccountName=iotspace;AccountKey=JvDBulZEWQ4ep9jOkZZgucti7OPBUQyh82iMWUNxBWURzT57AQtZ6HGBt+9dAGqfCjL1zMNnpfpffOp7qqDI9Q==";
            string serviceBus = "Endpoint=sb://iotnate.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+XRBh06RRMu0/Ew/DSEtdOvOXm7fk9401S/AtmAMEAs=";
            string eventHubName = "ioteventh";
            
            EventHubClient client = EventHubClient.CreateFromConnectionString(serviceBus, eventHubName);
            Trace.TraceInformation("Consumer group is: " + client.GetConsumerGroup("trolls"));

             _host = new EventProcessorHost("singleworker", eventHubName, client.GetDefaultConsumerGroup().GroupName, serviceBus, storage);


            return result;
        }

        private EventProcessorHost _host;
       
        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole1 has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
