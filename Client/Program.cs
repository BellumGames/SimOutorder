using System;
using System.Diagnostics;
using System.IO;

namespace Client
{
    class MainClass
    {
        private static readonly string PATH_TO_SCRIPT = "../Debug/simplesim-3.0/script.sh";
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

        }
        public void GenerateScript()
        {
            string commnandBuffer = "./sim-outorder -redir:sim results/simulation.res -redir:prog results/applu_progout.res -max:inst 10000 -fastfwd 0 -fetch:ifqsize 4 -bpred:bimod 2048 -decode:width 4 -issue:width 4 -commit:width 4 -ruu:size 16 -lsq:size 8 -cache:dl1 dl1:128:32:4:l -mem:lat 18 2 -tlb:itlb itlb:16:4096:4:l -res:ialu 4 benchmarks/applu.ss < benchmarks/applu.in";

            File.WriteAllText(PATH_TO_SCRIPT, "#!/bin/bash\nchmod 777 script.sh\ncd /home/timarc/Desktop/MareProiect/SimOutorder/Client/bin/Debug/simplesim-3.0\n" + commnandBuffer);
            File.Exists(PATH_TO_SCRIPT);
        }
        public void LaunchSim()
        {
            GenerateScript();
            string readScript = File.ReadAllText(PATH_TO_SCRIPT);

            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/bin/bash", Arguments = "./script.sh", WorkingDirectory = "../Debug/simplesim-3.0" };
            Process proc = new Process() { StartInfo = startInfo };
            proc.Start();

            Metrics metrics = new Metrics();
            metrics.parseString("../Debug/simplesim-3.0/results/simulation.res");

        }
    }
}
