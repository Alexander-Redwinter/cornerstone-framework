using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cornerstone
{
    public static class WebRequests
    {
        public static async Task<HttpWebResponse> PostAsync(string url, object content = null
            , ContentSerializers sendType = ContentSerializers.Json
            , ContentSerializers returnType = ContentSerializers.Json)
        {
            var request = WebRequest.CreateHttp(url);

            request.Method = HttpMethod.Post.ToString();

            request.Accept = returnType.ToMimeString();

            request.ContentType = sendType.ToMimeString();

            if (content == null)
                request.ContentLength = 0;
            else
            {
                var contentString = string.Empty;

                if (sendType == ContentSerializers.Json)
                    contentString = JsonConvert.SerializeObject(content);

                else if (sendType == ContentSerializers.Xml)
                {
                    var xmlSerializer = new XmlSerializer(content.GetType());

                    using (var sw = new StringWriter())
                    {
                        xmlSerializer.Serialize(sw, content);

                        contentString = sw.ToString();
                    }
                }

                else
                {
                    throw new Exception();
                }

                using (var requestStream = await request.GetRequestStreamAsync())
                using (var streamWriter = new StreamWriter(requestStream))
                    await streamWriter.WriteAsync(contentString);

            }

            return await request.GetResponseAsync() as HttpWebResponse;

        }
    }
}
