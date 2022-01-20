using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace HttpEdit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var record = new Command("record", "Listen to http request and get plain text TCP buffer.");
            record.AddArgument(new Argument<string>("target", "Bind address on which to open listening socket."));

            record.Handler = CommandHandler.Create<string>(HandleRecord);

            var send = new Command("send", "Send plain text http request to specified server.");
            send.AddArgument(new Argument<string>("target", "Bind address on which to open listening socket."));
            send.AddArgument(new Argument<string>("file", "File to read HTTP request buffer from."));

            send.Handler = CommandHandler.Create<string, string>(HandleSend);

            var rootCommand = new RootCommand()
            {
                record,
                send
            };

            try
            {
                rootCommand.Invoke(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Environment.Exit(1);
            }
        }

        public static void HandleRecord(string target)
        {
            // Open TCP listener
            // Handle incoming request
            // Print request to stdout
            string request = new TcpHandler().RecordRequest(target);
            Console.WriteLine(request);
        }

        public static void HandleSend(string target, string file)
        {
            // Read file from fs
            string filePath = Path.Combine(Environment.CurrentDirectory, file);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File does not exist: {file}");

            byte[] buf = File.ReadAllBytes(filePath);

            // Open TCP connection
            // Send file
            // Wait for response
            string response = new TcpHandler().SendRequest(buf, target);

            // Print response to stdout
            Console.WriteLine(response);
        }
    }
}