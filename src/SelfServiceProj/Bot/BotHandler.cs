using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace SelfServiceProj
{
    public class BotHandler : ActivityHandler
    {

        protected readonly SelfServiceConfig _config;

        
        public BotHandler(
            IConfiguration configuration,
            ILogger<IBotFrameworkHttpAdapter> logger)
        {
            _config = new SelfServiceConfig();
            configuration.Bind(_config);
        }


        protected override async Task OnMembersAddedAsync(
            IList<ChannelAccount> membersAdded,
            ITurnContext<IConversationUpdateActivity> turnContext,
            CancellationToken cancellationToken)
        {

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
        }


        protected override async Task OnMessageActivityAsync(
            ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {

            // Generate an appropriate response
            //var replyText = $"Echo: {turnContext.Activity.Text}";
            var attachment = GetHelp();

            // Send the response
            // await turnContext.SendActivityAsync(
            //     MessageFactory.Text(replyText, replyText),
            //     cancellationToken);
            await turnContext.SendActivityAsync(
                MessageFactory.Attachment(attachment),
                cancellationToken);
        }


        private Attachment GetHelp()
        {

            // Connect to DB
            var db = new Database(_config);

            // Get list of actions
            var actions = db.GetAll();

            // Create a help card
            var card = new SelfServiceProj.HelpCard();
            var attachment = card.GenerateAttachment();

            return attachment;
        }
    }
}
