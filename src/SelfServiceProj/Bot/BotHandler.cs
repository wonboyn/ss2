using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.RegularExpressions;


namespace SelfServiceProj
{
    public class BotHandler : ActivityHandler
    {

        protected static string _cardFolder = "Resources";
        protected readonly SelfServiceConfig _config;
        protected readonly ILogger _logger;
        protected readonly string _rootPath;


        public BotHandler(
            IConfiguration configuration,
            ILogger<IBotFrameworkHttpAdapter> logger)
        {

            // Bind the configuration to our config model
            _config = new SelfServiceConfig();
            configuration.Bind(_config);

            // Setup the logger
            _logger = logger;

            // Derive the root path
            _rootPath = configuration.GetValue<string>("AzureWebJobsScriptRoot");
        }


        private async Task<Attachment> DoHelp(String action)
        {

            // Connect to DB
            var db = new Database(_config);

            // Get list of actions
            var actionDetails = await db.GetAction(action);

            // Create an action help card
            var json = LoadJson("Help.json");
            var attachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(json),
            };
            return attachment;
        }


        private async Task<Attachment> DoList()
        {

            // Connect to DB
            var db = new Database(_config);

            // Get list of actions
            var actions = await db.GetAll();
            var actionsJson = JsonConvert.SerializeObject(actions);

            // Create the help/list card
            var json = LoadJson("List.json");
            var attachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(json),
            };
            return attachment;
        }


        private Attachment DoWelcome()
        {

            var json = LoadJson("Welcome.json");
            var attachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(json),
            };
            return attachment;
        }


        private string LoadJson(string file)
        {

            // Derive the path to the Card JSON file
            var contentPath = Path.GetFullPath(Path.Combine(_rootPath, _cardFolder, file));
            var cardJson = File.ReadAllText(contentPath);
            return cardJson;
        }


        protected override async Task OnMembersAddedAsync(
            IList<ChannelAccount> membersAdded,
            ITurnContext<IConversationUpdateActivity> turnContext,
            CancellationToken cancellationToken)
        {

            // Log start
            _logger.LogDebug("Started OnMembersAddedAsync() processing...");

            // Create a welcome card
            var attachment = DoWelcome();

            // Send the welcome card to new members
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(
                        MessageFactory.Attachment(attachment),
                        cancellationToken);
                }
            }

            // Log end
            _logger.LogDebug("Finished OnMembersAddedAsync() processing...");
        }


        protected override async Task OnMessageActivityAsync(
            ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {

            // Log start
            _logger.LogDebug("Started OnMessageActivityAsync() processing...");

            // Get user input
            Attachment? attachment = null;
            var input = turnContext.Activity.Text.ToLower();

            // Log details
            _logger.LogDebug($"Received input: {input}");

            //  What do we need to do?
            if (String.IsNullOrEmpty(input))
            {
                // No input so send help
                attachment = await DoList();
            }
            else
            {
                // What was requested?
                switch (input)
                {
                    case "help":
                    case "list":

                        // Log details
                        _logger.LogDebug("Matched help/list...");

                        // Send list of actions
                        attachment = await DoList();
                        break;

                    case string s when Regex.IsMatch(s, @"^help\s+[0-9a-zA-Z]+$"):

                        // Log details
                        _logger.LogDebug("Matched regex...");

                        // Specific action help
                        var cmd = input.Split(" ");
                        if (cmd.Length != 2)
                        {
                            attachment = await DoList();
                        }
                        else
                        {
                            attachment = await DoHelp(cmd[1]);
                        }
                        break;

                    default:

                        // Log details
                        _logger.LogDebug("No match...");

                        // Send list of actions
                        attachment = await DoList();
                        break;
                }
            }

            // Send the response
            await turnContext.SendActivityAsync(
                MessageFactory.Attachment(attachment),
                cancellationToken);

            // Log end
            _logger.LogDebug("Finished OnMessageActivityAsync() processing...");
        }
    }
}
