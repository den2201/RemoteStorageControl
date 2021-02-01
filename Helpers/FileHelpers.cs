using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDiskControl.Helpers
{
   public static class FileHelpers
    {
        public static byte [] ConvertToByte(string filename)
        {
            return System.IO.File.ReadAllBytes(filename);
        }

        //get fileName from full path

        public static string GetFilNameFromPathString(string path)
        {
            string fileName = path.Split('/').Last();
            return fileName;
        }
    }
}
