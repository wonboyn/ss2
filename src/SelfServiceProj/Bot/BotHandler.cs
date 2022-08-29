using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;


namespace SelfServiceProj
{
    public class BotHandler : ActivityHandler
    {

        protected readonly SelfServiceConfig _config;
        protected readonly ILogger _logger;


        public BotHandler(
            IConfiguration configuration,
            ILogger<IBotFrameworkHttpAdapter> logger)
        {

            // Bind the configuration to our config model
            _config = new SelfServiceConfig();
            configuration.Bind(_config);

            // Setup the logger
            _logger = logger;
        }


        private Attachment DoBasicHelp()
        {

            // Connect to DB
            var db = new Database(_config);

            // Get list of actions
            var actions = db.GetAll();

            // Create an action list card
            var card = new SelfServiceProj.ActionListCard();
            var attachment = card.GenerateAttachment();

            return attachment;
        }


        private Attachment DoActionHelp(String action)
        {

            // Connect to DB
            var db = new Database(_config);

            // Get list of actions
            var actionDetails = db.GetAction(action);

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
            var card = new SelfServiceProj.WelcomeCard();
            var attachment = card.GenerateAttachment();

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
                attachment = DoBasicHelp();
            }
            else
            {
                // What was requested?
                switch (input)
                {
                    case "help":
                    case "list":

                        // Basic help (ie list actions)
                        attachment = DoBasicHelp();
                        break;

                    case string s when Regex.IsMatch(s, @"^help\s+[0-9a-zA-Z]+$"):

                        // Specific action help
                        var cmd = input.Split(" ");
                        if (cmd.Length != 2)
                        {
                            attachment = DoBasicHelp();
                        }
                        else
                        {
                            attachment = DoActionHelp(cmd[1]);
                        }
                        break;

                    default:
                        attachment = DoBasicHelp();
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
