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


        private async Task<Attachment> DoBasicHelp()
        {

            // Connect to DB
            var db = new Database(_config);

            // Get list of actions
            var actions = await db.GetAll();

            // Create an action list card
            var card = new SelfServiceProj.ActionListCard();
            var attachment = card.GenerateAttachment();

            return attachment;
        }


        private async Task<Attachment> DoActionHelp(String action)
        {

            // Connect to DB
            var db = new Database(_config);

            // Get list of actions
            var actionDetails = await db.GetAction(action);

            // Create an action help card
            var card = new SelfServiceProj.ActionHelpCard();
            var attachment = card.GenerateAttachment();

            return attachment;
        }


        protected override async Task OnMembersAddedAsync(
            IList<ChannelAccount> membersAdded,
            ITurnContext<IConversationUpdateActivity> turnContext,
            CancellationToken cancellationToken)
        {

            // Log start
            _logger.LogDebug("Started OnMembersAddedAsync() processing...");

            // Create a welcome card
            // var card = new SelfServiceProj.WelcomeCard();
            // var attachment = card.GenerateAttachment();
            var attachment = LoadSchema("Welcome.json");

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

            //  What do we need to do?
            if (String.IsNullOrEmpty(input))
            {
                // No input so send help
                attachment = await DoBasicHelp();
            }
            else
            {
                // What was requested?
                switch (input)
                {
                    case "help":
                    case "list":

                        // Basic help (ie list actions)
                        attachment = await DoBasicHelp();
                        break;

                    case string s when Regex.IsMatch(s, @"^help\s+[0-9a-zA-Z]+$"):

                        // Specific action help
                        var cmd = input.Split(" ");
                        if (cmd.Length != 2)
                        {
                            attachment = await DoBasicHelp();
                        }
                        else
                        {
                            attachment = await DoActionHelp(cmd[1]);
                        }
                        break;

                    default:
                        attachment = await DoBasicHelp();
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


        private Attachment LoadSchema(string file)
        {

            // Derive the path to the Card JSON file
            var contentPath = Path.GetFullPath(Path.Combine(_rootPath, "Resources", file));
            var adaptiveCardJson = File.ReadAllText(contentPath);
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        }
    }
}
