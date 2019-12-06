using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Requests;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework.Result;
using SIS.MvcFramework.Routing.Contracts;
using SIS.MvcFramework.Sessions;
using SIS.Common;

namespace SIS.MvcFramework
{
    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly IServerRoutingTable serverRoutingTable;

        private readonly IHttpSessionStorage sessionStorage;

        public ConnectionHandler(Socket client, IServerRoutingTable serverRoutingTable, IHttpSessionStorage sessionStorage)
        {
            client.ThrowIfNull(nameof(client));
            serverRoutingTable.ThrowIfNull(nameof(serverRoutingTable));
            sessionStorage.ThrowIfNull(nameof(sessionStorage));

            this.client = client;
            this.serverRoutingTable = serverRoutingTable;
            this.sessionStorage = sessionStorage;
        }

        private async Task<IHttpRequest> ReadRequestAsync()
        {
            // PARSE REQUEST FROM BYTE DATA
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesToRead = await this.client.ReceiveAsync(data, SocketFlags.None);

                if (numberOfBytesToRead == 0)
                {
                    break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesToRead);
                result.Append(bytesAsString);

                if (numberOfBytesToRead < 1023)
                {
                    break;
                }
            }

            if (result.Length == 0)
            {
                return null;
            }

            return new HttpRequest(result.ToString());
        }

        private IHttpResponse ReturnIfResource(IHttpRequest httpRequest)
        {
            string folderPrefix = "/../../../../";
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string resourceFolderPath = "Resources/";
            string requestedResource = httpRequest.Path;

            string fullPathResource = assemblyLocation + folderPrefix + resourceFolderPath + requestedResource;

            if (File.Exists(fullPathResource))
            {
                return new InlineResourceResult(File.ReadAllBytes(fullPathResource), HttpResponseStatusCode.Ok);
            }

            return new TextResult($"Route with method {httpRequest.RequestMethod} and path \"{httpRequest.Path}\" not found.", HttpResponseStatusCode.NotFound);
        }

        private IHttpResponse HandleRequest(IHttpRequest httpRequest)
        {
            // EXECUTE FUNCTION FOR CURRENT REQUEST -> RETURNS RESPONSE
            if (!this.serverRoutingTable.Contains(httpRequest.RequestMethod, httpRequest.Path))
            {
                return this.ReturnIfResource(httpRequest);
            }

            return this.serverRoutingTable.Get(httpRequest.RequestMethod, httpRequest.Path).Invoke(httpRequest);
        }

        private string SetRequestSession(IHttpRequest httpRequest)
        {
            string sessionId;

            if (httpRequest.Cookies.ContainsCookie(HttpSessionStorage.SessionCookieKey))
            {
                var cookie = httpRequest.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
            }

            httpRequest.Session = this.sessionStorage.GetSession(sessionId);
            return httpRequest.Session.Id;
        }

        private void SetResponseSession(IHttpResponse httpResponse, string sessionId)
        {
            if (sessionId != null)
            {
                httpResponse.Cookies
                    .AddCookie(new HttpCookie(HttpSessionStorage
                        .SessionCookieKey, sessionId));
            }
        }

        private void PrepareResponse(IHttpResponse httpResponse)
        {
            // PREPARES RESPONSE -> MAPS IT TO BYTE DATA
            byte[] byteSegments = httpResponse.GetBytes();

            this.client.Send(byteSegments, SocketFlags.None);
        }

        public async Task ProcessRequestAsync()
        {
            IHttpResponse httpResponse = null;
            try
            {
                IHttpRequest httpRequest = await this.ReadRequestAsync();

                if (httpRequest != null)
                {
                    Console.WriteLine($"Processing: {httpRequest.RequestMethod} {httpRequest.Path}...");

                    string sessionId = this.SetRequestSession(httpRequest);

                    httpResponse = this.HandleRequest(httpRequest);

                    this.SetResponseSession(httpResponse, sessionId);
                }
            }
            catch (BadRequestException e)
            {
                httpResponse = new TextResult(e.Message, HttpResponseStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                httpResponse = new TextResult(e.Message, HttpResponseStatusCode.InternalServerError);
            }
            this.PrepareResponse(httpResponse);

            this.client.Shutdown(SocketShutdown.Both);
        }
    }
}
