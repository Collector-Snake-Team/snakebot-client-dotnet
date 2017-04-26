using System;
using Cygni.Snake.Client;

namespace Cygni.Snake.SampleBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var training = true;
            var serverUri = new Uri("ws://snake.cygni.se:80");


            if(training)
                serverUri = new Uri(serverUri, "training");
            else
                serverUri = new Uri(serverUri, "tournament");


            var client = SnakeClient.Connect(serverUri, new OpenBrowser());
            client.Start(new MySnakeBot("dotnetSnake")
            {
                AutoStart = false
            });
            Console.ReadLine();
        }
    }
}