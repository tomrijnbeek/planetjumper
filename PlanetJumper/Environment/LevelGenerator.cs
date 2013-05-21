using System;
using System.Collections.Generic;
using OpenTK;
using amulware.Graphics;
using PlanetJumper.Helpers;

namespace PlanetJumper.Environment
{
    class LevelGenerator : WorldObject<PlanetGameEnvironment>
    {
        private LinkedList<Planet> planets { get { return this.environment.Planets; } }

        #region Constants
        private const int planetThreshold = 8;
        public const float SideBuffer = 200;
        #endregion

        public LevelGenerator(PlanetGameEnvironment env)
            : base(env)
        {

        }

        public override void Update(UpdateEventArgs e)
        {
            // Check for planets out of the screen
            while (!this.planets.First.Value.IsOnScreen())
            {
                this.environment.RemoveWorldObject(this.planets.First.Value.ID);
                this.environment.Planets.RemoveFirst();
            }

            // Always have at least a certain amount of planets on the playfield
            while (this.planets.Count < planetThreshold)
                this.generatePlanet();
        }

        private void generatePlanet()
        {
            // Random radius
            float r = GameMath.Clamp(10, 80, (float)GlobalRandom.NormalDouble(45, 10));

            // y-coordinate
            float y = 0;
            do
            {
                y = (float)GlobalRandom.NextDouble(-360 + r, 360 - r);
            } while (!this.checkPosition(y, r));

            this.environment.AddPlanet(this.selectFactory(), new Vector2(640 + SideBuffer + r + this.environment.Offset, y), r);
        }

        private bool checkPosition(Vector2 position, float r)
        {
            foreach (Planet p in this.planets)
                if ((p.Position - position).LengthSquared <= (r + p.Radius) * (r + p.Radius))
                    return false;

            return true;
        }
        private bool checkPosition(float y, float r)
        {
            return this.checkPosition(new Vector2(640 + SideBuffer + r + this.environment.Offset, y), r);
        }
        private IPlanetFactory selectFactory()
        {
            if (GlobalRandom.NextDouble() < 0.2)
                return new FirePlanetFactory();
            else
                return new OrdinaryPlanetFactory();
        }
    }
}
