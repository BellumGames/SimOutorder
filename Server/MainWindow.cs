using System;
using Gtk;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        Init();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnBtnSimulate(object sender, EventArgs e)
    {
    }

    protected void OnBtnReset(object sender, EventArgs e)
    {
    }

    protected void OnBtnHelp(object sender, EventArgs e)
    {
    }

    protected void OnBtnStartServer(object sender, EventArgs e)
    {
    }

    protected void OnChkPtrace(object sender, EventArgs e)
    {
        Ptrace();
    }

    protected void OnChkFetch(object sender, EventArgs e)
    {
        Fetch();
    }

    protected void Init()
    {
        Ptrace();
        Fetch();
    }

    protected void Ptrace()
    {
        if (chkPtrace.Active == true)
        {
            entPraceName.Visible = true;
            entPtraceRange.Visible = true;
            lbPtraceName.Visible = true;
            lbPtraceRange.Visible = true;
        }
        else
        {
            entPraceName.Visible = false;
            entPtraceRange.Visible = false;
            lbPtraceName.Visible = false;
            lbPtraceRange.Visible = false;
        }
    }

    protected void Fetch() 
    { 
        if(chkFetch.Active == true) 
        {
            comboFetchOption.Visible = true;
            spinFetch.Visible = true;
            lbFetchOption.Visible = true;
            lbFetchValue.Visible = true;
        }
        else 
        {
            comboFetchOption.Visible = false;
            spinFetch.Visible = false;
            lbFetchOption.Visible = false;
            lbFetchValue.Visible = false;
        }
    }
}
