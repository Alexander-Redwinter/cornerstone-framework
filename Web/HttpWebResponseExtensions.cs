using System.IO;
using System.Net;

namespace Cornerstone
{
    public static class HttpWebResponseExtensions
    {
        public static WebRequestResult<TResponse> CreateWebRequestResult<TResponse>(this HttpWebResponse response)
        {
            var result = new WebRequestResult<TResponse>
            {
                ContentType = response.ContentType,
                Headers = response.Headers,
                Cookies = response.Cookies,
                StatusCode = response.StatusCode,
                StatusDescription = response.StatusDescription,
            };

            if (result.StatusCode == HttpStatusCode.OK)
            {
                using (var responseStream = response.GetResponseStream())
                using (var streamReader = new StreamReader(responseStream))
                    result.RawServerResponse = streamReader.ReadToEnd();
            }

            return result;
        }

    }
}
