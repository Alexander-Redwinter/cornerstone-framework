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

        public static async Task<WebRequestResult<TResponse>> PostAsync<TResponse>(string url, object content = null
            , ContentSerializers sendType = ContentSerializers.Json
            , ContentSerializers returnType = ContentSerializers.Json)
        {
            var response = await PostAsync(url, content, sendType, returnType);

            var result = response.CreateWebRequestResult<TResponse>();

            if (result.StatusCode != HttpStatusCode.OK)
            {
                //TODO LOCALIZE
                result.ErrorMessage = $"Server returned {response.StatusCode} {response.StatusDescription}";
                return result;
            }

            if (string.IsNullOrEmpty(result.RawServerResponse))
                return result;

            try
            {

                if (!response.ContentType.ToLower().Contains(returnType.ToMimeString().ToLower()))
                {
                    result.ErrorMessage = $"Unexpected type. Expected {returnType.ToMimeString()}, got {response.ContentType}";
                    return result;
                }


                if (returnType == ContentSerializers.Json)
                {
                    result.ServerResponse = JsonConvert.DeserializeObject<TResponse>(result.RawServerResponse);

                }

                else if (returnType == ContentSerializers.Xml)
                {
                    var xmlSerializer = new XmlSerializer(typeof(TResponse));

                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(result.RawServerResponse)))
                    {
                        result.ServerResponse = (TResponse)xmlSerializer.Deserialize(ms);
                    }
                }

                else
                {
                    result.ErrorMessage = "Unknown return type";
                    return result;
                }
            }
            catch
            {
                result.ErrorMessage = "Failed to deserialize";
                return result;
            }
            
            return result;

        }
    }
}
