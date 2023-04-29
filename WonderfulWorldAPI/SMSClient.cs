namespace WonderfulWorldAPI;
using System.Net;
class SMSClient
    {
        public string Session { get; set; }

        public void SendPOST(string msg)
        {
            SendPOST("https://xiaoyuan.aoyadianzi.cn:7443/v1/parent/miniapp/command/sendMsg",msg);
        }

        public void SendPOST(string url, string msg)
        {
            var url1 = url;
            var request = (HttpWebRequest)WebRequest.Create(url1);
            request.Method = "POST";
            request.Headers.Add("Host", "xiaoyuan.aoyadianzi.cn:7443");
            request.Headers.Add("sessionId", Session);
            request.Headers.Add("from", "STU_PARENT");
            request.Headers.Add("user-agent", "Chrome 114514");
            request.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(msg);
            }
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                }
            }
        }

        public void SendMessage(string msg)
        {
            SendPOST("{\"commandType\":14,\"commandMsg\":{\"ledFlag\":1,\"vibration\":0,\"sound\":0,\"smsType\":5,\"context\":\"" + msg + "\",\"displayNum\":1,\"displayType\":1},\"imei\":\"862677060127893\",\"cardId\":134182}");
        }

        public void CleanMessage()
        {
            SendPOST("{\"cardId\":134182,\"imei\":\"862677060127893\",\"commandType\":9,\"commandMsg\":\"{\\\"restart\\\":0,\\\"restore\\\":1}\"}");
            SetPhoneNum();
        }

        public void SetPhoneNum()
        {
            SendPOST("https://xiaoyuan.aoyadianzi.cn/v1/parent/miniapp/command/sendFamilyPhoneMsg","{\"phone1\":\"18795702792\",\"phone2\":\"15555519609\",\"phone3\":\"\",\"familyName1\":\"\",\"familyName2\":\"\",\"familyName3\":\"\",\"sosPhone\":\"13814670163\",\"imei\":\"862677060127893\",\"commandType\":1,\"commandMsg\":\"\"}");
        }
    }

           





