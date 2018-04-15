using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class ReCaptchaResponseModel
    {
        public ReCaptchaResponseModel()
        {
        }

        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTs { get; set; }
        [JsonProperty("hostname")]
        public string Hostname { get; set; }
        [JsonProperty("error-codes")]
        public string[] ErrorCodes { get; set; }
    }
}