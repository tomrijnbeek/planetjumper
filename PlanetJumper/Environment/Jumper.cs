using System;
using OpenTK;
using OpenTK.Input;
using amulware.Graphics;
using PlanetJumper.Helpers;

namespace PlanetJumper.Environment
{
    class Jumper : DrawableWorldObject<PlanetGameEnvironment>
    {
        public static readonly Vector2 Size = new Vector2(16, 32);
        private const float G = 0.4f;
        private const float artificialG = 0.1f;
        private const float jumpStrengthInitial = 100;
        private const float jumpStrengthFinal = 800;
        private const float jumpStrengthTime = 1;

        private Planet planet;
        private float planetPosition;

        private float builtStrength = 0;

        private float angle
        {
            get
            {
                if (this.planet != null)
                    return this.planetPosition + MathHelper.PiOver2;
                else
                    return (float)Math.Atan2(this.velocity.Y, this.velocity.X) + MathHelper.PiOver2;
            }
        }

        public Jumper(PlanetGameEnvironment env, Vector2 position)
            : base(env)
        {
            this.position = position;
            this.velocity = new Vector2(300, 100);
        }

        public override void Update(UpdateEventArgs e)
        {
            if (planet == null)
            {
                Vector2 acc = Vector2.Zero;

                foreach (Planet p in this.environment.Planets)
                {
                    Vector2 d = p.Position - this.position;

                    // Check if you are standing on planet
                    if (d.LengthSquared <= ((p.Radius + Jumper.Size.Y * 0.5f) * (p.Radius + Jumper.Size.Y * 0.5f)))
                    {
                        this.lockToPlanet(p);
                        break;
                    }

                    float a = G * p.Volume * 1 / d.LengthSquared;
                    d.Normalize();
                    acc += a * d;
                }

                // Artificial gravity if out of screen
                acc.X += Math.Max(0, -this.position.X - this.environment.Offset - 640) * artificialG;
                acc.X -= Math.Max(0, this.position.X - this.environment.Offset - 640) * artificialG;
                acc.Y += Math.Max(0, -this.position.Y - 360) * artificialG;
                acc.Y -= Math.Max(0, this.position.Y - 360) * artificialG;

                if (this.planet == null)
                    this.velocity += acc;
            }
            if (planet != null) // yes, not elseif, because planet might become set in the previous if expression
            {
                this.position = this.planet.Position + GameMath.Vector2FromRotation(this.planetPosition, this.planet.Radius + Jumper.Size.Y * 0.5f);
                this.velocity = Vector2.Zero;

                if (this.keyboard[Key.Left])
                    this.planetPosition += 150 * (float)e.ElapsedTimeInS / this.planet.Radius;
                if (this.keyboard[Key.Right])
                    this.planetPosition -= 150 * (float)e.ElapsedTimeInS / this.planet.Radius;
                if (this.keyboard[Key.Space])
                {
                    if (this.builtStrength == 0)
                        this.builtStrength = jumpStrengthInitial;
                    else
                        this.builtStrength = Math.Min(jumpStrengthFinal, this.builtStrength + (jumpStrengthFinal - jumpStrengthInitial) / jumpStrengthTime * (float)e.ElapsedTimeInS);
                    Console.WriteLine(this.builtStrength);
                }
                else if (this.builtStrength > 0)
                {
                    this.planet = null;
                    this.velocity = GameMath.Vector2FromRotation(this.planetPosition, this.builtStrength);
                    this.builtStrength = 0;
                }
            }

            Vector2 prev = this.position;
            base.Update(e);
            if (this.position != prev)
                this.environment.Trail.AddTrail(prev, this.position);
        }

        public override void Draw(UpdateEventArgs e)
        {
            this.environment.Graphics.JumperGeometry.DrawSprite(this.position, this.angle);
        }

        private void lockToPlanet(Planet p)
        {
            Vector2 delta = this.position - p.Position;
            this.planet = p;
            this.planetPosition = (float)Math.Atan2(delta.Y, delta.X);

            if (p is FirePlanet)
                this.environment.Die();
        }
    }
}
