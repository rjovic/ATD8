using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace FutureDev.Hello.MessageHandlers
{
    public class UriFormatExtensionMessageHandler : DelegatingHandler
    {
        private static IDictionary<string, string> extMappings = new Dictionary<string, string>
            {
                {"xml", "application/xml"},
                {"json", "application/json"},
                {"csv", "text/csv"}
            };

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = request.RequestUri.AbsolutePath;
            var ext = path.Substring(path.LastIndexOf('.') + 1);

            string mediaType = null;
            var found = extMappings.TryGetValue(ext, out mediaType);

            if(found)
            {
                var newUri = request.RequestUri.OriginalString.Replace('.' + ext, String.Empty);
                request.RequestUri = new Uri(newUri, UriKind.Absolute);
                request.GetRouteData().Values.Remove("ext");
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}