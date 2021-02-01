using RemoteDiskControl.Controllers;
using RemoteDiskControl.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDiskControl
{
    class Program 
    {
        const string filePath = "c:/temp/";

        static  void Main(string[] args)
        {
            Console.WriteLine("Files for sending: ");
            SendFiles(filePath, "");

            Console.WriteLine("Hello");
            
            Console.ReadKey();
        }

        public  static async void  SendFiles(string directoryPath, string storagePath)
        {
            
            List<string> fileList = Directory.GetFiles(directoryPath).ToList<string>();
            foreach (var t in fileList)
            {
                Console.WriteLine(t);
            }

            IRemoteDiskControl yandexControl = new YandexDiskController();
            foreach (var file in fileList)
            {
               var status =  await yandexControl.PutFileOnDiskAsync(file, @"test\");
                Console.WriteLine($"{file}: status: {status}");
            }

        }
    }
}
