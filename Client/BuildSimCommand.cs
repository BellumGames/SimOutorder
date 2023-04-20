using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace Client
{
    [Serializable]
    public class BuildSimCommand
    {
        private static readonly string SIMULTOR = "sim-outorder";
        private static readonly string REDIR = "Res";

        private string commnad;
        private string max_int;
        private string fast_fwd;
        private string ptrace;

        private string fetch_ifqsize;
        private string fetch_mplat;
        private string fetch_speed;

        private string bpred_nottaken;
        private string bpred_taken;
        private string bpred_perfect;
        private string bpred_2lev;
        private string bpred_comb;
        private string bpred_bimod;
        private string bpred_ras;
        private string bpred_spec_update;
        private string bpred_btb;

        private string decode_with;

        private string issue_width;
        private string issue_inorder;
        private string issue_wrongpath;

        private string commit_width;

        private string ruu_width;

        private string lsq_width;

        private string cache_dl1;
        private string cache_dl1lat;
        private string cache_dl2;
        private string cache_dl2lat;
        private string cache_il1;
        private string cache_il1lat;
        private string cache_il2;
        private string cache_il2lat;
        private string cache_fluh;
        private string cache_icompress;

        private string mem_lat;
        private string mem_width;

        private string tib_itlb;
        private string tib_dtlb;
        private string tib_lat;

        private string res_ialu;
        private string res_imult;
        private string res_memport;
        private string res_fpalu;
        private string res_fpmult;

        private string pcstat;

        private string bugcompat;

        [JsonProperty("name")]
        private string name;
        [JsonProperty("value")]
        private string value;

        public void GetPropertis()
        {
            string json = "/home/timarc/Desktop/MareProiect/SimOutorder/Client/CommandSettings.json";

            try
            {
                string json_from_file;
                using (var reader = new StreamReader(json))
                {
                    json_from_file = reader.ReadToEnd();
                }

                BuildSimCommand simCommand = JsonConvert.DeserializeObject<BuildSimCommand>(json_from_file);
                Console.WriteLine(simCommand.name + " " + simCommand.value);
            }catch(IOException ex)
            {
                Console.WriteLine(ex);
            }

            commnad = $"{SIMULTOR} -redir:sim {REDIR} -max:inst {max_int}"+
                $"-cache:dl1 {cache_dl1}:{}"


        }
    }
}
