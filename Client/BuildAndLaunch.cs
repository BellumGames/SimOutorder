using System;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace Client
{
    [Serializable]
    public class BuildAndLaunch
    {
        private static readonly string PATH_TO_SCRIPT = "../Debug/simplesim-3.0/script.sh";

        private void GenerateScript()
        {
            string commnandBuffer = "./sim-outorder -redir:sim results/simulation.res -redir:prog results/applu_progout.res -max:inst 10000 -fastfwd 0 -fetch:ifqsize 4 -bpred:bimod 2048 -decode:width 4 -issue:width 4 -commit:width 4 -ruu:size 16 -lsq:size 8 -cache:dl1 dl1:128:32:4:l -mem:lat 18 2 -tlb:itlb itlb:16:4096:4:l -res:ialu 4 benchmarks/applu.ss < benchmarks/applu.in";

            try
            {
                File.WriteAllText(PATH_TO_SCRIPT, "#!/bin/bash\nchmod 777 script.sh\ncd /home/timarc/Desktop/MareProiect/SimOutorder/Client/bin/Debug/simplesim-3.0\n" + commnandBuffer);
                File.Exists(PATH_TO_SCRIPT);

            }catch(IOException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void LaunchSim()
        {
            string readScript = File.ReadAllText(PATH_TO_SCRIPT);

            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/bin/bash", Arguments = "./script.sh", WorkingDirectory = "../Debug/simplesim-3.0" };
            Process proc = new Process() { StartInfo = startInfo };
            proc.Start();
            if (proc.HasExited)
            {
                Console.WriteLine("The simulation has ended. Go in peace...");
            }

            Metrics metrics = new Metrics();
            metrics.parseString("../Debug/simplesim-3.0/results/simulation.res");
                       
        }
    }
}


//./sim-outorder -redir:sim results/gcc_1024.res -fastfwd 1000000 -max:inst 10000000 -lvpt:size 1024 benchmarks/applu.ss < benchmarks/applu.in