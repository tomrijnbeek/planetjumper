using System;
using OpenTK;
using amulware.Graphics;

namespace PlanetJumper.Environment
{
    class RepellingPlanet : Planet
    {
        public RepellingPlanet(PlanetGameEnvironment env, Vector2 position, float radius)
            : base(env, position, radius)
        { }

        public override void Draw(UpdateEventArgs e)
        {
            this.environment.Graphics.PlanetGeometry.Color = Color.LightBlue;
            base.Draw(e);
        }
    }
}
