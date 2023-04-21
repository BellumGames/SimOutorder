using System;

namespace Client
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BuildAndLaunch simCommand = new BuildAndLaunch();
            simCommand.LaunchSim();
        }
    }
}
