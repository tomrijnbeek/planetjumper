﻿using System;
using System.Collections.Generic;
using amulware.Graphics;
using OpenTK;
using OpenTK.Input;
using PlanetJumper.Helpers;

namespace PlanetJumper.Environment
{
    class PlanetGameEnvironment : GameEnvironment<PlanetGameEnvironment>
    {
        // tmp
        bool dead = false;

        public GraphicsManager Graphics
        {
            get;
            private set;
        }
        public Matrix4 CameraMatrix
        {
            get;
            private set;
        }
        public float Offset
        {
            get;
            private set;
        }

        public LinkedList<Planet> Planets { get; private set; }
        public TrailManager Trail { get; private set; }
        
        public PlanetGameEnvironment(Program p, GraphicsManager graphics)
            : base(p)
        {
            this.Graphics = graphics;
            this.Planets = new LinkedList<Planet>();
            this.Trail = new TrailManager(this);
            this.AddWorldObject("trail", this.Trail);
            this.CameraMatrix = Matrix4.Identity;

            this.AddWorldObject("generator", new LevelGenerator(this));
        }

        public override void Update(UpdateEventArgs e)
        {
            if (!this.dead)
            {
                this.Offset += 64 * (float)e.ElapsedTimeInS;
                base.Update(e);
                this.updateMatrices(e);
            }
        }

        private void updateMatrices(UpdateEventArgs e)
        {
            Jumper jumper = this.GetWorldObject<Jumper>("jumper");
            if (GameMath.IsInRectangle(jumper.Position, this.Offset - 620, -340, 1240, 680))
                this.CameraMatrix = Matrix4.CreateTranslation(-this.Offset, 0, 0);
            else
            {
                float top = Math.Max(360, jumper.Position.Y + 20);
                float right = Math.Max(this.Offset + 640, jumper.Position.X + 20);
                float bottom = Math.Min(-360, jumper.Position.Y - 20);
                float left = Math.Min(this.Offset - 640, jumper.Position.X - 20);

                Matrix4 translation = Matrix4.CreateTranslation(-0.5f * (left + right), -0.5f * (top + bottom), 0);
                Matrix4 scale = Matrix4.Scale(Math.Min(1280 / (right - left), 720 / (top - bottom)));

                this.CameraMatrix = translation * scale;
            }

            this.Graphics.SetMatrices(this.CameraMatrix * Matrix4.LookAt(-2f * Vector3.UnitZ, Vector3.UnitZ, -Vector3.UnitY), null);
        }

        public override void Draw(UpdateEventArgs e)
        {
            base.Draw(e);

            if (this.dead)
            {
                this.Graphics.TrailGeometry.Color = Color.Red;
                this.Graphics.TrailGeometry.DrawLine(-640, -360, 640, 360);
                this.Graphics.TrailGeometry.DrawLine(640, -360, -640, 360);
            }
        }

        public void AddPlanet(IPlanetFactory factory, Vector2 position, float radius)
        {
            Planet p = factory.Create(this, position, radius);
            this.Planets.AddLast(p);
            this.AddWorldObject(p.ID, p);
        }

        public void Die()
        {
            this.dead = true;
        }
    }
}
