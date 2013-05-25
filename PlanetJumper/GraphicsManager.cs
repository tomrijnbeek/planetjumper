using System;
using amulware.Graphics;
using OpenTK;
using PlanetJumper.Environment;

namespace PlanetJumper
{
    class GraphicsManager
    {
        // Matrices
        Matrix4Uniform modelview;
        Matrix4Uniform projection;

        public QuadSurface<UVColorVertexData> PlanetSurface { get; private set; }
        public QuadSurface<UVColorVertexData> AsteroidSurface { get; private set; }
        public QuadSurface<UVColorVertexData> SpaceCoreSurface { get; private set; }
        public QuadSurface<UVColorVertexData> JumperSurface { get; private set; }
        public QuadSurface<PrimitiveVertexData> TrailSurface { get; private set; }

        public Sprite2DGeometry PlanetGeometry { get; private set; }
        public Sprite2DGeometry AsteroidGeometry { get; private set; }
        public Sprite2DGeometry SpaceCoreGeometry { get; private set; }
        public Sprite2DGeometry JumperGeometry { get; private set; }
        public PrimitiveGeometry TrailGeometry { get; private set; }

        public GraphicsManager()
        {
            // Load Shader Programs.
            ShaderProgram simpleShader = new ShaderProgram("data/shaders/simple_vs.glsl", "data/shaders/simple_fs.glsl");
            ShaderProgram uvShader = new ShaderProgram("data/shaders/uvcolor_vs.glsl", "data/shaders/uvcolor_fs.glsl");

            // Create matrix uniforms used for rendering.
            this.modelview = new Matrix4Uniform("modelviewMatrix");
            this.projection = new Matrix4Uniform("projectionMatrix");

            // Create the surfaces
            #region Planet Surface
            Texture t = new Texture("data/graphics/planet.png");

            this.PlanetSurface = new QuadSurface<UVColorVertexData>();
            this.PlanetSurface.AddSettings(
                this.modelview,
                this.projection,
                new TextureUniform("diffusetexture", t),
                SurfaceDepthMaskSetting.DontMask,
                SurfaceBlendSetting.Alpha
            );

            this.PlanetSurface.SetShaderProgram(uvShader);

            this.PlanetGeometry = new Sprite2DGeometry(this.PlanetSurface);
            this.PlanetGeometry.Size = new Vector2(2, 2);
            #endregion

            #region Asteroid Surface
            t = new Texture("data/graphics/asteroid.png");

            this.AsteroidSurface = new QuadSurface<UVColorVertexData>();
            this.AsteroidSurface.AddSettings(
                this.modelview,
                this.projection,
                new TextureUniform("diffusetexture", t),
                SurfaceDepthMaskSetting.DontMask,
                SurfaceBlendSetting.Alpha
            );

            this.AsteroidSurface.SetShaderProgram(uvShader);

            this.AsteroidGeometry = new Sprite2DGeometry(this.AsteroidSurface);
            this.AsteroidGeometry.Size = new Vector2(2, 2);
            #endregion

            #region Space Core Surface
            t = new Texture("data/graphics/spacecore.png");

            this.SpaceCoreSurface = new QuadSurface<UVColorVertexData>();
            this.SpaceCoreSurface.AddSettings(
                this.modelview,
                this.projection,
                new TextureUniform("diffusetexture", t),
                SurfaceDepthMaskSetting.DontMask,
                SurfaceBlendSetting.Alpha
            );

            this.SpaceCoreSurface.SetShaderProgram(uvShader);

            this.SpaceCoreGeometry = new Sprite2DGeometry(this.SpaceCoreSurface);
            this.SpaceCoreGeometry.Size = new Vector2(30, 30);
            #endregion

            #region Jumper Surface
            t = new Texture("data/graphics/jumper.png");

            this.JumperSurface = new QuadSurface<UVColorVertexData>();
            this.JumperSurface.AddSettings(
                this.modelview,
                this.projection,
                new TextureUniform("diffusetexture", t),
                SurfaceDepthMaskSetting.DontMask,
                SurfaceBlendSetting.Alpha
            );

            this.JumperSurface.SetShaderProgram(uvShader);

            this.JumperGeometry = new Sprite2DGeometry(this.JumperSurface);
            this.JumperGeometry.Size = Jumper.Size;
            #endregion

            #region Trail Surface
            this.TrailSurface = new QuadSurface<PrimitiveVertexData>();
            this.TrailSurface.AddSettings(
                this.modelview,
                this.projection,
                SurfaceDepthMaskSetting.DontMask,
                SurfaceBlendSetting.Alpha
            );

            this.TrailSurface.SetShaderProgram(simpleShader);

            this.TrailGeometry = new PrimitiveGeometry(this.TrailSurface);
            this.TrailGeometry.Color = Color.Cyan;
            #endregion
        }

        public void SetMatrices(Matrix4? modelview, Matrix4? projection)
        {
            if (modelview.HasValue)
                this.modelview.Matrix = modelview.Value;
            if (projection.HasValue)
                this.projection.Matrix = projection.Value;
        }
    }
}