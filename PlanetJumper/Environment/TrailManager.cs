using System;
using System.Collections.Generic;
using amulware.Graphics;
using OpenTK;
using PlanetJumper.Helpers;

namespace PlanetJumper.Environment
{
    class TrailManager : DrawableWorldObject<PlanetGameEnvironment>
    {
        LinkedList<TrailPart> trail;

        public TrailManager(PlanetGameEnvironment env)
            : base(env)
        {
            this.trail = new LinkedList<TrailPart>();
        }

        public override void Draw(UpdateEventArgs e)
        {
            foreach (TrailPart part in this.trail)
                part.Draw(this.environment.Graphics.TrailGeometry);
        }

        public void AddTrail(Vector2 from, Vector2 to)
        {
            this.trail.AddLast(new TrailPart(from, to));
        }
    }

    struct TrailPart
    {
        public Vector2 From, To;
        public Color Color;

        public TrailPart(Vector2 from, Vector2 to)
        {
            this.From = from;
            this.To = to;
            this.Color = Color.Black;
            this.calculateColor();
        }

        private void calculateColor()
        {
            float l = (this.From - this.To).Length;
            this.Color = Color.FromHSVA(GameMath.Lerp(MathHelper.PiOver3 * 2, 0, (l - 6.4f) / 12f), 1, 1);
        }

        public void Draw(PrimitiveGeometry geo)
        {
            geo.Color = this.Color;
            geo.DrawLine(this.From, this.To);
        }
    }
}
