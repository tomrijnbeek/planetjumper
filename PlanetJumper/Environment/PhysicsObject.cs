using System;
using OpenTK;

namespace PlanetJumper.Environment
{
    abstract class PhysicsObject : DrawableWorldObject<PlanetGameEnvironment>
    {
        protected abstract float G { get; }
        protected abstract float artificialG { get; }

        public PhysicsObject(PlanetGameEnvironment env)
            : base(env) { }

        protected void updateVelocity()
        {
            Vector2 acc = Vector2.Zero;

            foreach (Planet p in this.environment.Planets)
            {
                Vector2 d = p.Position - this.position;

                float a = this.G * p.Volume * 1 / d.LengthSquared;
                d.Normalize();

                if (p is RepellingPlanet)
                    acc -= a * d;
                else
                    acc += a * d;
            }

            // Artificial gravity if out of screen
            acc.Y += Math.Max(0, -this.position.Y - 360) * this.artificialG;
            acc.Y -= Math.Max(0, this.position.Y - 360) * this.artificialG;

            this.velocity += acc;
        }
    }
}
