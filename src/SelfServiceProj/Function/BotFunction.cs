using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Logging;

namespace SelfServiceProj
{
    public class BotFunction
    {

        // Setup member variables
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly IBot _bot;
        private readonly ILogger _logger;

        // Initialise
        public BotFunction(
            ILoggerFactory loggerFactory,
            IBotFrameworkHttpAdapter adapter,
            IBot bot)
        {
            _adapter = adapter;
            _bot = bot;
            _logger = loggerFactory.CreateLogger<BotFunction>();
        }


        // Azure Function definition
        [Function("BotFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "get", "post",
                Route = "messages"
            )] HttpRequestData request)
        {

            // Log start of processing
            _logger.LogDebug("Started Bot processing...");

            // Build the request/response objects
            var (botreq, botresp) = GenBotObjects(request);

            // Send to the bot adapter for processing
            await _adapter.ProcessAsync(botreq, botresp, _bot);

            // Log completion of processing
            _logger.LogDebug("Finished Bot processing...");

            // Mock up a response
            var response = request.CreateResponse(HttpStatusCode.OK);

            // Return the response
            return response;
        }

        private (HttpRequest, HttpResponse) GenBotObjects(HttpRequestData inbound)
        {
            // Create an empty request
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            var response = httpContext.Response;

            // Set the method
            request.Method = inbound.Method;

            // Set the headers
            foreach (var item in inbound.Headers)
            {
                var hdr_key = item.Key;
                var hdr_val = item.Value.ToString();
                request.Headers[hdr_key] = hdr_val;
            }

            // Set the content type
            request.ContentType = "application/json";

            // Set the body
            var payload = inbound.ReadAsString();
            if (payload != null)
            {
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(payload));
                request.Body = stream;
                request.ContentLength = stream.Length;
            }

            // Return it
            return (request, response);
        }
    }
}
