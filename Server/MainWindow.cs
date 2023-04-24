using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Gtk;
using InterfataSimOutorder;

public partial class MainWindow : Gtk.Window
{
    public static string command = "./sim-outorder ";
    public static Dictionary<string, string> benchmarks = new Dictionary<string, string>();
    public static List<string> commands = new List<string>();
    public static string PATH_CLIENT = @"..\..\..\Client\bin\Debug\Client.exe";
    public static int ServerStatus = 0;
    public static Thread th = null;
    public static int i = 0;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        Init();
        InitDic();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
        th.Abort();
    }

    protected void Init()
    {
        lbConsole.Text += "\n";
        Fetch();
        Bpred();
        Decode();
        Commit();
        Issue();
        Ruu();
        Lsq();
        DL1();
        DL2();
        IL1();
        IL2();
        DTLB();
        ITLB();
        CacheFlush();
        CacheIcompress();
        MemLat();
        MemWidth();
    }

    protected void InitDic() 
    {
        benchmarks.Add("applu", "benchmarks/applu.ss < benchmarks/applu.in");
        benchmarks.Add("apsi", "benchmarks/apsi.ss < benchmarks/apsi.in");
        benchmarks.Add("hydro2d", "benchmarks/hydro2d.ss < benchmarks/hydro2d.in");
        benchmarks.Add("go", "benchmarks/go.ss < benchmarks/9stone21.in");
        benchmarks.Add("su2cor", "benchmarks/su2cor.ss < benchmarks/su2cor.in");
        benchmarks.Add("swim", "benchmarks/swim.ss < benchmarks/swim2.in");
        benchmarks.Add("tomcatv", "benchmarks/tomcatv.ss < benchmarks/tomcatv.in");
        benchmarks.Add("cc1", "benchmarks/cc1.ss < benchmarks/1stmt.i");
    }

    protected void OnBtnHelp(object sender, EventArgs e)
    {
    }

    protected void OnBtnPopulate(object sender, EventArgs e)
    {
        PopulateCommand();
        PreparingCommand();
        btnStartServer.Sensitive = true;
    }

    protected void OnBtnStartServer(object sender, EventArgs e)
    {
        btnSimulate.Sensitive = true;
        ServerThread();
    }

    protected void OnBtnSimulate(object sender, EventArgs e)
    {
    }

    protected void ServerThread() 
    {
        if (ServerStatus == 0)
        {
            th = new Thread(Net);
            th.Start();
            ServerStatus = 1;
            StartClients();
        }
        else
        {
            th.Abort();
            ServerStatus = 0;
        }
    }

    protected void Net() 
    {
        // Create a server Socket
        System.Net.Sockets.Socket serverSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        serverSocket.Bind(endPoint);
        serverSocket.Listen(10);

        lbConsole.Text += "Server started. Waiting for clients...\n";

        while (true)
        {
            // Accept incoming client connections
            System.Net.Sockets.Socket clientSocket = serverSocket.Accept();
            lbConsole.Text += "Client connected: " + clientSocket.RemoteEndPoint + "\n";

            // Handle client communication in a separate thread
            // (you can use Task.Run or other threading mechanisms)
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = clientSocket.Receive(buffer)) > 0)
            {
                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                lbConsole.Text += "Received data from client: " + receivedData + "\n";

                // Send response back to client
                //./sim-outorder -max:inst 10000 -fastfwd 0 -bpred:bimod 512 -cache:dl1 dl1:128:32:4:l -cache:dl1lat 1 -cache:dl2 dl2:1024:64:4:l -cache:dl2lat 6 -cache:il1 il1:512:32:1:l -cache:il1lat 1 -cache:il2 il2:1024:64:4:l -cache:il2lat 6 -tlb:dtlb dtlb:32:4096:4:l -tlb:itlb itlb:16:4096:4:l-tlb:lat 30 -redir:sim results/applu_simout.res benchmarks/applu.ss < benchmarks/applu.in && exit
                string response = commands[i];
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                clientSocket.Send(responseBytes);
            }
            // Close the client socket
            clientSocket.Close();
        }
    }

    protected void StartClients() 
    {
        try 
        {
            Process.Start(PATH_CLIENT);
        } 
        catch (Exception ex)
        {
            lbConsole.Text += "Failed to launch Client.exe: " + ex.Message + "\n";
        }
    }

    protected void PreparingCommand() 
    {
        foreach(var item in benchmarks) 
        { 
            string temp = command + $"-redir:sim results/{item.Key}_simout.res {item.Value} && exit";
            commands.Add(temp);
        }
        lbConsole.Text = commands[0];
    }

    protected void PopulateCommand()
    {
        command += $"-max:inst {spinInstrNum.Value} -fastfwd {spinFastForwardCount.Value} ";
        if (chkFetch.Active == true)
        {
            command += $"-fetch:{comboFetchOption.ActiveText} {spinFetch.Value} ";
        }
        if (chkBpred.Active == true)
        {
            command += $"-bpred:{comboBpred.ActiveText} ";
            switch (comboBpred.ActiveText)
            {
                case "bimod":
                    command += $"{spinBpredBimodTableSize.Value} ";
                    break;
                case "2lev":
                    command += $"{spinBpred2levL1Size.Value} {spinBpred2levL2Size.Value} {spinBpred2levHistSize.Value} 0 ";
                    break;
                case "comb":
                    command += $"{spinBpredCombMetaTableSize.Value} ";
                    break;
                case "ras":
                    command += $"{spinBpredRasRasSize.Value} ";
                    break;
                case "btb":
                    command += $"{spinBpredBtbNumSets.Value} {spinBpredBtbAssociativity.Value} ";
                    break;
                case "spec_update":
                    string BpredSpecUpdate = comboBpredSpecUpdate.ActiveText;
                    if (BpredSpecUpdate == "null")
                    {
                        BpredSpecUpdate = "";
                    }
                    command += $"{BpredSpecUpdate} ";
                    break;
            }
        }
        if (chkDecode.Active == true)
        {
            command += $"-decode:width {spinDecodeWidth.Value} ";
        }
        if (chkCommit.Active == true)
        {
            command += $"-commit:width {spinCommitWidth.Value} ";
        }
        if (chkIssue.Active == true)
        {
            command += $"-issue:{comboIssue.ActiveText} ";
            switch (comboIssue.ActiveText)
            {
                case "width":
                    command += $"{spinIssueWidth.Value} ";
                    break;
                case "inorder":
                    command += $"{comboIssueInorder.ActiveText} ";
                    break;
                case "wrongpath":
                    command += $"{comboIssueWrongPath.ActiveText} ";
                    break;
            }
        }
        if (chkRuu.Active == true)
        {
            command += $"-ruu:size {spinRuuSize.Value} ";
        }
        if (chkLsq.Active == true)
        {
            command += $"-lsq:size {spinLsqSize.Value} ";
        }
        CacheConfig();
        if(chkCacheFlush.Active == true) 
        {
            command += $"-cache:flush {comboCacheFlush.ActiveText} ";
        }
        if(chkCacheIcompress.Active == true) 
        {
            command += $"-cache:icompress {comboCacheIcompress.ActiveText} ";
        }
        if (chkMemLat.Active) 
        {
            command += $"-mem:lat {spinMemLatFirstChunk.Value} {spinMemLatInterChunk.Value} ";
        }
        if(chkMemWidth.Active == true) 
        {
            command += $"-mem:width {spinMemWidth.Value} "; 
        }
        lbConsole.Text = command;
    }

    protected void CacheConfig() 
    {
        string dl1Name = "dl1";
        string dl2Name = "dl2";
        string il1Name = "il1";
        string il2Name = "il2";
        string dtlbName = "dtlb";
        string itlbName = "itlb";

        string replDL1 = Repl(comboReplacementDL1.ActiveText);
        string replDL2 = Repl(comboReplacementDL2.ActiveText);
        string replIL1 = Repl(comboReplacementIL1.ActiveText);
        string replIL2 = Repl(comboReplacementIL2.ActiveText);
        string replDTLB = Repl(comboReplacementDTLB.ActiveText);
        string replITLB = Repl(comboReplacementITLB.ActiveText);

        if (chkUnifiedIL1.Active) 
        {
            dl1Name = "ul1";
            dl2Name = "ul2";
            if (chkDL1.Active) 
            {
                command += $"-cache:dl1 {dl1Name}:{spinNumSetsDL1.Value}:{spinBlockSizeDL1.Value}:{spinAssociativityDL1.Value}:{replDL1} ";
                command += $"-cache:dl1lat {spinLatDL1.Value} ";
            }
            if (chkDL2.Active) 
            {
                command += $"-cache:dl2 {dl2Name}:{spinNumSetsDL2.Value}:{spinBlockSizeDL2.Value}:{spinAssociativityDL2.Value}:{replDL2} ";
                command += $"-cache:dl2lat {spinLatDL2.Value} ";
            }
            if (chkIL1.Active) 
            {
                command += $"-cache:il1 dl1 ";
                command += $"-cache:il1lat {spinLatIL1.Value} ";
            }
            //if (chkIL2.Active) { }
        }
        else if (chkUnifiedIL2.Active)
        {
            dl2Name = "ul2";
            if (chkDL1.Active)
            {
                command += $"-cache:dl1 {dl1Name}:{spinNumSetsDL1.Value}:{spinBlockSizeDL1.Value}:{spinAssociativityDL1.Value}:{replDL1} ";
                command += $"-cache:dl1lat {spinLatDL1.Value} ";
            }
            if (chkDL2.Active)
            {
                command += $"-cache:dl2 {dl2Name}:{spinNumSetsDL2.Value}:{spinBlockSizeDL2.Value}:{spinAssociativityDL2.Value}:{replDL2} ";
                command += $"-cache:dl2lat {spinLatDL2.Value} ";
            }
            if (chkIL1.Active)
            {
                command += $"-cache:il1 {il1Name}:{spinNumSetsIL1.Value}:{spinBlockSizeIL1.Value}:{spinAssociativityIL1.Value}:{replIL1} ";
                command += $"-cache:il1lat {spinLatIL1.Value} ";
            }
            if (chkIL2.Active)
            {
                command += $"-cache:il2 dl2 ";
                command += $"-cache:il2lat {spinLatIL2.Value} ";
            }
        }
        else
        {
            if (chkDL1.Active)
            {
                command += $"-cache:dl1 {dl1Name}:{spinNumSetsDL1.Value}:{spinBlockSizeDL1.Value}:{spinAssociativityDL1.Value}:{replDL1} ";
                command += $"-cache:dl1lat {spinLatDL1.Value} ";
            }
            if (chkDL2.Active)
            {
                command += $"-cache:dl2 {dl2Name}:{spinNumSetsDL2.Value}:{spinBlockSizeDL2.Value}:{spinAssociativityDL2.Value}:{replDL2} ";
                command += $"-cache:dl2lat {spinLatDL2.Value} ";
            }
            if (chkIL1.Active)
            {
                command += $"-cache:il1 {il1Name}:{spinNumSetsIL1.Value}:{spinBlockSizeIL1.Value}:{spinAssociativityIL1.Value}:{replIL1} ";
                command += $"-cache:il1lat {spinLatIL1.Value} ";
            }
            if (chkIL2.Active)
            {
                command += $"-cache:il2 {il2Name}:{spinNumSetsIL2.Value}:{spinBlockSizeIL2.Value}:{spinAssociativityIL2.Value}:{replIL2} ";
                command += $"-cache:il2lat {spinLatIL2.Value} ";
            }
        }

        if (chkDTLB.Active)
        {
            command += $"-tlb:dtlb {dtlbName}:{spinNumSetsDTLB.Value}:{spinBlockSizeDTLB.Value}:{spinAssociativityDTLB.Value}:{replDTLB} ";
        }

        if (chkITLB.Active)
        {
            command += $"-tlb:itlb {itlbName}:{spinNumSetsITLB.Value}:{spinBlockSizeITLB.Value}:{spinAssociativityITLB.Value}:{replITLB} ";
        }

        if (chkDTLB.Active || chkITLB.Active)
        {
            command += $"-tlb:lat {spinLatTLB.Value} ";
        }
    }

    protected string Repl(string chk) 
    {
        return chk == "LRU" ? "l" : comboReplacementDL1.ActiveText == "FIFO" ? "f" : "r";
    }

    protected void OnBtnReset(object sender, EventArgs e)
    {
        Reset();
    }

    protected void Reset()
    {
        command = "./sim-outorder ";

        if(th.IsAlive == true) 
        {
            th.Abort();
        }
        btnStartServer.Sensitive = false;
        btnSimulate.Sensitive = false;

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

        chkDecode.Active = false;
        spinDecodeWidth.Value = 4;

        chkCommit.Active = false;
        spinCommitWidth.Value = 4;

        chkIssue.Active = false;
        spinIssueWidth.Value = 4;
        comboIssueInorder.Active = 0;
        comboIssueWrongPath.Active = 0;

        chkRuu.Active = false;
        spinRuuSize.Value = 16;

        chkLsq.Active = false;
        spinLsqSize.Value = 8;

        chkDL1.Active = true;
        spinNumSetsDL1.Value = 128;
        spinBlockSizeDL1.Value = 32;
        spinAssociativityDL1.Value = 4;
        comboReplacementDL1.Active = 0;
        spinLatDL1.Value = 1;

        chkDL2.Active = true;
        spinNumSetsDL2.Value = 1024;
        spinBlockSizeDL2.Value = 64;
        spinAssociativityDL2.Value = 4;
        comboReplacementDL2.Active = 0;
        spinLatDL2.Value = 6;

        chkIL1.Active = true;
        spinNumSetsIL1.Value = 512;
        spinBlockSizeIL1.Value = 32;
        spinAssociativityIL1.Value = 1;
        comboReplacementIL1.Active = 0;
        spinLatIL1.Value = 1;
        chkUnifiedIL1.Active = false;

        chkIL2.Active = true;
        spinNumSetsIL2.Value = 1024;
        spinBlockSizeIL2.Value = 64;
        spinAssociativityIL2.Value = 4;
        comboReplacementIL2.Active = 0;
        spinLatIL2.Value = 6;
        chkUnifiedIL2.Active = false;

        spinLatTLB.Value = 30;

        chkDTLB.Active = true;
        spinNumSetsDTLB.Value = 32;
        spinBlockSizeDTLB.Value = 4096;
        spinAssociativityDTLB.Value = 4;
        comboReplacementDTLB.Active = 0;

        chkITLB.Active = true;
        spinNumSetsITLB.Value = 16;
        spinBlockSizeITLB.Value = 4096;
        spinAssociativityITLB.Value = 4;
        comboReplacementITLB.Active = 0;

        chkCacheFlush.Active = false;
        comboCacheFlush.Active = 0;

        chkCacheIcompress.Active = false;
        comboCacheIcompress.Active = 0;

        chkMemLat.Active = false;
        spinMemLatFirstChunk.Value = 18;
        spinMemLatInterChunk.Value = 2;

        chkMemWidth.Active = false;
        spinMemWidth.Value = 0;
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
        if (chkBpred.Active == true)
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

    protected void OnChkDecode(object sender, EventArgs e)
    {
        Decode();
    }

    protected void Decode()
    {
        if (chkDecode.Active == true)
        {
            lbDecodeWidth.Visible = true;
            spinDecodeWidth.Visible = true;
        }
        else
        {
            lbDecodeWidth.Visible = false;
            spinDecodeWidth.Visible = false;
        }
    }

    protected void OnChkCommit(object sender, EventArgs e)
    {
        Commit();
    }

    protected void Commit()
    {
        if (chkCommit.Active == true)
        {
            lbCommitWidth.Visible = true;
            spinCommitWidth.Visible = true;
        }
        else
        {
            lbCommitWidth.Visible = false;
            spinCommitWidth.Visible = false;
        }
    }

    protected void OnChkIssue(object sender, EventArgs e)
    {
        Issue();
    }

    protected void Issue()
    {
        if (chkIssue.Active == true)
        {
            lbIssue.Visible = true;
            comboIssue.Visible = true;
            ComboIssue();
        }
        else
        {
            lbIssue.Visible = false;
            comboIssue.Visible = false;

            lbIssueWidth.Visible = false;
            spinIssueWidth.Visible = false;

            lbIssueInorder.Visible = false;
            comboIssueInorder.Visible = false;

            lbIssueWrongPath.Visible = false;
            comboIssueWrongPath.Visible = false;
        }
    }

    protected void OnComboIssue(object sender, EventArgs e)
    {
        ComboIssue();
    }

    protected void ComboIssue()
    {
        switch (comboIssue.ActiveText)
        {
            case "width":
                lbIssueWidth.Visible = true;
                spinIssueWidth.Visible = true;

                lbIssueInorder.Visible = false;
                comboIssueInorder.Visible = false;

                lbIssueWrongPath.Visible = false;
                comboIssueWrongPath.Visible = false;
                break;
            case "inorder":
                lbIssueWidth.Visible = false;
                spinIssueWidth.Visible = false;

                lbIssueInorder.Visible = true;
                comboIssueInorder.Visible = true;

                lbIssueWrongPath.Visible = false;
                comboIssueWrongPath.Visible = false;
                break;
            case "wrongpath":
                lbIssueWidth.Visible = false;
                spinIssueWidth.Visible = false;

                lbIssueInorder.Visible = false;
                comboIssueInorder.Visible = false;

                lbIssueWrongPath.Visible = true;
                comboIssueWrongPath.Visible = true;
                break;
        }
    }

    protected void OnChkRuu(object sender, EventArgs e)
    {
        Ruu();
    }

    protected void Ruu()
    {
        if (chkRuu.Active == true)
        {
            lbRuuSize.Visible = true;
            spinRuuSize.Visible = true;
        }
        else
        {
            lbRuuSize.Visible = false;
            spinRuuSize.Visible = false;
        }
    }

    protected void OnChkLsq(object sender, EventArgs e)
    {
        Lsq();
    }

    protected void Lsq()
    {
        DL2();
        if (chkLsq.Active == true)
        {
            lbLsqSize.Visible = true;
            spinLsqSize.Visible = true;
        }
        else
        {
            lbLsqSize.Visible = false;
            spinLsqSize.Visible = false;
        }
    }

    protected void OnChkDL1(object sender, EventArgs e)
    {
        DL1();
    }

    protected void DL1() 
    {
        if(chkDL1.Active == true) 
        {
            lbNumSetsDL1.Visible = true;
            lbBlockSizeDL1.Visible = true;
            lbAssociativityDL1.Visible = true;
            lbReplacementDL1.Visible = true;
            lbLatDL1.Visible = true;

            spinNumSetsDL1.Visible = true;
            spinBlockSizeDL1.Visible = true;
            spinAssociativityDL1.Visible = true;
            comboReplacementDL1.Visible = true;
            spinLatDL1.Visible = true;
        }
        else 
        {
            lbNumSetsDL1.Visible = false;
            lbBlockSizeDL1.Visible = false;
            lbAssociativityDL1.Visible = false;
            lbReplacementDL1.Visible = false;
            lbLatDL1.Visible = false;

            spinNumSetsDL1.Visible = false;
            spinBlockSizeDL1.Visible = false;
            spinAssociativityDL1.Visible = false;
            comboReplacementDL1.Visible = false;
            spinLatDL1.Visible = false;
        }
    }

    protected void OnChkDL2(object sender, EventArgs e)
    {
        DL2();
    }

    protected void DL2() 
    {
        if (chkDL2.Active == true)
        {
            lbNumSetsDL2.Visible = true;
            lbBlockSizeDL2.Visible = true;
            lbAssociativityDL2.Visible = true;
            lbReplacementDL2.Visible = true;
            lbLatDL2.Visible = true;

            spinNumSetsDL2.Visible = true;
            spinBlockSizeDL2.Visible = true;
            spinAssociativityDL2.Visible = true;
            comboReplacementDL2.Visible = true;
            spinLatDL2.Visible = true;
        }
        else
        {
            lbNumSetsDL2.Visible = false;
            lbBlockSizeDL2.Visible = false;
            lbAssociativityDL2.Visible = false;
            lbReplacementDL2.Visible = false;
            lbLatDL2.Visible = false;

            spinNumSetsDL2.Visible = false;
            spinBlockSizeDL2.Visible = false;
            spinAssociativityDL2.Visible = false;
            comboReplacementDL2.Visible = false;
            spinLatDL2.Visible = false;
        }
    }

    protected void OnChkIL1(object sender, EventArgs e)
    {
        IL1();
    }

    protected void IL1() 
    {
        UnifiedIL1();
        if (chkIL1.Active == true)
        {
            lbNumSetsIL1.Visible = true;
            lbBlockSizeIL1.Visible = true;
            lbAssociativityIL1.Visible = true;
            lbReplacementIL1.Visible = true;
            lbLatIL1.Visible = true;

            spinNumSetsIL1.Visible = true;
            spinBlockSizeIL1.Visible = true;
            spinAssociativityIL1.Visible = true;
            comboReplacementIL1.Visible = true;
            spinLatIL1.Visible = true;

            chkUnifiedIL1.Visible = true;
        }
        else
        {
            lbNumSetsIL1.Visible = false;
            lbBlockSizeIL1.Visible = false;
            lbAssociativityIL1.Visible = false;
            lbReplacementIL1.Visible = false;
            lbLatIL1.Visible = false;

            spinNumSetsIL1.Visible = false;
            spinBlockSizeIL1.Visible = false;
            spinAssociativityIL1.Visible = false;
            comboReplacementIL1.Visible = false;
            spinLatIL1.Visible = false;

            chkUnifiedIL1.Visible = false;
        }
    }

    protected void OnUnifiedIL1(object sender, EventArgs e)
    {
        UnifiedIL1();
    }

    protected void UnifiedIL1()
    {
        if (chkUnifiedIL1.Active == true)
        {
            spinNumSetsIL1.Visible = false;
            spinBlockSizeIL1.Visible = false;
            spinAssociativityIL1.Visible = false;
            comboReplacementIL1.Visible = false;
            spinLatIL1.Visible = false;
        }
        else
        {
            spinNumSetsIL1.Visible = true;
            spinBlockSizeIL1.Visible = true;
            spinAssociativityIL1.Visible = true;
            comboReplacementIL1.Visible = true;
            spinLatIL1.Visible = true;
        }
    }

    protected void OnChkIL2(object sender, EventArgs e)
    {
        IL2();
    }

    protected void IL2() 
    {
        UnifiedIL2();
        if (chkIL2.Active == true)
        {
            lbNumSetsIL2.Visible = true;
            lbBlockSizeIL2.Visible = true;
            lbAssociativityIL2.Visible = true;
            lbReplacementIL2.Visible = true;
            lbLatIL2.Visible = true;

            spinNumSetsIL2.Visible = true;
            spinBlockSizeIL2.Visible = true;
            spinAssociativityIL2.Visible = true;
            comboReplacementIL2.Visible = true;
            spinLatIL2.Visible = true;

            chkUnifiedIL2.Visible = true;
        }
        else
        {
            lbNumSetsIL2.Visible = false;
            lbBlockSizeIL2.Visible = false;
            lbAssociativityIL2.Visible = false;
            lbReplacementIL2.Visible = false;
            lbLatIL2.Visible = false;

            spinNumSetsIL2.Visible = false;
            spinBlockSizeIL2.Visible = false;
            spinAssociativityIL2.Visible = false;
            comboReplacementIL2.Visible = false;
            spinLatIL2.Visible = false;

            chkUnifiedIL2.Visible = false;
        }
    }

    protected void OnUnifiedIL2(object sender, EventArgs e)
    {
        UnifiedIL2();
    }

    protected void UnifiedIL2()
    {
        if(chkUnifiedIL2.Active == true) 
        {
            spinNumSetsIL2.Visible = false;
            spinBlockSizeIL2.Visible = false;
            spinAssociativityIL2.Visible = false;
            comboReplacementIL2.Visible = false;
            spinLatIL2.Visible = false;
        }
        else 
        {
            spinNumSetsIL2.Visible = true;
            spinBlockSizeIL2.Visible = true;
            spinAssociativityIL2.Visible = true;
            comboReplacementIL2.Visible = true;
            spinLatIL2.Visible = true;
        }
    }

    protected void OnChkDTLB(object sender, EventArgs e)
    {
        DTLB();
    }

    protected void DTLB() 
    {
        LatTLB();
        if (chkDTLB.Active == true)
        {
            lbNumSetsDTLB.Visible = true;
            lbBlockSizeDTLB.Visible = true;
            lbAssociativityDTLB.Visible = true;
            lbReplacementDTLB.Visible = true;

            spinNumSetsDTLB.Visible = true;
            spinBlockSizeDTLB.Visible = true;
            spinAssociativityDTLB.Visible = true;
            comboReplacementDTLB.Visible = true;
        }
        else
        {
            lbNumSetsDTLB.Visible = false;
            lbBlockSizeDTLB.Visible = false;
            lbAssociativityDTLB.Visible = false;
            lbReplacementDTLB.Visible = false;

            spinNumSetsDTLB.Visible = false;
            spinBlockSizeDTLB.Visible = false;
            spinAssociativityDTLB.Visible = false;
            comboReplacementDTLB.Visible = false;
        }
    }

    protected void OnChkITLB(object sender, EventArgs e)
    {
        ITLB();
    }

    protected void ITLB() 
    {
        LatTLB();
        if (chkITLB.Active == true)
        {
            lbNumSetsITLB.Visible = true;
            lbBlockSizeITLB.Visible = true;
            lbAssociativityITLB.Visible = true;
            lbReplacementITLB.Visible = true;

            spinNumSetsITLB.Visible = true;
            spinBlockSizeITLB.Visible = true;
            spinAssociativityITLB.Visible = true;
            comboReplacementITLB.Visible = true;
        }
        else
        {
            lbNumSetsITLB.Visible = false;
            lbBlockSizeITLB.Visible = false;
            lbAssociativityITLB.Visible = false;
            lbReplacementITLB.Visible = false;

            spinNumSetsITLB.Visible = false;
            spinBlockSizeITLB.Visible = false;
            spinAssociativityITLB.Visible = false;
            comboReplacementITLB.Visible = false;
        }
    }

    protected void LatTLB()
    { 
        if(chkDTLB.Active || chkITLB.Active) 
        {
            lbLatTLB.Visible = true;
            spinLatTLB.Visible = true;
        }
        else 
        {
            lbLatTLB.Visible = false;
            spinLatTLB.Visible = false;
        }
    }

    protected void OnChkCacheFlush(object sender, EventArgs e)
    {
        CacheFlush();
    }

    protected void CacheFlush() 
    {
        if(chkCacheFlush.Active == true) 
        {
            comboCacheFlush.Visible = true;
        }
        else 
        {
            comboCacheFlush.Visible = false;
        }
    }

    protected void OnChkCacheIcompress(object sender, EventArgs e)
    {
        CacheIcompress();
    }

    protected void CacheIcompress() 
    {
        if (chkCacheIcompress.Active == true)
        {
            comboCacheIcompress.Visible = true;
        }
        else 
        {
            comboCacheIcompress.Visible = false;
        }
    }

    protected void OnMemLat(object sender, EventArgs e)
    {
        MemLat();
    }

    protected void MemLat() 
    {
        if(chkMemLat.Active == true) 
        {
            lbMemLatFirstChunk.Visible = true;
            lbMemLatInterChunk.Visible = true;

            spinMemLatFirstChunk.Visible = true;
            spinMemLatInterChunk.Visible = true; 
        }
        else 
        {
            lbMemLatFirstChunk.Visible = false;
            lbMemLatInterChunk.Visible = false;

            spinMemLatFirstChunk.Visible = false;
            spinMemLatInterChunk.Visible = false;
        }
    }

    protected void OnMemWidth(object sender, EventArgs e)
    {
        MemWidth();
    }

    protected void MemWidth()
    { 
        if(chkMemWidth.Active == true) 
        {
            spinMemWidth.Visible = true;
        }
        else 
        {
            spinMemWidth.Visible = false;
        }
    }
}
