using AdaptiveCards;

namespace SelfServiceProj
{
    public class ActionListCard : BaseCard
    {

        public ActionListCard()
        {
            _card.Body.Add(new AdaptiveTextBlock()
            {
                Text = "Help Me!!!",
                Size = AdaptiveTextSize.Large
            });
        }
    }
}
