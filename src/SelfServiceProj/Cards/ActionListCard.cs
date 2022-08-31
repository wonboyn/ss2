using AdaptiveCards;

namespace SelfServiceProj
{
    public class ActionListCard : BaseCard
    {

        public ActionListCard()
        {

            // Set card header
            _card.Body.Add(new AdaptiveTextBlock()
            {
                Text = "List of available actions",
                Size = AdaptiveTextSize.Large
            });

            // Setup columnset
            var columnset = new AdaptiveColumnSet();
        }
    }
}
