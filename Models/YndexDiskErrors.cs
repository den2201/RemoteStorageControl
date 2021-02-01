using RemoteDiskControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDiskControl.Models
{

  public  class YandexDiskErrors : IRemoteDiskErrors
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Info { get; set; }
    }
}
