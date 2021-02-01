using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDiskControl.Models
{
    public class RefToUploadResponse
    {
        public string operation_id { get; set; }

        public string href { get; set; }

        public string method { get; set; }

        public bool templated { get; set; }

    }
}
