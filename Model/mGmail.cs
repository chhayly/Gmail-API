using Gmail_Api.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmail_Api.Model
{
    public class mGmail: IGmail
    {
        private string UserId;

        public mGmail(string UserId) 
        {
            this.UserId = UserId;
        }

        public string GetUserId() => UserId;
        public mToken token
        {
            get
            {
                using (var stream = new FileStream($"tokens\\token_{UserId}.json", FileMode.Open, FileAccess.Read))
                {
                    using var sr = new StreamReader(stream, Encoding.UTF8);

                    string json = sr.ReadToEnd();

                    return JsonConvert.DeserializeObject<mToken>(json);
                }
            }
        }
        public mCredential credential
        {
            get
            {

                using (var stream = new FileStream($"credential\\credential.json", FileMode.Open, FileAccess.Read))
                {
                    using var sr = new StreamReader(stream, Encoding.UTF8);

                    string json = sr.ReadToEnd();

                    webStruct _web = JsonConvert.DeserializeObject<webStruct>(json);

                    return _web.web;
                }
            }
        }
    }
}
