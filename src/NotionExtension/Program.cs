using System;
using System.Threading;
using Microsoft.CommandPalette.Extensions;

namespace NotionExtension;

public class Program
{
    [MTAThread]
    public static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "-RegisterProcessAsComServer")
        {
            using ExtensionServer server = new();
            var extensionDisposedEvent = new ManualResetEvent(false);
            var extension = new NotionExtension(extensionDisposedEvent);

            server.RegisterExtension(() => extension);

            extensionDisposedEvent.WaitOne();
        }
        else
        {
            Console.WriteLine("Not being launched as a Extension... exiting.");
        }
    }
}
