using System;
using OpenTK;
using amulware.Graphics;

namespace PlanetJumper.Environment
{
    class Asteroid : DrawableWorldObject<PlanetGameEnvironment>
    {
        private static int asteroidCounter = 0;

        public string ID { get; private set; }

        private float radius;

        public float Radius { get { return this.radius; } }

        public Asteroid(PlanetGameEnvironment env, Vector2 position, float radius)
            : base(env)
        {
            this.position = position;
            this.radius = radius;

            this.ID = "asteroid" + asteroidCounter++;
        }

        public override void Draw(UpdateEventArgs e)
        {
            this.environment.Graphics.AsteroidGeometry.DrawSprite(this.position, (float)(e.TimeInS), this.radius);
        }

        public bool IsOnScreen()
        {
            return this.Position.X + this.Radius + 640 + LevelGenerator.SideBuffer > this.environment.Offset;
        }
    }
}
