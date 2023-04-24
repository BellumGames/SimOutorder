using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
namespace Client
{
    public class Metrics
    {

        public double rataHitDL1 { get; set; }
        public double rataHitDL2 { get; set; }
        public double rataHitIL1 { get; set; }
        public double rataHitIL2 { get; set; }
        public double rataHitDTLB { get; set; }
        public double rataHitITLB { get; set; }

        public string benchmarkName { get; set; }
        public double sim_IPC { get; set; }



        public Metrics parseString(string simulation)
        {
            Metrics data = new Metrics();
            var values = new[] {
                "il1.accesses", "il1.hits", 
                "dl1.accesses", "dl1.hits", 
                "il2.accesses", "il2.hits", 
                "dl2.accesses", "dl2.hits",
                "itlb.accesses", "itlb.hits ",
                "dtlb.accesses", "dtlb.hits",
                "sim_IPC",
                "applu","apsi","hydro","go","su2cor","swin","tomcatv","cc1" };
            double il1Acc = 0, il2Acc = 0, dl1Acc = 0, dl2Acc = 0, itlbAcc = 0, dtlbAcc = 0;
            double il1Hit = 0, il2Hit = 0, dl1Hit = 0, dl2Hit = 0, itlbHit = 0, dtlbHit = 0,IPC = 0;
            string benchmark = null;


            Int32 BufferReader = 131072;
            using (StreamReader st = new StreamReader(simulation, Encoding.UTF8,true,BufferReader))
            {
                string line;
                while ((line = st.ReadLine()) != null)
                {
                    if (values.Any(line.Contains))
                    {
                        var arguments = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Take(2).ToArray();
                        switch (arguments[0])
                        {
                            case "il1.accesses":
                                il1Acc = double.Parse(arguments[1]);
                                break;
                            case "il2.accesses":
                                il2Acc = double.Parse(arguments[1]);
                                break;
                            case "dl1.accesses":
                                dl1Acc = double.Parse(arguments[1]);
                                break;
                            case "dl2.accesses":
                                dl2Acc = double.Parse(arguments[1]);
                                break;
                            case "itlb.accesses":
                                itlbAcc = double.Parse(arguments[1]);
                                break;
                            case "dtlb.accesses":
                                dtlbAcc = double.Parse(arguments[1]);
                                break;

                            case "il1.hits":
                                il1Hit = double.Parse(arguments[1]);
                                break;
                            case "il2.hits":
                                il2Hit = double.Parse(arguments[1]);
                                break;
                            case "dl1.hits":
                                dl1Hit = double.Parse(arguments[1]);
                                break;
                            case "dl2.hits":
                                dl2Hit = double.Parse(arguments[1]);
                                break;
                            case "itlb.hits":
                                itlbHit = double.Parse(arguments[1]);
                                break;
                            case "dtlb.hits":
                                dtlbHit = double.Parse(arguments[1]);
                                break;
                            case "sim_IPC":
                                IPC = double.Parse(arguments[1]);
                                break;


                            case "applu":
                                benchmark = arguments[1];
                                break;
                            case "apsi":
                                benchmark = arguments[1];
                                break;
                            case "go":
                                benchmark = arguments[1];
                                break;
                            case "hydro":
                                benchmark = arguments[1];
                                break;
                            case "su2cor":
                                benchmark = arguments[1];
                                break;
                            case "tomcatv":
                                benchmark = arguments[1];
                                break;
                            case "cc1":
                                benchmark = arguments[1];
                                break;

                        }
                    }

                }
            }
            data.rataHitDL1 = Math.Round(dl1Hit / dl1Acc * 100, 2);
            data.rataHitDL2 = Math.Round(dl2Hit / dl2Acc * 100, 2);
            data.rataHitIL1 = Math.Round(il1Hit / il1Acc * 100, 2);
            data.rataHitIL2 = Math.Round(il2Hit / il2Acc * 100, 2);
            data.rataHitDTLB = Math.Round(dtlbHit / dtlbAcc * 100, 2);
            data.rataHitITLB = Math.Round(itlbHit / itlbAcc * 100, 2);

            data.sim_IPC = Math.Round(IPC * 100, 2);
            data.benchmarkName = benchmark;
            Console.WriteLine(data.benchmarkName);
            return data;
        }
    }
}
