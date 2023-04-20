using System;

namespace Client
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BuildSimCommand simCommand = new BuildSimCommand();
            simCommand.GetPropertis();
        }
    }
}
