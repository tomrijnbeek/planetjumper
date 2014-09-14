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
        Matrix4Uniform hudMatrix;
        Matrix4Uniform projection;

		public IndexedSurface<UVColorVertexData> BackgroundSurface { get; private set; }
		public IndexedSurface<UVColorVertexData> PlanetSurface { get; private set; }
		public IndexedSurface<UVColorVertexData> AsteroidSurface { get; private set; }
		public IndexedSurface<UVColorVertexData> SpaceCoreSurface { get; private set; }
		public IndexedSurface<UVColorVertexData> JumperSurface { get; private set; }
		public IndexedSurface<PrimitiveVertexData> TrailSurface { get; private set; }
		public IndexedSurface<UVColorVertexData> ScoreSurface { get; private set; }
		public IndexedSurface<PrimitiveVertexData> OverlaySurface { get; private set; }

        public Sprite2DGeometry BackgroundGeometry { get; private set; }
        public Sprite2DGeometry PlanetGeometry { get; private set; }
        public Sprite2DGeometry AsteroidGeometry { get; private set; }
        public Sprite2DGeometry SpaceCoreGeometry { get; private set; }
        public Sprite2DGeometry JumperGeometry { get; private set; }
        public PrimitiveGeometry TrailGeometry { get; private set; }
        public FontGeometry ScoreGeometry { get; private set; }
        public PrimitiveGeometry OverlayGeometry { get; private set; }

        public GraphicsManager()
        {
            // Load Shader Programs.
			ShaderProgram simpleShader = new ShaderProgram(VertexShader.FromFile("data/shaders/simple_vs.glsl"), FragmentShader.FromFile("data/shaders/simple_fs.glsl"));
			ShaderProgram uvShader = new ShaderProgram(VertexShader.FromFile("data/shaders/uvcolor_vs.glsl"), FragmentShader.FromFile("data/shaders/uvcolor_fs.glsl"));

            // Create matrix uniforms used for rendering.
            this.modelview = new Matrix4Uniform("modelviewMatrix");
            this.projection = new Matrix4Uniform("projectionMatrix");
            this.hudMatrix = new Matrix4Uniform("modelviewMatrix");

            // Font
            Font quartz = Font.FromJsonFile("data/fonts/Quartz.json");

            // Create the surfaces
            #region Background surface
            Texture t = new Texture("data/graphics/omega-nebula.jpg");

			this.BackgroundSurface = new IndexedSurface<UVColorVertexData>();
            this.BackgroundSurface.AddSettings(
                this.hudMatrix,
                this.projection,
                new TextureUniform("diffusetexture", t),
                SurfaceDepthMaskSetting.DontMask,
                SurfaceBlendSetting.Alpha
            );

            this.BackgroundSurface.SetShaderProgram(uvShader);

            this.BackgroundGeometry = new Sprite2DGeometry(this.BackgroundSurface);
            this.BackgroundGeometry.Size = new Vector2(1280, -720);
            this.BackgroundGeometry.Color.A = (byte)100;
            #endregion

            #region Planet Surface
            t = new Texture("data/graphics/planet.png");

			this.PlanetSurface = new IndexedSurface<UVColorVertexData>();
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

			this.AsteroidSurface = new IndexedSurface<UVColorVertexData>();
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

			this.SpaceCoreSurface = new IndexedSurface<UVColorVertexData>();
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

			this.JumperSurface = new IndexedSurface<UVColorVertexData>();
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
			this.TrailSurface = new IndexedSurface<PrimitiveVertexData>();
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

            #region Score Surface
            t = new Texture("data/fonts/Quartz.png");

			this.ScoreSurface = new IndexedSurface<UVColorVertexData>();
            this.ScoreSurface.AddSettings(
                this.hudMatrix,
                this.projection,
                new TextureUniform("diffusetexture", t),
                SurfaceDepthMaskSetting.DontMask,
                SurfaceBlendSetting.Alpha
            );

            this.ScoreSurface.SetShaderProgram(uvShader);

            this.ScoreGeometry = new FontGeometry(this.ScoreSurface, quartz);
            this.ScoreGeometry.Height = 32;
            this.ScoreGeometry.SizeCoefficient = new Vector2(1, -1);
            #endregion

            #region Overlay surface
			this.OverlaySurface = new IndexedSurface<PrimitiveVertexData>();
            this.OverlaySurface.AddSettings(
                this.hudMatrix,
                this.projection,
                SurfaceDepthMaskSetting.DontMask,
                SurfaceBlendSetting.Alpha
            );
            
            this.OverlaySurface.SetShaderProgram(simpleShader);

            this.OverlayGeometry = new PrimitiveGeometry(this.OverlaySurface);
            this.OverlayGeometry.Color = Color.Cyan;
            this.OverlayGeometry.Color.A = (byte)150;
            #endregion
        }

        public void SetMatrices(Matrix4? modelview, Matrix4? projection, Matrix4? hud)
        {
            if (modelview.HasValue)
                this.modelview.Matrix = modelview.Value;
            if (projection.HasValue)
                this.projection.Matrix = projection.Value;
            if (hud.HasValue)
                this.hudMatrix.Matrix = hud.Value;
        }
    }
}