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

        public override void Update(UpdateEventArgs e)
        {
            while (this.trail.Count > 0 && this.trail.First.Value.Age > 3)
                this.trail.RemoveFirst();

            foreach (TrailPart part in this.trail)
                part.Update(e);
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

    class TrailPart
    {
        public Vector2 From, To;
        public Color Color;
        public float Age;

        public TrailPart(Vector2 from, Vector2 to)
        {
            this.From = from;
            this.To = to;
            this.Color = Color.Black;
            this.Age = 0;

            this.calculateColor();
        }

        private void calculateColor()
        {
            float l = (this.From - this.To).Length;
            this.Color = Color.FromHSVA(GameMath.Lerp(MathHelper.PiOver3 * 2, 0, (l - 6.4f) / 12f), 1, 1, 100);
        }

        public void Update(UpdateEventArgs e)
        {
            this.Age += (float)e.ElapsedTimeInS;
            this.Color.A = (byte)GameMath.Clamp(0, 100, 100.0f * (3.0f - this.Age));
        }

        public void Draw(PrimitiveGeometry geo)
        {
            geo.Color = this.Color;
            geo.DrawLine(this.From, this.To);
        }
    }
}
