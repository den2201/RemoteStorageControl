using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDiskControl.Interfaces
{
    interface IRemoteDiskErrors
    {
        string ErrorCode { get; set; }
        string ErrorMessage { get; set; } 
        string Info { get; set; }
    }
}
