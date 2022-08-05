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
            )] HttpRequestData req)
        {

            // Log start of processing
            _logger.LogInformation("Started Self Service Bot processing with framework...");

            // var response = req.CreateResponse(HttpStatusCode.OK);
            // response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            // response.WriteString("Welcome to Self Service Bot Function!");

            // Mocked bot message
            var str_activity = @"
            {
                ""type"": ""message"",
                ""id"": ""1485983408511"",
                ""timestamp"": ""2017-02-01T21:10:07.437Z"",
                ""localTimestamp"": ""2017-02-01T14:10:07.437-07:00"",
                ""serviceUrl"": ""https://smba.trafficmanager.net/amer/"",
                ""channelId"": ""msteams"",
                ""from"": {
                    ""id"": ""29:1XJKJMvc5GBtc2JwZq0oj8tHZmzrQgFmB39ATiQWA85gQtHieVkKilBZ9XHoq9j7Zaqt7CZ-NJWi7me2kHTL3Bw"",
                    ""name"": ""Megan Bowen"",
                    ""aadObjectId"": ""7faf8ab2-3d56-4244-b585-20c8a42ed2b8""
                },
                ""conversation"": {
                    ""conversationType"": ""personal"",
                    ""id"": ""a:17I0kl9EkpE1O9PH5TWrzrLNwnWWcfrU7QZjKR0WSfOpzbfcAg2IaydGElSo10tVr4C7Fc6GtieTJX663WuJCc1uA83n4CSrHSgGBj5XNYLcVlJAs2ZX8DbYBPck201w-""
                },
                ""recipient"": {
                    ""id"": ""28:c9e8c047-2a74-40a2-b28a-b162d5f5327c"",
                    ""name"": ""Teams TestBot""
                },
                ""textFormat"": ""plain"",
                ""text"": ""Hello Teams TestBot"",
                ""entities"": [
                  { 
                    ""locale"": ""en-US"",
                    ""country"": ""US"",
                    ""platform"": ""Windows"",
                    ""timezone"": ""America/Los_Angeles"",
                    ""type"": ""clientInfo""
                  }
                ],
                ""channelData"": {
                    ""tenant"": {
                        ""id"": ""72f988bf-86f1-41af-91ab-2d7cd011db47""
                    }
                },
                ""locale"": ""en-US""
            }";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(str_activity));

            // Mock up a response
            var resp = req.CreateResponse();

            // Mock up a request
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = "POST";
            request.Scheme = "https";
            request.ContentType = "application/json";
            request.Body = stream;
            request.ContentLength = stream.Length;


            // Mock up a response
            var response = httpContext.Response;


            // Send to the bot adapter for processing
            //await _adapter.ProcessAsync(req, resp, _bot);
            await _adapter.ProcessAsync(request, response, _bot);

            // Log completion of processing
            _logger.LogInformation("Finished Self Service Bot processing with framework...");

            return resp;
        }
    }
}
