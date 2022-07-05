using Newtonsoft.Json;
using System.Collections.Generic;

namespace SolCARDIP.App_Code
{
    public class ReCaptchaClass
    {
        public static string Validate(string EncodedResponse, string remoteIP)
        {
            var client = new System.Net.WebClient();
            string PrivateKey = "6LdwGy0UAAAAAPPbZadO_-K68d9NF2zotMbUqnIm";
            //string PrivateKeyInv = "6LcGkC0UAAAAAAaUSjP0WaxPv_IQY2yklf4IT-ee";
            var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}", PrivateKey, EncodedResponse, remoteIP));

            var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ReCaptchaClass>(GoogleReply);

            return captchaResponse.Success;
        }

        [JsonProperty("success")]
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }

        private string m_Success;
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }


        private List<string> m_ErrorCodes;
    }
}