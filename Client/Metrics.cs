﻿using System;
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
        public double rataInlocuireDL1 { get; set; }
        public double rataInlocuireDL2 { get; set; }
        public double rataInlocuireIL1 { get; set; }
        public double rataInlocuireIL2 { get; set; }
        public double rataInlocuireDTLB { get; set; }
        public double rataInlocuireITLB { get; set; }

         


        public Metrics parseString(string simulation)
        {
            Metrics data = new Metrics();
            var values = new[] {
                "il1.accesses", "il1.hits", "il1.replacements",
                "dl1.accesses", "dl1.hits", "dl1.replacements",
                "il2.accesses", "il2.hits", "il2.replacements",
                "dl2.accesses", "dl2.hits", "dl2.replacements",
                "itlb.accesses", "itlb.hits ", "itlb.replacements",
                "dtlb.accesses", "dtlb.hits", "dtlb.replacements" };
            double il1Acc = 0, il2Acc = 0, dl1Acc = 0, dl2Acc = 0, itlbAcc = 0, dtlbAcc = 0;
            double il1Hit = 0, il2Hit = 0, dl1Hit = 0, dl2Hit = 0, itlbHit = 0, dtlbHit = 0;
            double il1Repl = 0, il2Repl = 0, dl1Repl = 0, dl2Repl = 0, itlbRepl = 0, dtlbRepl = 0;

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

                            case "il1.replacements":
                                il1Repl = double.Parse(arguments[1]);
                                break;
                            case "il2.replacements":
                                il2Repl = double.Parse(arguments[1]);
                                break;
                            case "dl1.replacements":
                                dl1Repl = double.Parse(arguments[1]);
                                break;
                            case "dl2.replacements":
                                dl2Repl = double.Parse(arguments[1]);
                                break;
                            case "itlb.replacements":
                                itlbRepl = double.Parse(arguments[1]);
                                break;
                            case "dtlb.replacements":
                                dtlbRepl = double.Parse(arguments[1]);
                                break;
                        }
                    }

                }
            }
            data.rataInlocuireIL1 = Math.Round(il1Repl / il1Acc * 100,2);
            data.rataInlocuireIL2 = Math.Round(il2Repl / il2Acc * 100, 2);
            data.rataInlocuireDL1 = Math.Round(dl1Repl / dl1Acc * 100, 2);
            data.rataInlocuireDL2 = Math.Round(dl2Repl / dl2Acc * 100, 2);
            data.rataInlocuireITLB = Math.Round(itlbRepl / itlbAcc * 100, 2);
            data.rataInlocuireDTLB = Math.Round(dtlbRepl / dtlbAcc * 100, 2);


            data.rataHitDL1 = Math.Round(dl1Hit / dl1Acc * 100, 2);
            data.rataHitDL2 = Math.Round(dl2Hit / dl2Acc * 100, 2);
            data.rataHitIL1 = Math.Round(il1Hit / il1Acc * 100, 2);
            data.rataHitIL2 = Math.Round(il2Hit / il2Acc * 100, 2);
            data.rataHitDTLB = Math.Round(dtlbHit / dtlbAcc * 100, 2);
            data.rataHitITLB = Math.Round(itlbHit / itlbAcc * 100, 2);
            return data;
        }
    }
}
