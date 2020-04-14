using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessRESTAPI_JIRA
{
    class Project
    {
        public string expand { get; set; }
        public string self { get; set; }
        public double id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public urls avatarUrls { get; set; }
        public string projectTypeKey { get; set; }


        public class urls
        {
            public string x48x48 { get; set; }
            public string x24by24 { get; set; }
            public string x16by16 { get; set; }
            public string x32by32 { get; set; }
        }

    }
}
