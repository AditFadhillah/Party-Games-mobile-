using Microsoft.Maui.Graphics;

namespace PartyGames.Features.DiceRoller
{
    /// <summary>
    /// Draws a die as a shape matching its number of sides, with its value centered
    /// </summary>
    public class DieShapeDrawable : IDrawable
    {
        public DieShapeDrawable(int sides, int value)
        {
            Sides = sides;
            Value = value;
        }

        public int Sides { get; }
        public int Value { get; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float cx = dirtyRect.Center.X;
            float cy = dirtyRect.Center.Y;
            float r = Math.Min(dirtyRect.Width, dirtyRect.Height) / 2f - 4f;

            PathF path = BuildShape(cx, cy, r);

            canvas.FillColor = Color.FromArgb("#FFE6CC");
            canvas.FillPath(path);
            canvas.StrokeColor = Color.FromArgb("#FF9500");
            canvas.StrokeSize = 2;
            canvas.DrawPath(path);

            canvas.FontColor = Color.FromArgb("#333333");
            canvas.FontSize = 22;
            canvas.DrawString(Value == 0 ? "?" : Value.ToString(), dirtyRect,
                HorizontalAlignment.Center, VerticalAlignment.Center);
        }

        private PathF BuildShape(float cx, float cy, float r)
        {
            return Sides switch
            {
                4 => Polygon(cx, cy, r, 3, -90),   // triangle
                6 => Polygon(cx, cy, r, 4, -45),   // square
                8 => Polygon(cx, cy, r, 4, -90),   // diamond
                10 => Circle(cx, cy, r),           // circle
                12 => Polygon(cx, cy, r, 5, -90),  // pentagon
                _ => Polygon(cx, cy, r, 6, -90),   // hexagon (d20)
            };
        }

        private static PathF Polygon(float cx, float cy, float r, int vertices, double startDegrees)
        {
            var path = new PathF();
            for (int i = 0; i < vertices; i++)
            {
                double angle = (startDegrees + 360.0 * i / vertices) * Math.PI / 180.0;
                float x = cx + r * (float)Math.Cos(angle);
                float y = cy + r * (float)Math.Sin(angle);
                if (i == 0)
                    path.MoveTo(x, y);
                else
                    path.LineTo(x, y);
            }
            path.Close();
            return path;
        }

        private static PathF Circle(float cx, float cy, float r)
        {
            var path = new PathF();
            path.AppendCircle(cx, cy, r);
            return path;
        }
    }
}
