using PartyGames.Drawables;

namespace PartyGames.ViewModels
{
    /// <summary>
    /// A single die result paired with the shape used to draw it
    /// </summary>
    public class DieResult
    {
        public DieResult(int value, int sides)
        {
            Value = value;
            Shape = new DieShapeDrawable(sides, value);
        }

        public int Value { get; }
        public IDrawable Shape { get; }
    }
}
