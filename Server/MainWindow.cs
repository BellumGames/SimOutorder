using System;
using Gtk;

public partial class MainWindow : Gtk.Window
{
    public static string command = "./sim-outorder ";
    public static uint MaxInstr { get; set; }
    public static uint FastFowardCount { get; set; }
    public static string FetchType { get; set; }
    public static uint FetchValue { get; set; }
    public static string BpredMod { get; set; }
    public static uint BpredBimodTableSize { get; set; }
    public static uint Bpred2levL1Size { get; set; }
    public static uint Bpred2levL2Size { get; set; }
    public static uint Bpred2levHistSize { get; set; }
    public static uint BpredCombMetaTableSize { get; set; }
    public static uint BpredRasRasSize { get; set; }
    public static uint BpredBtbNumSets { get; set; }
    public static uint BpredBtbAssociativity { get; set; }
    public static string BpredSpecUpdate { get; set; }

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        Init();
    }

    protected void Init()
    {
        Fetch();
        Bpred();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnBtnHelp(object sender, EventArgs e)
    {
    }

    protected void OnBtnStartServer(object sender, EventArgs e)
    {
    }

    protected void OnBtnSimulate(object sender, EventArgs e)
    {
        PopulateCommand();
    }

    protected void PopulateCommand() 
    {
        MaxInstr = (uint)spinInstrNum.Value;
        FastFowardCount = (uint)spinFastForwardCount.Value;
        command += $"-max:inst {MaxInstr} -fastfwd {FastFowardCount} ";
        if (chkFetch.Active == true) 
        {
            FetchType = comboFetchOption.ActiveText;
            FetchValue = (uint)spinFetch.Value;
            command += $"-fetch:{FetchType} {FetchValue} ";
        }
        if(chkBpred.Active == true) 
        {
            BpredMod = comboBpred.ActiveText;
            command += $"-bpred:{BpredMod} ";
            switch (BpredMod)
            {
                case "bimod":
                    BpredBimodTableSize = (uint)spinBpredBimodTableSize.Value;
                    command += $"{BpredBimodTableSize} ";
                    break;
                case "2lev":
                    Bpred2levL1Size = (uint)spinBpred2levL1Size.Value;
                    Bpred2levL2Size = (uint)spinBpred2levL2Size.Value;
                    Bpred2levHistSize = (uint)spinBpred2levHistSize.Value;
                    command += $"{Bpred2levL1Size} {Bpred2levL2Size} {Bpred2levHistSize} 0 ";
                    break;
                case "comb":
                    BpredCombMetaTableSize = (uint)spinBpredCombMetaTableSize.Value;
                    command += $"{BpredCombMetaTableSize} ";
                    break;
                case "ras":
                    BpredRasRasSize = (uint)spinBpredRasRasSize.Value;
                    command += $"{BpredRasRasSize} ";
                    break;
                case "btb":
                    BpredBtbNumSets = (uint)spinBpredBtbNumSets.Value;
                    BpredBtbAssociativity = (uint)spinBpredBtbAssociativity.Value;
                    command += $"{BpredBtbNumSets} {BpredBtbAssociativity} ";
                    break;
                case "spec_update":
                    BpredSpecUpdate = comboBpredSpecUpdate.ActiveText;
                    if (BpredSpecUpdate == "null") 
                    {
                        BpredSpecUpdate = "";
                    }
                    command += $"{BpredSpecUpdate} ";
                    break;
            }
        }
        //
    }

    protected void OnBtnReset(object sender, EventArgs e)
    {
        Reset();
    }

    protected void Reset() 
    {
        spinInstrNum.Value = 10000;
        spinFastForwardCount.Value = 0;

        chkFetch.Active = false;
        comboFetchOption.Active = 0;
        spinFetch.Value = 4;

        chkBpred.Active = true;
        comboBpred.Active = 0;
        spinBpredBimodTableSize.Value = 512;
        spinBpred2levL1Size.Value = 1;
        spinBpred2levL2Size.Value = 1024;
        spinBpred2levHistSize.Value = 8;
        spinBpredCombMetaTableSize.Value = 1024;
        spinBpredRasRasSize.Value = 8;
        spinBpredBtbNumSets.Value = 512;
        spinBpredBtbAssociativity.Value = 4;

        comboBpredSpecUpdate.Active = 0;
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

            lbBpredRasRasSize.Visible = false;
            spinBpredRasRasSize.Visible = false;

            lbBpredBtbNumSets.Visible = false;
            lbBpredBtbAssociativity.Visible = false;
            spinBpredBtbNumSets.Visible = false;
            spinBpredBtbAssociativity.Visible = false;

            lbBpredSpecUpdate.Visible = false;
            comboBpredSpecUpdate.Visible = false;
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

                lbBpredRasRasSize.Visible = false;
                spinBpredRasRasSize.Visible = false;

                lbBpredBtbNumSets.Visible = false;
                lbBpredBtbAssociativity.Visible = false;
                spinBpredBtbNumSets.Visible = false;
                spinBpredBtbAssociativity.Visible = false;

                lbBpredSpecUpdate.Visible = false;
                comboBpredSpecUpdate.Visible = false;
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

                lbBpredRasRasSize.Visible = false;
                spinBpredRasRasSize.Visible = false;

                lbBpredBtbNumSets.Visible = false;
                lbBpredBtbAssociativity.Visible = false;
                spinBpredBtbNumSets.Visible = false;
                spinBpredBtbAssociativity.Visible = false;

                lbBpredSpecUpdate.Visible = false;
                comboBpredSpecUpdate.Visible = false;
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

                lbBpredRasRasSize.Visible = false;
                spinBpredRasRasSize.Visible = false;

                lbBpredBtbNumSets.Visible = false;
                lbBpredBtbAssociativity.Visible = false;
                spinBpredBtbNumSets.Visible = false;
                spinBpredBtbAssociativity.Visible = false;

                lbBpredSpecUpdate.Visible = false;
                comboBpredSpecUpdate.Visible = false;
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

                lbBpredRasRasSize.Visible = true;
                spinBpredRasRasSize.Visible = true;

                lbBpredBtbNumSets.Visible = false;
                lbBpredBtbAssociativity.Visible = false;
                spinBpredBtbNumSets.Visible = false;
                spinBpredBtbAssociativity.Visible = false;

                lbBpredSpecUpdate.Visible = false;
                comboBpredSpecUpdate.Visible = false;
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

                lbBpredRasRasSize.Visible = false;
                spinBpredRasRasSize.Visible = false;

                lbBpredBtbNumSets.Visible = true;
                lbBpredBtbAssociativity.Visible = true;
                spinBpredBtbNumSets.Visible = true;
                spinBpredBtbAssociativity.Visible = true;

                lbBpredSpecUpdate.Visible = false;
                comboBpredSpecUpdate.Visible = false;
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

                lbBpredRasRasSize.Visible = false;
                spinBpredRasRasSize.Visible = false;

                lbBpredBtbNumSets.Visible = false;
                lbBpredBtbAssociativity.Visible = false;
                spinBpredBtbNumSets.Visible = false;
                spinBpredBtbAssociativity.Visible = false;

                lbBpredSpecUpdate.Visible = true;
                comboBpredSpecUpdate.Visible = true;
                break;
        }
    }
}
