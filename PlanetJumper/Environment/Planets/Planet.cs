﻿using System;
using OpenTK;
using amulware.Graphics;

namespace PlanetJumper.Environment
{
    abstract class Planet : DrawableWorldObject<PlanetGameEnvironment>
    {
        private static int planetCounter = 0;

        protected float radius;

        public float Radius { get { return this.radius; } }
        public float Volume { get { return (float)(Math.PI * 4 * (this.Radius * this.Radius * this.Radius) / 3); } }

        public string ID { get; private set; }

        public Planet(PlanetGameEnvironment env, Vector2 position, float radius)
            : base(env)
        {
            this.position = position;
            this.radius = radius;
            this.ID = "planet" + (planetCounter++).ToString();
        }

        public override void Draw(UpdateEventArgs e)
        {
            this.environment.Graphics.PlanetGeometry.DrawSprite(this.position, 0, this.Radius);
        }

        public bool IsOnScreen()
        {
            return this.Position.X + this.Radius + 640 + LevelGenerator.SideBuffer > this.environment.Offset;
        }
    }
}
