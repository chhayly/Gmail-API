using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmail_Api.Model
{

    public class mMessage
    {
        public string from { set; get; }
        public List<string> to { set; get; }
        public List<string> cc { set; get; }
        public string subject { set; get; }
        public string plainTextMessage { set; get; }
        public string htmlMessage { set; get; }
    }
}
