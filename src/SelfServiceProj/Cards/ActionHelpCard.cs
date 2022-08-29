using AdaptiveCards;

namespace SelfServiceProj
{
    public class ActionHelpCard : BaseCard
    {

        public ActionHelpCard()
        {
            _card.Body.Add(new AdaptiveTextBlock()
            {
                Text = "Help Me!!!",
                Size = AdaptiveTextSize.Large
            });
        }
    }
}
