using AdaptiveCards;

namespace SelfServiceProj
{
    public class HelpCard : BaseCard
    {

        public HelpCard()
        {
            _card.Body.Add(new AdaptiveTextBlock()
            {
                Text = "Help Me!!!",
                Size = AdaptiveTextSize.Large
            });
        }
    }
}
