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
        Reset();
    }

    protected void Reset() 
    {
        spinInstrNum.Value = 10000;
        spinFastForwardCount.Value = 0;

        chkPtrace.Active = false;
        entPraceName.Text = "FOO.trc";
        entPtraceRange.Text = "#0:#1000";

        chkFetch.Active = false;
        comboFetchOption.Active = 0;
        spinFetch.Value = 0;

        chkBpred.Active = true;
        comboBpred.Active = 0;
        spinBpredBimodTableSize.Value = 512;
        spinBpred2levL1Size.Value = 32;
        spinBpred2levL2Size.Value = 64;
        spinBpred2levHistSize.Value = 0;
        spinBpredCombMetaTableSize.Value = 512;
    }

    protected void OnBtnStartServer(object sender, EventArgs e)
    {
    }

    protected void OnBtnHelp(object sender, EventArgs e)
    {
    }

    protected void Init()
    {
        Ptrace();
        Fetch();
        Bpred();
    }

    protected void OnChkPtrace(object sender, EventArgs e)
    {
        Ptrace();
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

    protected void OnChkFetch(object sender, EventArgs e)
    {
        Fetch();
    }
    protected void Fetch()
    {
        if (chkFetch.Active == true)
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

    protected void OnChkBpred(object sender, EventArgs e)
    {
        Bpred();
    }

    protected void Bpred() 
    { 
        if(chkBpred.Active == true) 
        {
            lbBpred.Visible = true;
            comboBpred.Visible = true;
            ComboBpred();
        }
        else 
        {
            lbBpred.Visible = false;
            comboBpred.Visible = false;
            lbBpredBimodTableSize.Visible = false;
            spinBpredBimodTableSize.Visible = false;

            lbBpred2levL1Size.Visible = false;
            lbBpred2levL2Size.Visible = false;
            lbBpred2levHistSize.Visible = false;
            spinBpred2levL1Size.Visible = false;
            spinBpred2levL2Size.Visible = false;
            spinBpred2levHistSize.Visible = false;

            lbBpredCombMetaTableSize.Visible = false;
            spinBpredCombMetaTableSize.Visible = false;
        }
    }

    protected void OnComboBpred(object sender, EventArgs e)
    {
        ComboBpred();
    }

    protected void ComboBpred() 
    {
        switch (comboBpred.ActiveText)
        {
            case "bimod":
                lbBpredBimodTableSize.Visible = true;
                spinBpredBimodTableSize.Visible = true;

                lbBpred2levL1Size.Visible = false;
                lbBpred2levL2Size.Visible = false;
                lbBpred2levHistSize.Visible = false;
                spinBpred2levL1Size.Visible = false;
                spinBpred2levL2Size.Visible = false;
                spinBpred2levHistSize.Visible = false;

                lbBpredCombMetaTableSize.Visible = false;
                spinBpredCombMetaTableSize.Visible = false;
                break;
            case "2lev":
                lbBpredBimodTableSize.Visible = false;
                spinBpredBimodTableSize.Visible = false;

                lbBpred2levL1Size.Visible = true;
                lbBpred2levL2Size.Visible = true;
                lbBpred2levHistSize.Visible = true;
                spinBpred2levL1Size.Visible = true;
                spinBpred2levL2Size.Visible = true;
                spinBpred2levHistSize.Visible = true;

                lbBpredCombMetaTableSize.Visible = false;
                spinBpredCombMetaTableSize.Visible = false;
                break;
            case "comb":
                lbBpredBimodTableSize.Visible = false;
                spinBpredBimodTableSize.Visible = false;

                lbBpred2levL1Size.Visible = false;
                lbBpred2levL2Size.Visible = false;
                lbBpred2levHistSize.Visible = false;
                spinBpred2levL1Size.Visible = false;
                spinBpred2levL2Size.Visible = false;
                spinBpred2levHistSize.Visible = false;

                lbBpredCombMetaTableSize.Visible = true;
                spinBpredCombMetaTableSize.Visible = true;
                break;
            case "ras":
                lbBpredBimodTableSize.Visible = false;
                spinBpredBimodTableSize.Visible = false;

                lbBpred2levL1Size.Visible = false;
                lbBpred2levL2Size.Visible = false;
                lbBpred2levHistSize.Visible = false;
                spinBpred2levL1Size.Visible = false;
                spinBpred2levL2Size.Visible = false;
                spinBpred2levHistSize.Visible = false;

                lbBpredCombMetaTableSize.Visible = false;
                spinBpredCombMetaTableSize.Visible = false;
                break;
            case "btb":
                lbBpredBimodTableSize.Visible = false;
                spinBpredBimodTableSize.Visible = false;

                lbBpred2levL1Size.Visible = false;
                lbBpred2levL2Size.Visible = false;
                lbBpred2levHistSize.Visible = false;
                spinBpred2levL1Size.Visible = false;
                spinBpred2levL2Size.Visible = false;
                spinBpred2levHistSize.Visible = false;

                lbBpredCombMetaTableSize.Visible = false;
                spinBpredCombMetaTableSize.Visible = false;
                break;
            case "spec_update":
                lbBpredBimodTableSize.Visible = false;
                spinBpredBimodTableSize.Visible = false;

                lbBpred2levL1Size.Visible = false;
                lbBpred2levL2Size.Visible = false;
                lbBpred2levHistSize.Visible = false;
                spinBpred2levL1Size.Visible = false;
                spinBpred2levL2Size.Visible = false;
                spinBpred2levHistSize.Visible = false;

                lbBpredCombMetaTableSize.Visible = false;
                spinBpredCombMetaTableSize.Visible = false;
                break;
        }
    }
}
