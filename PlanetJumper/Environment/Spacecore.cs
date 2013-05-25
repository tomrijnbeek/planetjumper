using System;
using OpenTK;
using amulware.Graphics;

namespace PlanetJumper.Environment
{
    class Spacecore : DrawableWorldObject<PlanetGameEnvironment>
    {
        public Spacecore(PlanetGameEnvironment env, Vector2 position)
            : base(env)
        {
            this.position = position;
        }

        public override void Draw(UpdateEventArgs e)
        {
            this.environment.Graphics.SpaceCoreGeometry.DrawSprite(this.position);
        }
    }
}
