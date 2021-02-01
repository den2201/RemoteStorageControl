using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDiskControl.Interfaces
{
    interface IRemoteDiskControl
    {
        void ConnectToDisk();

        string GetDiskInfo();

        string PutFileOnDiskAsync(string filesPath, string diskPath);
    }
}
