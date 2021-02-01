using RemoteDiskControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Rest;
using RemoteDiskControl.Models;
using System.Text.Json;
using RemoteDiskControl.Helpers;

namespace RemoteDiskControl.Controllers
{
    public enum httpMethod
    {
        PUT,
        DELETE,
        GET,
        POST
    }

    class YandexDiskController: IRemoteDiskControl
    {
        private string id;
        private string password;
        private string Token { get; set; }

        public YandexDiskController()
        {
            Token = "AgAAAABP_YRRAADLW61MZ7ecHkvjmhyCKiWucUg";
        }

        public YandexDiskController(string id, string password):this()
        {
            this.id = id;
            this.password = password;
        }

        /// <summary>
        /// получение токена. 
        /// </summary>
        public void ConnectToDisk()
        {
            
        }

        /// <summary>
        /// Получаем общую информацию о пользователе. Нужен был для тестирования
        /// </summary>
        /// <returns></returns>
        public string GetDiskInfo()
        {
            var request = CreateRequest(httpMethod.GET,"");
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    string remoteDiskErrors = GenerateError(response);
                    return remoteDiskErrors;
                }

                else
                {
                    using (var st = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(st))
                        {
                            string text = reader.ReadToEnd();
                            return text;
                        }
                    }
                }
            }
        }

        public async Task<string> PutFileOnDiskAsync(string directoryPath, string diskPath)
        {
            var x = await Task.Run(() => PutFiles(directoryPath, diskPath));
            return x;
        }

        //первый реквест на сервер. Получаем ссылку для загрузки файла
        //по умолчанию файлы будут ложиться в корневой каталог, если указан относительный путь с 
        //существующими папками, то файл поломеститься поданному пути

        private string GetUrlForUploadFile(string localFfilePath, string putToPath)
        {
           var urlPath = FileHelpers.GetFilNameFromPathString(localFfilePath);

            //if (!string.IsNullOrEmpty(putToPath))
            //    urlPath = putToPath + urlPath;

            string url =$"https://cloud-api.yandex.net/v1/disk/resources/upload?path="+ urlPath;
            var request = CreateRequest(httpMethod.GET, url);

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    string remoteDiskErrors = GenerateError(response);
                    throw new Exception($"Error getting reference for uploading file {localFfilePath}" + remoteDiskErrors);
                }
                else
                {
                    using (var st = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(st))
                        {
                            string text = reader.ReadToEnd();
                            return text;
                        }
                    }
                }
            }
        }
        
        private string PutFiles(string filesPath, string storagePath)
        {
            string responseBodyText = string.Empty; ;
            try
            {
                //geting reference for uploading file
                var responseModel = JsonSerializer.Deserialize<RefToUploadResponse>(GetUrlForUploadFile(filesPath, storagePath));

                var putFilesRequest = CreateRequest(httpMethod.PUT, responseModel.href);

                FileStream file = new FileStream(filesPath, FileMode.Open, FileAccess.ReadWrite);
                byte[] rData = new byte[file.Length];
                file.Read(rData, 0, rData.Length);
                putFilesRequest.ContentLength = rData.Length;

                using (Stream st = putFilesRequest.GetRequestStream())
                {
                    st.Write(rData, 0, rData.Length);
                }

                using(var response = putFilesRequest.GetResponse())
                {
                    return response.Headers["StatusCode"];
                }
               
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return responseBodyText;
        }

        /// <summary>
        /// получаем текст с ошибкой
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>

        //todo:
        //need to refact. two methods 
        private string GenerateError(HttpWebResponse response)
        {
            using (Stream st = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(st))
                {
                    string errorJsonText = reader.ReadToEnd();
                    return errorJsonText;
                }
            }
        }

    /// <summary>
    /// извлекаем из респонса тело json и приводим к DTO модели. ToDo
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>

        private RefToUploadResponse GetReponse(HttpWebResponse response)
        {
            using (Stream st = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(st))
                {
                    string JsonText = reader.ReadToEnd();

                    var responseWithReference = JsonSerializer.Deserialize<RefToUploadResponse>(JsonText);

                    return responseWithReference;
                }
            }

        }


        /// <summary>
        /// создаем новый реквест и добавляем токен, хедеры и метод в реквест 
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="URL"></param>
        /// <returns></returns>
        private HttpWebRequest CreateRequest(httpMethod method, string URL)
        {
            var request = WebRequest.Create(URL) as HttpWebRequest;
            request.Headers.Add("Authorization: OAuth " + Token);
            request.Accept = "*/*";
            if (method == httpMethod.PUT)
                request.ContentType = "text/plain";
            request.Method = method.ToString();
            return request;
        }

       
    }
}
