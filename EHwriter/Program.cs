using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;

namespace EHwriter
{
    class Program
    {

        static string eventHubName = "ioteventh";
        static string connectionString = "Endpoint=sb://iotnate.servicebus.windows.net/;SharedAccessKeyName=sender;SharedAccessKey=lQ4yQe61GSoWi2kH38t/1d372cKhIj/EgKk67n4dFPo=";
        
static async Task SendingRandomMessages()
{
    var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
    while (true)
    {
        try
        {
            var message = Guid.NewGuid().ToString();
            Console.WriteLine("{0} > Sending message: {1}", DateTime.Now.ToString(), message);
            await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
        }
        catch (Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0} > Exception: {1}", DateTime.Now.ToString(), exception.Message);
            Console.ResetColor();
        }

        await Task.Delay(200);
    }
}
        
        static void Main(string[] args)
        {
            Console.WriteLine("Press Ctrl-C to stop the sender process");
            Console.WriteLine("Press Enter to start now");
            Console.ReadLine();
            SendingRandomMessages().Wait();
        }
    }
}
