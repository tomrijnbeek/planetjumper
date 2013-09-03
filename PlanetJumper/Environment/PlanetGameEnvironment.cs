using System;
using System.Collections.Generic;
using amulware.Graphics;
using OpenTK;
using OpenTK.Input;
using PlanetJumper.Helpers;

namespace PlanetJumper.Environment
{
    class PlanetGameEnvironment : GameEnvironment<PlanetGameEnvironment>
    {
        public enum GameState { ALIVE, DEAD };

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
        public float Score
        {
            get;
            private set;
        }
        public GameState State { get; private set; }

        public LinkedList<Planet> Planets { get; private set; }
        public LinkedList<Asteroid> Asteroids { get; private set; }
        public TrailManager Trail { get; private set; }

        private float speed = 64;
        
        public PlanetGameEnvironment(Program p, GraphicsManager graphics)
            : base(p)
        {
            this.Graphics = graphics;
            this.Planets = new LinkedList<Planet>();
            this.Asteroids = new LinkedList<Asteroid>();
            this.Trail = new TrailManager(this);
            this.AddWorldObject("trail", this.Trail);
            this.CameraMatrix = Matrix4.Identity;

            this.AddWorldObject("generator", new LevelGenerator(this));
            this.AddWorldObject("jumper", new Jumper(this, new Vector2(-500, 0)));

            this.State = GameState.ALIVE;
        }

        public override void Update(UpdateEventArgs e)
        {
            this.Offset = Math.Max(this.Offset + this.speed * (float)e.ElapsedTimeInS, this.GetWorldObject<Jumper>("jumper").Position.X - 540);
            if (this.State == GameState.ALIVE)
                this.Score = (int)(0.1f * this.Offset);
            this.speed += (float)(e.ElapsedTimeInS) * 2.0f;
            this.updateMatrices(e);
            base.Update(e);
        }

        private void updateMatrices(UpdateEventArgs e)
        {
            Jumper jumper = this.GetWorldObject<Jumper>("jumper");
            if (GameMath.IsInRectangle(jumper.Position, this.Offset - 620, -340, 1240, 680))
                this.CameraMatrix = Matrix4.CreateTranslation(-this.Offset, 0, 0);
            else
            {
                float top = Math.Max(360, jumper.Position.Y + 20);
                float bottom = Math.Min(-360, jumper.Position.Y - 20);

                Matrix4 translation = Matrix4.CreateTranslation(-this.Offset, -0.5f * (top + bottom), 0);
                Matrix4 scale = Matrix4.Scale(Math.Min(1, 720 / (top - bottom)));

                this.CameraMatrix = translation * scale;
            }

            this.Graphics.SetMatrices(this.CameraMatrix * Matrix4.LookAt(-2f * Vector3.UnitZ, Vector3.UnitZ, -Vector3.UnitY), null, null);
        }

        public override void Draw(UpdateEventArgs e)
        {
            this.Graphics.BackgroundGeometry.DrawSprite(Vector2.Zero);

            base.Draw(e);

            this.Graphics.ScoreGeometry.DrawString(Vector2.Zero, this.Score.ToString(), 1);

            if (this.State == GameState.DEAD)
            {
                this.Graphics.TrailGeometry.Color = Color.Red;
                this.Graphics.TrailGeometry.DrawLine(-640 + this.Offset, -360, 640 + this.Offset, 360);
                this.Graphics.TrailGeometry.DrawLine(640 + this.Offset, -360, -640 + this.Offset, 360);
            }
        }

        public void AddPlanet(IPlanetFactory factory, Vector2 position, float radius)
        {
            Planet p = factory.Create(this, position, radius);
            this.Planets.AddLast(p);
            this.AddWorldObject(p.ID, p);
        }
        public void AddAsteroid(Vector2 position, float radius)
        {
            Asteroid a = new Asteroid(this, position, radius);
            this.Asteroids.AddLast(a);
            this.AddWorldObject(a.ID, a);
        }

        public void Die()
        {
            Console.WriteLine("Score: " + this.Offset.ToString());
            this.State = GameState.DEAD;
        }
    }
}
