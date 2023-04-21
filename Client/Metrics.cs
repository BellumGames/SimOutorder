using System;
using Newtonsoft.Json;
namespace Client
{
    public class Metrics
    {

        [JsonProperty("fetchRate")]
        private string fetchRate;
        [JsonProperty("instructionRegister")]
        private string instructionRegister;

        public Metrics(string fetchRate, string instructionRegister)
        {
            this.fetchRate = fetchRate;
            this.instructionRegister = instructionRegister;
        }

        private string parseString()
        {
            return null;
        }

        private string extractMatrics()
        {
            return null;
        }
    }
}
