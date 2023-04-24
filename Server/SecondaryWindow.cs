using System;
namespace InterfataSimOutorder
{
    public partial class SecondaryWindow : Gtk.Window
    {
        public static string official;

        public SecondaryWindow(string result) : base(Gtk.WindowType.Toplevel)
        {
            this.Build();
            official = result;
            lbResult.Text = result;
        }
    }
}
