using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;


namespace SelfServiceProj
{
    public class BaseCard
    {
        // Setup member variables
        protected AdaptiveCard _card;

        public BaseCard()
        {
            _card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3));
        }

        public Attachment GenerateAttachment()
        {

            // Convert the card to JSON
            var cardJson = _card.ToJson();

            // Build an attachment object
            var attachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(cardJson),
            };
            return attachment;
        }
    }
}
