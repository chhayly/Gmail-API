using Gmail_Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmail_Api.Interface
{
    interface IGmail
    {
        public mToken token { get; }
        public mCredential credential { get; }
    }

    interface IUserId 
    {
        public string UserId { get; }
    }
}
