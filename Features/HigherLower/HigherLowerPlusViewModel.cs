using PartyGames.Features.Blackjack;

namespace PartyGames.Features.HigherLower
{
    /// <summary>
    /// Drinking variant of Higher or Lower. The difference between the cards is the number of sips.
    /// Correct guess: the other player takes the sips. Wrong guess: the guesser takes the sips.
    /// Aces always count against the guesser. Ties and double Aces cost 1 sip.
    /// Straight: correct gives 2 sips; wrong costs the difference.
    /// </summary>
    public class HigherLowerPlusViewModel : HigherLowerViewModel
    {
        public HigherLowerPlusViewModel()
        {
            Title = "Higher or Lower +";
        }

        public override string HeaderText => "HIGHER OR LOWER +";

        public override string FontFamilyName => MonoFont;

        public override Microsoft.Maui.TextAlignment StatusAlignment => Microsoft.Maui.TextAlignment.Start;

        protected override string StartPrompt => ">> Tap the stack to start";

        protected override string GameOverLine => ">> Game Over";

        public override Color HigherFill => Color.FromArgb("#1E3A0F");

        public override Color StraightFill => Color.FromArgb("#1E3A0F");

        public override Color LowerFill => Color.FromArgb("#1E3A0F");

        public override Color PlayAgainFill => Color.FromArgb("#1E3A0F");

        public override Color ButtonBorder => Color.FromArgb("#FFD700");

        public override Color ButtonText => Color.FromArgb("#FFD700");

        public override Color PlayAgainBorder => Color.FromArgb("#FFD700");

        public override double PlayAgainBorderWidth => 2;

        protected override GuessOutcome Judge(GuessKind guess, Card previous, Card next)
        {
            var (correct, sips) = Score(guess, previous, next);

            int taker = correct ? OtherPlayer : CurrentPlayer;
            string unit = sips == 1 ? "SIP" : "SIPS";
            string emphasis = $"{sips} {unit}";
            string verdict = correct ? "// CORRECT //" : "// WRONG //";

            string message = $">> Player {CurrentPlayer} = {verdict}\n>> Player {taker} = TAKE {emphasis}";
            return new GuessOutcome(correct, message, taker, sips, emphasis);
        }

        protected override string PromptLine(int player) => $">> Player {player} = HIGHER OR LOWER?";

        private static (bool Correct, int Sips) Score(GuessKind guess, Card previous, Card next)
        {
            bool previousAce = previous.Rank == 1;
            bool nextAce = next.Rank == 1;

            // Two Aces: always a 1 sip penalty for the guesser
            if (previousAce && nextAce)
                return (false, 1);

            if (guess == GuessKind.Straight)
            {
                if (Strength(previous) == Strength(next))
                    return (true, 2);

                if (previousAce || nextAce)
                {
                    // Use the Ace value (1 or 14) that is closer to the other card
                    int other = previousAce ? Strength(next) : Strength(previous);
                    return (false, Math.Min(Math.Abs(other - 1), Math.Abs(other - 14)));
                }

                return (false, Math.Abs(Strength(next) - Strength(previous)));
            }

            // Higher: a showcase Ace counts as 14 and a drawn Ace counts as 1.
            // Lower: a showcase Ace counts as 1 and a drawn Ace counts as 14.
            bool higher = guess == GuessKind.Higher;
            int previousValue = previousAce ? (higher ? 14 : 1) : Strength(previous);
            int nextValue = nextAce ? (higher ? 1 : 14) : Strength(next);

            // Tie: the guesser takes 1 sip
            if (previousValue == nextValue)
                return (false, 1);

            bool correct = higher ? nextValue > previousValue : nextValue < previousValue;
            return (correct, Math.Abs(nextValue - previousValue));
        }
    }
}
