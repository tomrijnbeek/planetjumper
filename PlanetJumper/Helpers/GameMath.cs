using System;
using OpenTK;
using amulware.Graphics;

namespace PlanetJumper.Helpers
{
    public static class GameMath
    {
        public static float Clamp(float min, float max, float value)
        {
            if (value <= min)
                return min;
            if (value >= max)
                return max;
            return value;
        }

        public static float Lerp(float from, float to, float t)
        {
            return from + (to - from) * GameMath.Clamp(0, 1, t);
        }

        #region Vector related stuff
        public static Vector2 Vector2FromRotation(float angle, float radius = 1)
        {
            return radius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public static bool IsInRectangle(Vector2 point, Vector2 topLeft, Vector2 bottomRight)
        {
            return point.X >= topLeft.X && point.X <= bottomRight.X && point.Y >= topLeft.Y && point.Y <= bottomRight.Y;
        }
        public static bool IsInRectangle(Vector2 point, float x, float y, float width, float height)
        {
            return IsInRectangle(point, new Vector2(x, y), new Vector2(x + width, y + height));
        }
        #endregion

        #region Colour related stuff
        public static Color ColorLerp(Color c1, Color c2, float n)
        {
            n = GameMath.Clamp(0, 1, n);
            return new Color((byte)(c1.R + n * (c2.R - c1.R)), (byte)(c1.G + n * (c2.G - c1.G)), (byte)(c1.B + n * (c2.B - c1.B)), (byte)(c1.A + n * (c2.A - c1.A)));
        }
        #endregion
    }
}
