using System;
using System.Collections.Generic;
using amulware.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using PlanetJumper.Environment;

namespace PlanetJumper
{
    static class PlanetJumper
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (JumperProgram program = new JumperProgram())
            {
                program.Run(50);
            }
        }
    }

    public class JumperProgram : Program
    {
        GraphicsManager graphics;
        PlanetGameEnvironment environment;

        public JumperProgram()
            : base(1280, 720)
        {
            this.WindowBorder = OpenTK.WindowBorder.Fixed;
        }

        protected override void OnLoad(EventArgs e)
        {
            this.graphics = new GraphicsManager();

            // Create worldobjects
            this.environment = new PlanetGameEnvironment(this, this.graphics);
            
            this.environment.AddPlanet(new OrdinaryPlanetFactory(), new Vector2(-490, -240), 30);
            this.environment.AddPlanet(new OrdinaryPlanetFactory(), new Vector2(-300, 270), 15);
            this.environment.AddPlanet(new OrdinaryPlanetFactory(), new Vector2(-200, 300), 35);
            this.environment.AddPlanet(new OrdinaryPlanetFactory(), new Vector2(80, -110), 70);
            this.environment.AddPlanet(new OrdinaryPlanetFactory(), new Vector2(310, -295), 25);
            this.environment.AddPlanet(new OrdinaryPlanetFactory(), new Vector2(640, 140), 65);
            this.environment.AddPlanet(new OrdinaryPlanetFactory(), new Vector2(800, -220), 40);
            this.environment.AddPlanet(new OrdinaryPlanetFactory(), new Vector2(920, 40), 35);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height);
            // Make sure to update anything custom made that relies on the screen resolution.
            int w = this.ClientRectangle.Width;
            int h = this.ClientRectangle.Height;
            // These matrices create a pixel perfect projection with a scale from 1:1 from the z=0 plane to the screen.
            this.graphics.SetMatrices(
                this.environment.CameraMatrix * Matrix4.LookAt(-2f * Vector3.UnitZ, Vector3.UnitZ, -Vector3.UnitY),
                Matrix4.CreatePerspectiveOffCenter(-w / 4f, w / 4f, h / 4f, -h / 4f, 1f, 64f),
                Matrix4.LookAt(-2f * Vector3.UnitZ, Vector3.UnitZ, -Vector3.UnitY)
            );
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
            // Close program on shift + escape
            if (this.Keyboard[Key.ShiftLeft] && this.Keyboard[Key.Escape])
                this.Close();

            this.environment.Update(e);
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            this.environment.Draw(e);

            this.graphics.BackgroundSurface.Render();
            this.graphics.PlanetSurface.Render();
            this.graphics.JumperSurface.Render();
            this.graphics.TrailSurface.Render();
            this.graphics.SpaceCoreSurface.Render();
            this.graphics.AsteroidSurface.Render();

            this.SwapBuffers();
        }
    }
}