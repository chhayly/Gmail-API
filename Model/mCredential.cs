using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmail_Api.Model
{
    public class mCredential
    {
        public string client_id;
        public string project_id;
        public string auth_uri;
        public string token_uri;
        public string auth_provider_x509_cert_url;
        public string client_secret;
        public List<string> redirect_uris;
    }


    public struct webStruct
    {
        public mCredential web;
    }
}
