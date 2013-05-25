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
        private LinkedList<Asteroid> asteroids { get { return this.environment.Asteroids; } }
        private Spacecore spacecore;

        #region Constants
        private const int minPlanets = 8;
        private const int maxPlanets = 10;
        public const float SideBuffer = 200;

        private const float asteroidChanceInitial = 0.001f;
        private const float asteroidChanceIncrease = 0.0001f;
        private const float asteroidChanceMax = 0.05f;

        private const float firePlanetChanceThreshold = 1000;
        private const float firePlanetChanceInitial = 0.01f;
        private const float firePlanetChanceIncrease = 0.005f;
        private const float firePlanetChanceMax = 0.17f;

        private const float repellingPlanetChanceThreshold = 4000;
        private const float repellingPlanetChanceInitial = 0.01f;
        private const float repellingPlanetChanceIncrease = 0.0025f;
        private const float repellingPlanetChanceMax = 0.06f;

        private const float spacecoreChance = 1 / 18000; // expected: once per hour
        #endregion

        private float asteroidChance = asteroidChanceInitial;
        private float fireplanetChance = 0;
        private float repellingPlanetChance = 0;

        private int fireplanetCount = 0;
        private int repellingPlanetCount = 0;

        public LevelGenerator(PlanetGameEnvironment env)
            : base(env)
        {
            // Generate initial planets
            /*for (int i = 0; i < (minPlanets + maxPlanets) * 0.5; i++)
            {
                float r = GameMath.Clamp(10, 80, (float)GlobalRandom.NormalDouble(45, 10));

                float x = 0, y = 0;
                do
                {
                    x = (float)GlobalRandom.NextDouble(-640 + SideBuffer + r, 640 - r + 3 * SideBuffer);
                    y = (float)GlobalRandom.NextDouble(-360 + r, 360 - r);
                } while (!checkPosition(new Vector2(x, y), 2 * r));

                this.environment.AddPlanet(this.selectFactory(), new Vector2(x, y), r);
            }*/
        }

        public override void Update(UpdateEventArgs e)
        {
            this.asteroidChance = Math.Min(asteroidChanceMax, this.asteroidChance + (float)e.ElapsedTimeInS * asteroidChanceIncrease);

            if (this.environment.Offset > firePlanetChanceThreshold)
                this.fireplanetChance = GameMath.Clamp(firePlanetChanceInitial, firePlanetChanceMax, this.fireplanetChance + (float)e.ElapsedTimeInS * firePlanetChanceIncrease);
            if (this.environment.Offset > repellingPlanetChanceThreshold)
                this.repellingPlanetChance = GameMath.Clamp(repellingPlanetChanceInitial, repellingPlanetChanceMax, this.repellingPlanetChance + (float)e.ElapsedTimeInS * repellingPlanetChanceIncrease);

            // Check for planets out of the screen
            while (!this.planets.First.Value.IsOnScreen())
            {
                this.environment.RemoveWorldObject(this.planets.First.Value.ID);

                if (this.planets.First.Value is FirePlanet)
                    this.fireplanetCount--;
                if (this.planets.First.Value is RepellingPlanet)
                    this.repellingPlanetCount--;

                this.environment.Planets.RemoveFirst();
            }

            // Check if spacecore is still visible
            if (this.spacecore != null && this.spacecore.Position.X + 645 + LevelGenerator.SideBuffer < this.environment.Offset)
            {
                this.environment.RemoveWorldObject("spacecore");
                this.spacecore = null;
            }

            // Always have at least a certain amount of planets on the playfield
            if (this.planets.Count < minPlanets)
                this.generatePlanet();

            if (GlobalRandom.NextDouble() < this.asteroidChance)
                this.generateAsteroid();

            if (GlobalRandom.NextDouble() < spacecoreChance && this.spacecore == null)
                this.generateSpacecore();
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

        private void generateAsteroid()
        {
            // Random radius
            float r = GameMath.Clamp(10, 18, (float)GlobalRandom.NormalDouble(14, 4));

            // y-coordinate
            float y = 0;
            do
            {
                y = (float)GlobalRandom.NextDouble(-360 + r, 360 - r);
            } while (!this.checkPosition(y, r));

            this.environment.AddAsteroid(new Vector2(640 + SideBuffer + r + this.environment.Offset, y), r);
        }

        private void generateSpacecore()
        {
            // y-coordinate
            float y = 0;
            do
            {
                y = (float)GlobalRandom.NextDouble(-345, 345);
            } while (!this.checkPosition(y, 15));

            this.spacecore = new Spacecore(this.environment, new Vector2(655 + SideBuffer + this.environment.Offset, y));
            this.environment.AddWorldObject("spacecore", this.spacecore);
        }

        private bool checkPosition(Vector2 position, float r)
        {
            foreach (Planet p in this.planets)
                if ((p.Position - position).LengthSquared <= (r + p.Radius) * (r + p.Radius))
                    return false;
            foreach (Asteroid a in this.asteroids)
                if ((a.Position - position).LengthSquared <= (r + a.Radius) * (r + a.Radius))
                    return false;

            return true;
        }
        private bool checkPosition(float y, float r)
        {
            return this.checkPosition(new Vector2(640 + SideBuffer + r + this.environment.Offset, y), r);
        }

        private IPlanetFactory selectFactory()
        {
            double a = GlobalRandom.NextDouble();
            if (a < this.fireplanetChance && this.fireplanetCount < 2 * minPlanets * this.fireplanetChance)
                return new FirePlanetFactory();
            else if (a < this.repellingPlanetChance + this.fireplanetChance && this.repellingPlanetCount < 2 * minPlanets * this.repellingPlanetChance)
                return new RepellingPlanetFactory();
            else
                return new OrdinaryPlanetFactory();
        }
    }
}
