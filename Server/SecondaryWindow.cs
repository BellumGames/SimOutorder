using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;

namespace InterfataSimOutorder
{
    public partial class SecondaryWindow : Gtk.Window
    {

        public SecondaryWindow(string result) : base(Gtk.WindowType.Toplevel)
        {
            //string data = $"{metrics.benchmarkName} {metrics.sim_IPC} {metrics.rataHitDL1} 
            //{metrics.rataHitDL2} {metrics.rataHitDTLB} {metrics.rataHitIL1} {metrics.rataHitIL2 } 
            //{metrics.rataHitITLB }";
            string[] data = result.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
            label1.Text = $"Benchmark: {data[0]}";
            label2.Text = $"IPC: {data[1]}";
            label3.Text = $"Rata Hit DL1: {data[2]}";
            label4.Text = $"Rata Hit DL2: {data[3]}";
            label5.Text = $"Rata Hit Data TLB: {data[4]}";
            label6.Text = $"Rata Hit IL1: {data[5]}";
            label7.Text = $"Rata Hit IL2: {data[6]}";
            label8.Text = $"Rata Hit Instruction TLB: {data[7]}";
            this.Build();
        }
    }
}
