using AdaptiveCards;

namespace SelfServiceProj
{
    public class WelcomeCard : BaseCard
    {

        public WelcomeCard()
        {
            _card.Body.Add(new AdaptiveTextBlock()
            {
                Text = "Welcome to the Self Service Bot",
                Size = AdaptiveTextSize.Large
            });
            _card.Body.Add(new AdaptiveTextBlock()
            {
                Text = "For a list of possible actions click the button below",
                Size = AdaptiveTextSize.Small
            });
            _card.Actions.Add(new AdaptiveSubmitAction()
            {
                Title = "List Actions",
                Data = "help"
            });
        }
    }
}
