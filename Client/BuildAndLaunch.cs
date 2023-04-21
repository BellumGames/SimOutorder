using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

namespace Client
{
    [Serializable]
    public class BuildAndLaunch
    {
        private static readonly string SIMULTOR = "sim-outorder";
        private static readonly string REDIR = "Res";
        private static readonly string PATH_TO_SCRIPT = "../Debug/simplesim-3.0/script.sh";



        private void GenerateScript()
        {

            string commnandBuffer = "./sim-outorder -redir:sim results/gcc_1024.res -fastfwd 1000000 -max:inst 10000000 -lvpt:size 1024 benchmarks/applu.ss < benchmarks/applu.in";

            try
            {
                File.WriteAllText(PATH_TO_SCRIPT, "#!/bin/bash\nchmod 777 script.sh\ncd /home/timarc/Desktop/MareProiect/SimOutorder/Client/bin/Debug/simplesim-3.0\n" + commnandBuffer);
                File.Exists(PATH_TO_SCRIPT);

            }catch(IOException ex)
            {
                Console.WriteLine(ex);
            }

            CreateJsonWithInfo();

        }

        public void LaunchSim()
        {
            GenerateScript();
            string readScript = File.ReadAllText(PATH_TO_SCRIPT);
            //Console.WriteLine(readScript);

            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/bin/bash", Arguments = "./script.sh",WorkingDirectory = "../Debug/simplesim-3.0" };
            Process proc = new Process() { StartInfo = startInfo };
            proc.Start();
            if (proc.HasExited)
            {
                //Console.WriteLine("The simulation has ended. Go in peace...");
            }


        }

        private void CreateJsonWithInfo()
        { 
            string jsonFile = "../../Metrics.json";
            Metrics metrics = new Metrics("10", "100");
            string jsonString = JsonConvert.SerializeObject(metrics);
            File.WriteAllText(jsonFile, jsonString);
            Console.WriteLine(jsonString);

        }
    }
}


//./sim-outorder -redir:sim results/gcc_1024.res -fastfwd 1000000 -max:inst 10000000 -lvpt:size 1024 benchmarks/applu.ss < benchmarks/applu.in